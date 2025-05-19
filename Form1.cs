using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System;
using System.Text;
using System.Diagnostics;
using System.Drawing.Design;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Text.RegularExpressions;

namespace DocMgr
{
    public partial class DocMgr : Form
    {
        readonly int BUTTON_Y_SPACING = 40;
        readonly int MIN_BUTTON_WIDTH = 120;
        readonly int BUTTON_COLUMN_GAP = 10;
        List<Button> buttons;
        public static readonly Font SystemFont = new Font("Calibri", 14, FontStyle.Bold);
        Font font = Properties.Settings.Default.DefaultFont;
        string BackupsAndArchivesFolder = Properties.Settings.Default.BackupsAndArchivesFolder;
        //TEMP FOR USB BACKUP: string BackupsAndArchivesFolder = @"D:\BackupsAndArchives";
        string BackupFolder, ArchiveFolder;
        string ProjName;
        static MarginSt MyMargins;    // In twips; 1/1440 inch.  Twentieth of a point.
        static bool HaveMargins = false;

        string? CurrentFilePath;
        static string? ProjectPath, lastDocName;
        bool loadingDoc = false;
        int originalLeft;
        static readonly int BAD_INT = int.MinValue;
        static readonly string BASE_REGISTRY_KEY = @"Software\PatternScope Systems\DocMgr";

        static private PropertyGrid propertyGrid;
        static private MySettings settings;


        public class MySettings
        {
            [Category("Properties")]
            [Description("The path to the BackupsAndArchives folder.")]
            [Editor(typeof(FolderPathEditor), typeof(UITypeEditor))]
            public string BackupsAndArchivesFolder { get; set; }

            [Category("Properties")]
            [Description("The font to use in new documents.")]
            public Font DefaultFont { get; set; }

            [Category("Properties")]
            [Description("The left margin to use for printing in inches.")]
            //[Editor(typeof(FolderPathEditor), typeof(NullableIntConverter))]
            public string MarginLeft { get; set; }

            [Category("Properties")]
            [Description("The right margin to use for printing in inches.")]
            //[Editor(typeof(FolderPathEditor), typeof(NullableIntConverter))]
            public string MarginRight { get; set; }

            [Category("Properties")]
            [Description("The top margin to use for printing in inches.")]
            //[Editor(typeof(FolderPathEditor), typeof(NullableIntConverter))]
            public string MarginTop { get; set; }

            [Category("Properties")]
            [Description("The bottom margin to use for printing in inches.")]
            //[Editor(typeof(FolderPathEditor), typeof(NullableIntConverter))]
            public string MarginBottom { get; set; }
        }

        private Doc? Root = new Doc("Root");
        private static Point ButtonListStart { get; set; } = new Point(10, 78);
        public DocMgr()
        {
            InitializeComponent();
            ArrangeLayout();

            string version = GetSubstringUpToSecondPeriod(Application.ProductVersion);
            //MessageBox.Show("Version: " + version);
            Text = "DocMgr v" + version;
            this.KeyPreview = true; // Enable KeyPreview programmatically
        }

        static string GetSubstringUpToSecondPeriod(string input)
        {
            int firstDot = input.IndexOf('.');
            if (firstDot == -1) return input; // No period found, return the whole string

            int secondDot = input.IndexOf('.', firstDot + 1);
            if (secondDot == -1) return input; // Only one period found, return the whole string

            return input.Substring(0, secondDot);
        }

        private void ArrangeLayout()
        {
            Point newStart = ButtonListStart;
            newStart.Y = label2.Location.Y
                + label2.Height + 3;
            ButtonListStart = newStart;
            originalLeft = richTextBox.Left;
        }

        public enum ScrollBarType : uint
        {
            SbHorz = 0,
            SbVert = 1,
            SbCtl = 2,
            SbBoth = 3
        }

        public enum Message : uint
        {
            WM_VSCROLL = 0x0115,
            EM_SETSCROLLPOS = 0x04DD
        }

        public enum ScrollBarCommands : uint
        {
            SB_THUMBPOSITION = 4
        }// Scroll definitions.
        [DllImport("User32.dll")]
        public extern static int GetScrollPos(IntPtr hWnd, int nBar);

        [DllImport("User32.dll")]
        public extern static int SetScrollPos(IntPtr hWnd, int nBar, int nPos, Boolean bRedraw);

        [DllImport("User32.dll")]
        public extern static int SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        public extern static int GetLastError();

        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);// DllImports.

        // Constants for scroll bar direction
        private const int SB_VERT = 1; // Vertical scroll bar
        private const int SIF_RANGE = 0x1;
        private const int SIF_PAGE = 0x2;
        private const int SIF_POS = 0x4;
        private const int SIF_DISABLENOSCROLL = 0x8;
        private const int SIF_TRACKPOS = 0x10;
        private const int SIF_ALL = SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS;

        // SCROLLINFO struct
        [StructLayout(LayoutKind.Sequential)]
        private struct SCROLLINFO
        {
            public uint cbSize;
            public uint fMask;
            public int nMin;
            public int nMax;
            public uint nPage;
            public int nPos;
            public int nTrackPos;
        }

        // P/Invoke declaration
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int SetScrollInfo(IntPtr hwnd, int nBar, ref SCROLLINFO lpsi, bool redraw);

        // Method to set scroll position  DFW
        //public static void SetVerticalScrollPosition(Control control, int position)
        //{
        //    if (!control.IsHandleCreated)
        //    {
        //        throw new InvalidOperationException("Control handle is not created.");
        //    }

        //    SCROLLINFO si = new SCROLLINFO
        //    {
        //        cbSize = (uint)Marshal.SizeOf(typeof(SCROLLINFO)),
        //       //ORIG:  fMask = SIF_POS | SIF_RANGE | SIF_PAGE,
        //        fMask = SIF_POS,
        //        nMin = 0,
        //        nMax = 90000, // Example range; customize as needed
        //        nPage = 10, // Example page size
        //        nPos = position
        //    };

        //    // Set the scroll info
        //    int rc = SetScrollInfo(control.Handle, SB_VERT, ref si, true);
        //}

        private void SaveScrollPosition()
        {
            Doc? doc = FindDocByName(DocName.Text.TrimEnd(':'));

            if (doc != null && ProjectPath != null)
            {                                   // Save new scroll position in project:
                doc.ScrollPos = GetScrollPosition();
                string text = System.Text.Json.JsonSerializer.Serialize<Doc>(Root);
                File.WriteAllText(ProjectPath, text);
                return;
            }
        }

        private int GetScrollPosition()
        {
            return richTextBox.GetCharIndexFromPosition(new Point(0, 0));

            //ORIG: return GetScrollPos(richTextBox.Handle, (int)ScrollBarType.SbVert);


            //int pos = GetScrollPos(richTextBox.Handle, (int)ScrollBarType.SbVert);
            //int refinedPos = richTextBox.GetCharIndexFromPosition(new Point(0, 0));

            //return refinedPos;
        }

        //[StructLayout(LayoutKind.Sequential)]
        //public struct POINT
        //{
        //    public int x;
        //    public int y;
        //}

        private bool SetScrollPosition(int nPos)
        {
            //SetVerticalScrollPosition(richTextBox, nPos);    // DFW.
            //return true;

            richTextBox.SelectionStart = nPos;
            richTextBox.SelectionLength = 0;
            richTextBox.ScrollToCaret();
            return true;



            // DFW:
            //POINT pt = new POINT { x = 0, y = nPos };
            //IntPtr wParam = IntPtr.Zero;
            //IntPtr lParam = (IntPtr)(((int)ScrollBarCommands.SB_THUMBPOSITION) | (nPos << 16));
            //GCHandle handle = GCHandle.Alloc(pt, GCHandleType.Pinned);

            //try
            //{
            //    IntPtr ptr = handle.AddrOfPinnedObject();

            //    return SendMessage(richTextBox.Handle, (int)Message.EM_SETSCROLLPOS,
            //        wParam, ptr) != 0;
            //}
            //finally
            //{
            //    handle.Free();
            //}

        }

        private void ButtonSaveAs_Click(object sender, EventArgs e)
        {
            string? fileName = GetSaveFileName("RTF Files|*.rtf|All Files|*.*", true);

            if (string.IsNullOrEmpty(fileName))
                return;

            //if (File.Exists(fileName))
            //{
            //    CenterCursor(160, 82);
            //    if (MessageBox.Show("File " + fileName + " exists.  OK to overwrite?",
            //        "OK to overwrite?", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning,
            //        MessageBoxDefaultButton.Button2) // Sets Cancel as the default button.
            //            != DialogResult.OK)
            //        return;
            //}

            SaveScrollPosition();
            CurrentFilePath = fileName;
            DocName.Text = Path.GetFileNameWithoutExtension(fileName) + ":";
            buttonSaveDoc_Click(null, null);
            CenterCursor(88, 72);
            MessageBox.Show("File " + fileName + " saved.");
        }

        private void DocMgr_Load(object sender, EventArgs e)
        {
            ProjectPath = GetLastProjectPath();

            CenterToScreen();

            if (ProjectPath != null)
            {
                LoadProject(ProjectPath, out Root);
                MakeButtons(Root.SubDocs);
                LoadProjectsLastDoc(Root.DocName);

                if (Root.SubDocs == null)
                    Root.SubDocs = new List<Doc>();

                string temp;
                if (IsNullOrEmpty(BackupsAndArchivesFolder))
                {
                    string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    Properties.Settings.Default.BackupsAndArchivesFolder = documentsPath + '\\' + "BackupsAndArchives";
                    BackupsAndArchivesFolder = Properties.Settings.Default.BackupsAndArchivesFolder;
                    Properties.Settings.Default.Save();
                }

                // NEXT STEPS: Need to serialize default font.  Then add button to use font dlg to set default font.
                // (LATER) When new doc created, set font from default font.
                // Get code project font mgr running.

                // Sets font selected for rich text box:
                //var fontDlg = new FontDialog();

                //if (fontDlg.ShowDialog() == DialogResult.OK)
                //    richTextBox1.Font = fontDlg.Font;

                // Serializes doc:
                //var doc = new Doc();    // Top-level root doc just provides SubDocs array for real docs.
                //doc.DocName = "Root";
                //doc.Path = "";

                //var doc2 = new Doc();
                //doc2.DocName = "Design";
                //doc2.Path = @"C:\Users\alank\Documents\DocMgr\design.rtf";

                //var doc3 = new Doc();
                //doc3.DocName = "3rd Doc!";
                //doc3.Path = @"C:\Users\alank\Documents\DocMgr\purpose.rtf";

                //doc.SubDocs = new List<Doc>();
                //doc.SubDocs.Add(doc2);
                //doc.SubDocs.Add(doc3);
            }
        }

        private void ClickSingleButton()    // Click the only button with path in Tag member.
        {
            foreach (Control cont in Controls)
                if (cont is Button && cont.Tag != null)
                {
                    SelectDocClick(cont, new EventArgs());
                    return;
                }
        }

        private void MakeButtons(List<Doc>? subDocs)    // Make a button on the left side
        {                                               // for each of project's documents.
            //Point next = ButtonListStart;
            buttons = new List<Button>();
            RemoveOldButtons();

            if (subDocs == null)
                return;

            foreach (Doc doc in subDocs)
            {
                // Make configurable button:
                Button b = new Button();
                b.Text = doc.DocName;
                b.Font = SystemFont;
                b.Name = doc.DocName;
                b.Tag = doc.DocPath;
                b.Click += SelectDocClick;
                //b.Location = next;
                //next.Y += BUTTON_SPACING;
                b.Size = new Size(120, 35);
                b.BackColor = Color.FromArgb(255, 250, 250, 250);
                b.MouseHover += button_MouseHover;
                buttons.Add(b);
            }

            ResizeAndAddButtons(buttons);
            ArrangeControls();
            CenterWindowIfOverEdge();
        }

        private void CenterWindowIfOverEdge()
        {
            //if (WindowExtensions.IsOutOfScreenBounds(this))
            if (this.IsOutOfScreenBounds())
                CenterToScreen();
        }


        private void ArrangeControls()
        {
            DocName.Left = richTextBox.Left;

            using (Graphics g = CreateGraphics())
            {
                var size = g.MeasureString(ProjectName.Text, ProjectName.Font);

                int projWidth = (int)Math.Round(size.Width);
                ProjectName.Width = projWidth;
                ProjectName.Left = richTextBox.Right - projWidth;
                label1.Left = ProjectName.Left - label1.Width;
            }

            Button[] rightButtons = new Button[] { buttonClose, ButtonNewDoc,
                ButtonNewProj, buttonRemoveDoc, buttonOpenFolder, buttonNumberLines,
                buttonBackUpFile, buttonBackUpProject, buttonArchiveFile,
                buttonArchiveProject, buttonProperties, buttonPrint};
            foreach (Button b in rightButtons)
                b.Left = richTextBox.Right + 10;

            Width = buttonClose.Right + 25;
            int leftTopButtons = richTextBox.Left;

            Button[] topButtons = new Button[] { buttonLoadProj, buttonLoadDoc, buttonSaveDoc, ButtonSaveAs };
            foreach (Button b in topButtons)
            {
                b.Left = leftTopButtons;
                leftTopButtons += (b.Width + 32);
            }
        }

        private void ResizeAndAddButtons(List<Button> buttons)
        {
            float buttonWidth = MIN_BUTTON_WIDTH;

            if (buttons.Count() == 0)
            {
                richTextBox.Left = originalLeft;
                return;
            }

            Graphics g = buttons[0].CreateGraphics();
            Font f = buttons[0].Font;
            Point next = ButtonListStart;
            HashSet<Button> buttonColumn = new();

            // PSEUDOCODE: For each button: Set left.  Increment next.Y.  if past richTextBox.bottom,
            // increment next.X by max width so far (buttonWidth)
            //             and reset next.Y to ButtonListStart.Y, and reset buttonWidth to 120.

            foreach (Button b in buttons)
            {
                var size = g.MeasureString(b.Text, f);
                float width = size.Width;

                b.Location = next;
                next.Y += BUTTON_Y_SPACING;

                if (b.Bottom > richTextBox.Bottom)
                {                           // Won't fit; start a new button column:
                    foreach (Button b2 in buttonColumn)     // Resize and add cur column:
                    {
                        b2.Width = (int)buttonWidth;
                        Controls.Add(b2);
                    }

                    buttonColumn.Clear();
                    next.X += ((int)buttonWidth + BUTTON_COLUMN_GAP);
                    next.Y = ButtonListStart.Y;
                    buttonWidth = MIN_BUTTON_WIDTH;
                    b.Location = next;
                    next.Y += BUTTON_Y_SPACING;
                }

                if (width > buttonWidth)
                    buttonWidth = width;    // Get maximum length button name.

                buttonColumn.Add(b);
            }

            foreach (Button b2 in buttonColumn)
            {
                b2.Width = (int)buttonWidth;
                Controls.Add(b2);
            }

            //ORIG: richTextBox.Left = (int)Math.Round(buttons[0].Left + buttonWidth) + 10;
            richTextBox.Left = next.X + (int)buttonWidth + BUTTON_COLUMN_GAP;
            CenterToScreen();
            // ORIG:
            //foreach (Button b in buttons)
            //{
            //    var size = g.MeasureString(b.Text, f);
            //    float width = size.Width;

            //    if (width > buttonWidth)
            //        buttonWidth = width;    // Get maximum length button name.
            //}

            //foreach (Button b in buttons)
            //{
            //    b.Width = (int)Math.Round(buttonWidth);
            //    Controls.Add(b);
            //}

            //richTextBox.Left = (int)Math.Round(buttons[0].Left + buttonWidth) + 10;
        }

        private void RemoveOldButtons()     // Remove every document button.
        {                                   // These are the buttons with non-null Tag.
            bool changed;

            do
            {
                changed = false;

                foreach (Control cont in Controls)
                    if ((cont is Button) && (cont.Left < richTextBox.Left - 5)
                        && cont.Top > ButtonListStart.Y - 2)
                    {
                        Controls.Remove(cont as Button);
                        changed = true;
                        break;
                    }
            }
            while (changed);
        }

        private void SelectDocClick(object? sender, EventArgs e)    // Called when document button is
        {                                                           // clicked to load the document.
            SaveChanges();

            loadingDoc = true;
            richTextBox.Clear();
            Button but = sender as Button;
            ColorButtonBknd(but);

            DocName.Text = but.Name + ':';

            if (but.Tag != null)
            {
                CurrentFilePath = but.Tag.ToString();

                if (File.Exists(CurrentFilePath))
                {
                    try
                    {
                        richTextBox.LoadFile(CurrentFilePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Problem loading document: " + ex.Message);
                        loadingDoc = false;
                        return;
                    }

                    ScrollToDocPosition(but.Name);
                    buttonSaveDoc.Enabled = false;
                    buttonRemoveDoc.Enabled = ProjectPath != null;
                    SetProjectsLastDoc(CurrentFilePath);
                    HaveMargins = false;
                }
                else
                    MessageBox.Show("File '" + CurrentFilePath + "' was not found.");
            }
            else
                MessageBox.Show("Can't select document.");

            if (richTextBox.Text.Length == 0)
                richTextBox.Font = Properties.Settings.Default.DefaultFont;

            buttonRemoveDoc.Enabled = true;
            richTextBox.Focus();
            loadingDoc = false;
        }

        private void ColorButtonBknd(Button? but)
        {
            foreach (Button b in buttons)
                if (b == but)
                    b.BackColor = Color.FromArgb(255, 210, 250, 255);
                else
                    b.BackColor = Color.FromArgb(255, 250, 250, 250);
        }

        Doc? FindDocByName(string name)
        {
            foreach (var doc in Root.SubDocs)
                if (doc.DocName == name)
                    return doc;

            return null;
        }

        private void ScrollToDocPosition(string name)
        {
            Doc? doc = FindDocByName(name);



            bool iTemp = false;
            int err;
            if (doc != null)
                iTemp = SetScrollPosition(doc.ScrollPos);

            if (!iTemp)
                err = GetLastError();
        }

        private void ClickButtonWithName(string buttonName)     // Load the document with that name.
        {
            foreach (Control cont in Controls)
                if (cont is Button && cont.Name == buttonName)
                {
                    SelectDocClick(cont, new EventArgs());
                    buttonSaveDoc.Enabled = false;
                    return;
                }

        }

        private void LoadProject(string projectPath, out Doc? Root)
        {
            if (File.Exists(projectPath))
            {                               // Load project file:
                string? docList = File.ReadAllText(projectPath);
                ProjectName.Text = Path.GetFileNameWithoutExtension(projectPath);

                if (docList != null)
                    try
                    {
                        Root = System.Text.Json.JsonSerializer.Deserialize<Doc>(docList);
                        buttonLoadDoc.Enabled = true;
                        Root.DocPath = projectPath;
                        LoadProjectDlg.AddProject(ProjectName.Text, projectPath);
                        ProjName = ProjectName.Text;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Problem loading project: " + ex.Message);
                        Root = new Doc("Root");
                        return;
                    }
                else
                    Root = new Doc("Root");
            }
            else
            {                               // File not found:
                MessageBox.Show("Project file '" + projectPath + "' not found.");
                Root = new Doc("Root");
                DocName.Text = "";
            }

            buttonRemoveDoc.Enabled = false;
            CurrentFilePath = null;
        }

        private static bool IsNullOrEmpty(string? word)
        {
            return word == null || word.Length == 0;
        }

        private void SaveProject(string projectPath, Doc? root)
        {
            if (IsNullOrEmpty(projectPath))
                return;

            string stringDoc = System.Text.Json.JsonSerializer.Serialize(root);
            File.WriteAllText(projectPath, stringDoc);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            SaveScrollPosition();

            if (DocName.Text.StartsWith('*'))
                buttonSaveDoc_Click(null, null);        // Save changes.

            Close();
        }

        private void DocMgr_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == (int)Keys.Escape)
            //    buttonClose_Click(sender, e);           // User wants to exit.

            if (e.KeyChar == 19 && buttonSaveDoc.Enabled) // Ctrl-S.  Civilized way to do this not apparent.
                buttonSaveDoc_Click(sender, e);         // Save document.

            if (e.KeyChar == 16)                        // Ctrl-P.  Print.
                buttonPrint_Click(sender, e);           // Invoke print.

            if (e.KeyChar == 6)                        // Ctrl-F.  Find.
                buttonFind_Click(sender, e);           // Invoke find.
        }

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            DocMgr_KeyPress(sender, e);                 // Forward key to parent form.
        }

        private void buttonLoadProj_Click(object sender, EventArgs e)
        {
            if (DocName.Text.StartsWith('*'))
                buttonSaveDoc_Click(null, null);            // In case changes not saved.

            SaveScrollPosition();
            ProjectPath = SelectProjectFile();
            buttonSaveDoc.Enabled = false;
            loadingDoc = true;

            if (ProjectPath != null)
            {
                LoadProject(ProjectPath, out Root);
                SetProjectPath(ProjectPath);

                MakeButtons(Root.SubDocs);
                DocName.Text = "";

                if (!LoadProjectsLastDoc(Root.DocName))
                    if (Root.SubDocs.Count == 1)
                        ClickSingleButton();
            }

            loadingDoc = false;
        }

        private static void SetProjectPath(string ProjectPath)
        {
            RegistryKey? key = Registry.CurrentUser.CreateSubKey(
                BASE_REGISTRY_KEY,
                RegistryKeyPermissionCheck.ReadWriteSubTree);
            key.SetValue("LastProjectPath", ProjectPath);
            key.Close();
        }

        private string? SelectProjectFile()
        {
            string? projectPath = null;
            LoadProjectDlg dlg = new LoadProjectDlg();
            dlg.StartPosition = FormStartPosition.Manual;
            dlg.Left = buttonLoadProj.Left - 7;
            dlg.Top = buttonLoadProj.Bottom + 32;

            if (dlg.NumProjects > 0)
                Cursor.Position = new Point(dlg.Left + 200, dlg.Top + 162);
            // else put cursor in Browse button.

            DialogResult dr = dlg.ShowDialog();

            if (dr == DialogResult.OK)
                projectPath = dlg.selectedPath;

            return projectPath;
        }

        public static string? GetLastProjectPath()
        {
            return GetRegistryValue("LastProjectPath");
        }

        private void SetProjectsLastDoc(string currentFilePath)
        {
            string ProjName = Path.GetFileNameWithoutExtension(Root.DocPath);
            RegistryKey? RegKey = GetRegKeyForProject(ProjName);

            if (RegKey == null)
                return;

            RegKey.SetValue("LastDoc", CurrentFilePath);
            RegKey.Close();
        }

        private bool LoadProjectsLastDoc(string? docName)
        {
            string ProjName = Path.GetFileNameWithoutExtension(Root.DocPath);
            RegistryKey? RegKey = GetRegKeyForProject(ProjName);
            richTextBox.Text = "";

            if (RegKey == null)
                return false;

            object? LastDocObject = RegKey.GetValue("LastDoc");

            if (LastDocObject != null)
            {
                string? LastDoc = LastDocObject as string;
                LoadDoc(LastDoc, false);
            }

            RegKey.Close();
            return richTextBox.Text != "";
        }

        private RegistryKey? GetRegKeyForProject(string projName)
        {
            RegistryKey? RegKey;
            int NumProjects = GetRegistryValueInt("NumProjects");

            for (int ProjNum = 0; ProjNum < NumProjects; ++ProjNum)
            {
                {
                    string RegKeyName = BASE_REGISTRY_KEY + @"\Project"
                        + ProjNum.ToString();
                    RegKey = Registry.CurrentUser.CreateSubKey(
                        RegKeyName,
                        RegistryKeyPermissionCheck.ReadWriteSubTree);
                    object? obj = RegKey.GetValue("Name");

                    if (obj != null)
                    {
                        string ProjName = obj as string;

                        if (ProjName != null && ProjName == projName)
                            return RegKey;
                    }

                    RegKey.Close();
                }
            }

            return null;
        }

        public static string? GetRegistryValue(string itemKey)
        {
            RegistryKey? RegKey = Registry.CurrentUser.CreateSubKey(
                BASE_REGISTRY_KEY,
                RegistryKeyPermissionCheck.ReadWriteSubTree);
            object? obj = RegKey.GetValue(itemKey);
            RegKey.Close();

            return (obj == null) ? null : obj.ToString();
        }

        public static void SetRegistryValue(string itemKey, string itemValue)
        {
            RegistryKey? RegKey = Registry.CurrentUser.CreateSubKey(
                BASE_REGISTRY_KEY,
                RegistryKeyPermissionCheck.ReadWriteSubTree);

            RegKey.SetValue(itemKey, itemValue);
            RegKey.Close();
        }

        private int GetRegistryValueInt(string key)
        {
            int IntResult;
            string? StringResult = GetRegistryValue(key);

            if (!IsNullOrEmpty(StringResult))
                if (int.TryParse(StringResult, out IntResult))
                    return IntResult;                       // Good result.

            return BAD_INT;                                 // Bad result.
        }

        // General method to select a file.
        // Used to select project & document files.
        public static string? OpenFile(string filter, bool overwriteOk = false)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                string? path = GetDefaultPath();

                openFileDialog.Title = "Select File";
                openFileDialog.InitialDirectory = path;
                openFileDialog.Filter = filter;
                openFileDialog.CheckFileExists = !overwriteOk;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                    return openFileDialog.FileName;

                return null;
            }
        }

        // General method to save a file.
        // Used to for Save As.
        public static string? GetSaveFileName(string filter, bool overwriteOk = false)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                string? path = GetDefaultPath();

                saveFileDialog.Title = "Select File";
                saveFileDialog.InitialDirectory = path;
                saveFileDialog.Filter = filter;
                saveFileDialog.CheckFileExists = !overwriteOk;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    return saveFileDialog.FileName;

                return null;
            }
        }

        static string GetDefaultPath()
        {
            string? path = Path.GetDirectoryName(ProjectPath); // Assume same as project.

            if (IsNullOrEmpty(path))
                path = GetLastProjectPath();        // From Registry.

            if (IsNullOrEmpty(path))
                path = "c:\\";                      // Default path.

            return path;
        }

        public static void CenterCursor(int xOffset = 0, int yOffset = 0)
        {
            Cursor.Position = new Point(Screen.PrimaryScreen.Bounds.Width / 2 + xOffset,
                Screen.PrimaryScreen.Bounds.Height / 2 + yOffset);
        }

        private void buttonLoadDoc_Click(object sender, EventArgs e)        // Add & load an existing .rtf
        {                                                                   // file into the current project.
            if (DocName.Text.StartsWith('*'))
                buttonSaveDoc_Click(null, null);                            // In case changes not saved.

            string? DocPath = OpenFile("rtf files (*.rtf)|*.rtf|All files (*.*)|*.*");

            if (IsNullOrEmpty(DocPath))
                return;

            LoadDoc(DocPath);
            //            SetRegistryValue("LastDoc", DocPath);
        }

        private void LoadDoc(string? docPath, bool addIfMissing = true)
        {
            loadingDoc = true;

            if (!IsNullOrEmpty(docPath))
            {
                string docName = Path.GetFileNameWithoutExtension(docPath);
                ProjName = Path.GetFileNameWithoutExtension(Root.DocPath);

                if (!DocAlreadyInProject(docName) && addIfMissing)
                {
                    Root.AddDoc(docPath);
                    WriteUpdatedPath();
                    MakeButtons(Root.SubDocs);
                }

                ClickButtonWithName(docName);
            }

            loadingDoc = false;
        }

        private bool DocAlreadyInProject(string docName)
        {
            return Root.SubDocs.Where(x => x.DocName == docName).Any();
        }

        private void WriteUpdatedPath()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = System.Text.Json.JsonSerializer.Serialize(Root, options);
            string? path = GetLastProjectPath();

            if (!IsNullOrEmpty(path))
                File.WriteAllText(path, jsonString);
        }

        private void buttonSaveDoc_Click(object sender, EventArgs e)
        {
            bool saveOk = false;

            //if (CurrentFilePath != null)
            //{
            //    // DOESN'T WORK FOR EMPTY DOC:
            try
            {
                if (richTextBox.Text.Trim().Length == 0 && File.Exists(CurrentFilePath)
                    && !RichTextBoxContainsImage(richTextBox))
                    if (MessageBox.Show("Warning: You are about to overwrite a file with an empty string."
                        + "  Click OK to continue or Cancel to cancel.", "Overwrite Warning", MessageBoxButtons.OKCancel)
                        != DialogResult.OK)
                        return;

                SaveScrollPosition();
                //ORIG: richTextBox.SaveFile(CurrentFilePath);

                string rtfWithMargins;

                if (File.Exists(CurrentFilePath))
                    UpdateMargins();

                if (MyMargins.IsNull())
                    rtfWithMargins = richTextBox.Rtf;   // Margins not used; just use existing text.
                else
                    rtfWithMargins = RtfMarginHelper.AddMarginsToRtf(richTextBox.Rtf, MyMargins);

                if (DocName.Text.Length == 0)
                {
                    string? fileName = GetSaveFileName("RTF Files|*.rtf|All Files|*.*", true);

                    if (fileName == null)
                        return;

                    CurrentFilePath = fileName;
                    DocName.Text = Path.GetFileNameWithoutExtension(CurrentFilePath);
                }

                File.WriteAllText(CurrentFilePath, rtfWithMargins);
                saveOk = true;
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("File " + CurrentFilePath + " is read-only or otherwise unauthorized.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem saving file " + CurrentFilePath + ": " + ex.Message);
            }
            // DFW: File.WriteAllText(CurrentFilePath, richTextBox.Rtf);
            //Root = new Doc("Root");
            //string text = System.Text.Json.JsonSerializer.Serialize<Doc>(Root);
            //File.WriteAllText(CurrentFilePath, text);

            if (saveOk && DocName.Text[0] == '*')
                DocName.Text = DocName.Text.Remove(0, 2);
            //}

            buttonSaveDoc.Enabled = !saveOk;
            richTextBox.Focus();
        }

        bool RichTextBoxContainsImage(RichTextBox richTextBox)
        {
            string rtf = richTextBox.Rtf;
            return rtf.Contains(@"\pict");
        }

        private void buttonRemoveDoc_Click(object sender, EventArgs e)      // Just removes from project.
        {                                                                   // .rtf file is untouched.
            if (IsNullOrEmpty(DocName.Text) || IsNullOrEmpty(ProjectPath))
                return;

            lastDocName = DocName.Text;
            richTextBox.Clear();
            CurrentFilePath = null;

            if (lastDocName[0] == '*')
                lastDocName = lastDocName.Remove(0, 2);

            if (lastDocName.EndsWith(':'))
                lastDocName = lastDocName.Remove(lastDocName.Length - 1, 1);

            foreach (Doc doc in Root.SubDocs)
                if (doc.DocName == lastDocName)
                {
                    //richTextBox.Clear();
                    buttonRemoveDoc.Enabled = false;
                    Root.SubDocs.Remove(doc);
                    MakeButtons(Root.SubDocs);
                    string text = System.Text.Json.JsonSerializer.Serialize<Doc>(Root);
                    File.WriteAllText(ProjectPath, text);
                    DocName.Text = "";
                    buttonSaveDoc.Enabled = false;
                    return;
                }

            lastDocName = null;
            MessageBox.Show("Path " + CurrentFilePath + " not found in documents.");
        }

        private void richTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!loadingDoc)
            {
                buttonSaveDoc.Enabled = true;
                // Indicate document modified:
                if (DocName.Text.Length > 0 && DocName.Text[0] != '*')
                    DocName.Text = DocName.Text.Insert(0, "* ");
            }
        }

        private void ButtonNewDoc_Click(object sender, EventArgs e)
        {
            SaveScrollPosition();

            FormCreateDoc fcd = new FormCreateDoc(GetDefaultPath());

            if (fcd.ShowDialog() != DialogResult.OK)
                return;

            bool success = fcd.DocName.Length > 0;

            if (success == true && Root != null)
            {
                SaveChanges();
                CurrentFilePath = fcd.FilePath;
                Root.AddDoc(CurrentFilePath, fcd.DocName);
                MakeButtons(Root.SubDocs);
                buttonSaveDoc.Enabled = true;
                SaveProject(ProjectPath, Root);
                DocName.Text = Path.GetFileNameWithoutExtension(CurrentFilePath);
                SetProjectsLastDoc(CurrentFilePath);
                HighlightThisButton("&" + fcd.textBoxDocName.Text);

                loadingDoc = true;
                richTextBox.Clear();
                loadingDoc = false;
                richTextBox.Font = font;
                richTextBox.Focus();
            }
        }

        private void SaveChanges()
        {
            if (DocName.Text.Length > 0 && DocName.Text[0] == '*')
                buttonSaveDoc_Click(null, null);                    // Save changes.

            if (DocName.Text.Length > 0)
                SaveScrollPosition();
        }

        private void HighlightThisButton(string text)
        {
            foreach (Control con in Controls)
                if ((con is Button) && (con.Text == text))
                {
                    ColorButtonBknd(con as Button);
                    return;
                }
        }

        private void buttonNumberLines_Click(object sender, EventArgs e)
        {
            string text = richTextBox.SelectedText.Trim();            // Get selected lines to number.

            if (text.Length == 0)
            {
                MessageBox.Show("No text lines are selected.");
                return;
            }

            //int numLines = CountChar(text, '\n') + 1;                 // Count # of lines: /n count plus 1.
            string rtfText = ReplaceNewLinesWithParagraphEnd(text);   // Convert selected lines to RTF format.
            int pos = richTextBox.Rtf.IndexOf(rtfText);               // Find position of lines in RTF text.

            if (pos == -1)
            {
                MessageBox.Show("List not found.");
                return;
            }

            //MessageBox.Show("Index = " + pos + ".  # lines = " + numLines);

            int lineNum = 0;
            StringBuilder numberedList = new StringBuilder(
                @"{\pntext\f0 1.\tab}{\*\pn\pnlvlbody\pnf0\pnindent0\pnstart1\pndec{\pntxta.}}" + "\n");

            foreach (string line in GetLines(text))
                if (++lineNum == 1)
                    numberedList.Append($@"\fi-360\li720\sl276\slmult1\f0\lang9 {line}\par");
                else
                    numberedList.Append(@"{\pntext\f0 " + lineNum.ToString() + @".\tab}" +
                        line + @"\par");

            numberedList.Append(@"\pard");

            string replacementText = numberedList.ToString();
            //File.WriteAllText("out.txt", replacementText);
            richTextBox.Rtf = richTextBox.Rtf.Replace(rtfText, replacementText);
        }

        private IEnumerable<string> GetLines(string text)
        {
            int pos = 0, len = text.Length, endPos;
            string nextLine;

            while (pos < len)
            {
                endPos = text.IndexOf("\n", pos);

                if (endPos == -1)
                {
                    nextLine = text.Substring(pos);
                    pos = len;
                }
                else
                {
                    nextLine = text.Substring(pos, endPos - pos);
                    pos = endPos + 1;
                }

                yield return nextLine;
            }
        }

        private string ReplaceNewLinesWithParagraphEnd(string v)
        {
            StringBuilder sb = new StringBuilder(v);

            sb.Replace("\n", "\\par\r\n");  // This is how RTF ends paragraphs:
            sb.Append("\\par\r\n");
            return sb.ToString();
        }

        private void DocMgr_Resize(object sender, EventArgs e)
        {
            Size size = richTextBox.Size;
            int FormHeight = Height;
            size.Height = FormHeight - 150;
            richTextBox.Size = size;
        }

        private void button_MouseHover(object sender, EventArgs e)
        {
            foreach (Doc doc in Root.SubDocs)
                if ((sender as Button).Text == doc.DocName)
                    toolTips.Show(doc.DocPath, sender as Button);
        }

        private void ProjectName_MouseHover(object sender, EventArgs e)
        {
            toolTips.Show(Root.DocPath, sender as Label);
        }

        private void DocName_MouseHover(object sender, EventArgs e)
        {
            foreach (Doc doc in Root.SubDocs)
                if ((sender as Label).Text == doc.DocName + ":")
                    toolTips.Show(doc.DocPath, sender as Label);
        }

        private void buttonOpenFolder_Click(object sender, EventArgs e)
        {
            string ProjectFolder = Path.GetDirectoryName(CurrentFilePath);

            if (ProjectFolder == null)
                ProjectFolder = Path.GetDirectoryName(ProjectPath);

            Process.Start("explorer", ProjectFolder);
        }


        #region BackupsAndArchives
        private void buttonBackUpFile_Click(object sender, EventArgs e)
        {
            if ((BackupFolder = GetMakeBackupFolder(ProjName)) == "")
                return;                         // Problems.

            HandleOneFile(BackupFolder);
        }

        private void buttonArchiveFile_Click(object sender, EventArgs e)
        {
            if ((ArchiveFolder = GetMakeArchiveFolder(ProjName)) == "")
                return;                         // Problems.

            HandleOneFile(ArchiveFolder);
        }

        private void HandleOneFile(string destFolder)
        {
            if (Root.SubDocs.Count == 0)
                return;                         // No docs.

            Cursor.Current = Cursors.WaitCursor;

            if (DocName.Text[0] == '*')
                buttonSaveDoc_Click(null, null);

            if (CopySingleDocToFolder(Root.SubDocs, destFolder, DocName.Text))
            {
                Cursor.Current = Cursors.Default;
                CenterCursor(162, 88);
                MessageBox.Show($"{DocName.Text.Substring(0, DocName.Text.Length - 1)} is saved to "
                    + destFolder + ".");
                return;
            }

            Cursor.Current = Cursors.Default;
            MessageBox.Show($"Warning: Document {DocName.Text} was not found and not saved.");
        }

        private void buttonBackUpProject_Click(object sender, EventArgs e)
        {

            if ((BackupFolder = GetMakeBackupFolder(ProjName)) == "")
                return;                         // Problems.

            HandleProject(BackupFolder);
        }

        private void buttonArchiveProject_Click(object sender, EventArgs e)
        {
            if ((ArchiveFolder = GetMakeArchiveFolder(ProjName)) == "")
                return;                         // Problems.

            HandleProject(ArchiveFolder);
            SetReadOnlyAttributes(ArchiveFolder);
        }

        private void HandleProject(string destFolder)
        {
            if (Root.SubDocs.Count == 0)
                return;                         // No docs.

            Cursor.Current = Cursors.WaitCursor;

            if (DocName.Text[0] == '*')
                buttonSaveDoc_Click(null, null);

            SaveProject(destFolder + '\\' + ProjName + ".json", Root);

            if (CopyDocsToFolder(Root.SubDocs, destFolder))
            {
                Cursor.Current = Cursors.Default;
                CenterCursor(162, 88);
                MessageBox.Show($"Project {ProjName} is saved to "
                    + destFolder + ".");
                return;
            }

            MessageBox.Show($"Warning: Document {DocName.Text} was not found and not saved.");
        }

        private bool CopySingleDocToFolder(List<Doc> subDocs, string destFolder, string docToCopy)
        {
            try
            {
                foreach (Doc doc in subDocs)
                    if (doc.DocName + ':' == docToCopy)             // Match name in GUI.
                        return CopyFileToFolder(doc, destFolder);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return false;
        }

        private bool CopyDocsToFolder(List<Doc> subDocs, string destFolder)
        {
            foreach (Doc doc in subDocs)
                try
                {
                    CopyFileToFolder(doc, destFolder);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }

            return true;
        }

        private bool CopyFileToFolder(Doc doc, string destFolder)
        {
            if (destFolder.ToLower().Contains("www."))
            {
                WebUtils.SendFileToUrl(doc.DocPath, destFolder);
                return true;
            }

            if (!Directory.Exists(destFolder) || !File.Exists(doc.DocPath))
                return false;

            string destinationFile = Path.Combine(destFolder, Path.GetFileName(doc.DocPath));
            File.Copy(doc.DocPath, destinationFile, true);
            return true;
        }

        private string GetMakeBackupFolder(string projName)
        {
            if (string.IsNullOrEmpty(Root.DocPath))
            {
                MessageBox.Show("Project name is empty.");
                return "";                              // Project name blank.
            }

            BackupFolder = BackupsAndArchivesFolder + '\\' + "Backups" + '\\' + projName;

            return GetMakeFolder(BackupFolder);
        }

        private string GetMakeArchiveFolder(string projName)
        {
            if (string.IsNullOrEmpty(Root.DocPath))
            {
                MessageBox.Show("Project name is empty.");
                return "";                              // Project name blank.
            }

            ArchiveFolder = BackupsAndArchivesFolder + '\\' + "Archives"
                + '\\' + projName + '\\' + DateTime.Now.ToString("s").Replace(':', '.');

            return GetMakeFolder(ArchiveFolder);
        }

        private void buttonProperties_Click(object sender, EventArgs e)
        {
            PropertiesForm prop = new();
            MySettings mySettings = new();

            // Copy to MySettings to edit settings according to type:

            mySettings.DefaultFont = Properties.Settings.Default.DefaultFont;
            mySettings.BackupsAndArchivesFolder = Properties.Settings.Default.BackupsAndArchivesFolder;
            UpdateMargins();

            mySettings.MarginLeft = TwipsToStrInches(MyMargins.Left);
            mySettings.MarginRight = TwipsToStrInches(MyMargins.Right);
            mySettings.MarginTop = TwipsToStrInches(MyMargins.Top);
            mySettings.MarginBottom = TwipsToStrInches(MyMargins.Bottom);

            prop.SetProperties(mySettings);
            prop.ShowDialog();

            if (prop.SaveSettings)
            {               // Copy settings back to default for persistence:
                Properties.Settings.Default.DefaultFont = mySettings.DefaultFont;
                Properties.Settings.Default.BackupsAndArchivesFolder = mySettings.BackupsAndArchivesFolder;
                bool marginsChanged = CopyMarginsAndDetectChange(mySettings);
                Properties.Settings.Default.Save();

                // Update current settings:
                BackupsAndArchivesFolder = Properties.Settings.Default.BackupsAndArchivesFolder;
                font = Properties.Settings.Default.DefaultFont;
                richTextBox.Font = font;

                if (marginsChanged)
                {
                    //RtfMarginHelper.SetMarginsInDoc(CurrentFilePath, MyMargins);
                    buttonSaveDoc_Click(null, null);
                }
            }

            bool CopyMarginsAndDetectChange(MySettings mySettings)
            {
                bool changed = false;

                changed = changed || MyMargins.Left != StrInchesToTwips(mySettings.MarginLeft);
                changed = changed || MyMargins.Right != StrInchesToTwips(mySettings.MarginRight);
                changed = changed || MyMargins.Top != StrInchesToTwips(mySettings.MarginTop);
                changed = changed || MyMargins.Bottom != StrInchesToTwips(mySettings.MarginBottom);

                MyMargins.Left = Properties.Settings.Default.MarginLeft = StrInchesToTwips(mySettings.MarginLeft);
                MyMargins.Right = Properties.Settings.Default.MarginRight = StrInchesToTwips(mySettings.MarginRight);
                MyMargins.Top = Properties.Settings.Default.MarginTop = StrInchesToTwips(mySettings.MarginTop);
                MyMargins.Bottom = Properties.Settings.Default.MarginBottom = StrInchesToTwips(mySettings.MarginBottom);

                return changed;
            }
        }

        private void UpdateMargins()
        {
            if (HaveMargins == false)
                if (CurrentFilePath == null)
                    MyMargins = MarginSt.NullMargin;
                else
                    MyMargins = RtfMarginHelper.GetMargins(CurrentFilePath);

            HaveMargins = true;
        }

        private float StrInchesToTwips(string floatOrMessage)
        {
            if (float.TryParse(floatOrMessage, out float value) && value != -1)
                return value * 1440;
            else
                return -1;      // "not used" or invalid float.
        }

        private string TwipsToStrInches(float num)
        {
            if (num == -1)
                return "not used";

            if (num == 0)
                return "0";

            return TrimSuffix((num / 1440).ToString("F3"), ".000").TrimEnd('0');
        }

        public static string TrimSuffix(string input, string suffix)
        {
            if (input != null && suffix != null && input.EndsWith(suffix))
            {
                return input.Substring(0, input.Length - suffix.Length);
            }
            return input;
        }
        private string GetMakeFolder(string folderName)
        {
            try
            {
                if (!Directory.Exists(folderName))
                    Directory.CreateDirectory(folderName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return "";
            }

            return folderName;
        }

        private void SetReadOnlyAttributes(string destFolder)
        {
            DirectoryInfo dirInfo = new(destFolder);

            if (dirInfo.Exists)
            {
                dirInfo.Attributes |= FileAttributes.ReadOnly;

                foreach (var file in dirInfo.GetFiles())
                {
                    file.Attributes |= FileAttributes.ReadOnly;
                }

                foreach (var subDir in dirInfo.GetDirectories())
                {
                    SetReadOnlyAttributes(subDir.FullName);
                }
            }
        }

        #endregion

        //private int CountChar(string text, char v)
        //{
        //    int count = 0;

        //    foreach (char c in text)
        //        if (c == v)
        //            ++count;

        //    return count;
        //}


        /***
        private void WriteTextToRichTextBox()
        {
           // Clear all text from the RichTextBox;
           //richTextBox.Clear();
           // Set the font for the opening text to a larger Arial font;
           richTextBox.SelectionFont = new Font("Arial", 16);
           // Assign the introduction text to the RichTextBox control.
           richTextBox.SelectedText = "The following is a list of bulleted items:" + "\n";
           // Set the Font for the first item to a smaller size Arial font.
           richTextBox.SelectionFont = new Font("Arial", 12);
           // Specify that the following items are to be added to a bulleted list.
           richTextBox.SelectionBullet = true;
           // Set the color of the item text.
           richTextBox.SelectionColor = Color.Red;
           // Assign the text to the bulleted item.
           richTextBox.SelectedText = "Apples" + "\n";
           // Apply same font since font settings do not carry to next line.
           richTextBox.SelectionFont = new Font("Arial", 12);
           richTextBox.SelectionColor = Color.Orange;
           richTextBox.SelectedText = "Oranges" + "\n";
           richTextBox.SelectionFont = new Font("Arial", 12);
           richTextBox.SelectionColor = Color.Purple;
           richTextBox.SelectedText = "Grapes" + "\n";
           // End the bulleted list.
           richTextBox.SelectionBullet = false;
           // Specify the font size and string for text displayed below bulleted list.
           richTextBox.SelectionFont = new Font("Arial", 16);
           richTextBox.SelectedText = "Bulleted Text Complete!";
        } ***/

        private void ButtonNewProj_Click(object sender, EventArgs e)
        {
            SaveScrollPosition();
            string parentFolder = GetParentFolderOfFilesFolder(Root.DocPath);

            using (CreateFolderDialog createDialog = new(parentFolder))
            {
                createDialog.Text = "Enter New Project Name";

                if (createDialog.ShowDialog() == DialogResult.OK)
                {
                    string newFolderPath = createDialog.NewFolderPath;
                    string projFilePath = newFolderPath + '\\' + createDialog.textProjName.Text + ".json";
                    Root = new Doc("Root");
                    SaveProject(projFilePath, Root);
                    SetProjectPath(projFilePath);
                    richTextBox.Clear();
                    DocName.Text = "";
                    Root.DocPath = newFolderPath;
                    LoadProject(projFilePath, out Root);
                    MakeButtons(Root.SubDocs);
                    DocName.Text = "";
                }
            }
        }

        private string GetParentFolderOfFilesFolder(string? docPath)
        {
            string projFolder;

            try
            {
                if (!IsNullOrEmpty(docPath))
                {
                    int lastSlashPos = docPath.LastIndexOf('\\');
                    int secondLastSlashPos = docPath.LastIndexOf("\\", lastSlashPos - 1);
                    projFolder = docPath.Substring(0, secondLastSlashPos);

                    if (projFolder.Length > 0)
                        return projFolder;
                }
            }
            catch { }

            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            return documentsPath + "\\Projects";
        }

        private PrintDocument printDocument;
        private PrintRange Printer_PrintRange;
        private int Printer_FromPage, Printer_ToPage, Printer_CurrentPage;
        private int Printer_CurrentPosition;

        private void buttonPrint_Click(object sender, EventArgs e)
        {
            UpdateMargins();
            printDocument = new PrintDocument();    // Initialize PrintDocument

            if (MyMargins.Left != -1)               // MyMargins in twips, PrintDoc in 1/100ths:
                printDocument.DefaultPageSettings.Margins.Left = (int)Math.Round(MyMargins.Left / 14.4);

            if (MyMargins.Right != -1)
                printDocument.DefaultPageSettings.Margins.Right = (int)Math.Round(MyMargins.Right / 14.4);

            if (MyMargins.Top != -1)
                printDocument.DefaultPageSettings.Margins.Top = (int)Math.Round(MyMargins.Top / 14.4);

            if (MyMargins.Bottom != -1)
                printDocument.DefaultPageSettings.Margins.Bottom = (int)Math.Round(MyMargins.Bottom / 14.4);

            printDocument.PrintPage += PrintDocument_PrintPage;

            PrintDialog printDialog = new PrintDialog { Document = printDocument };  // Connect dialog & doc.
            printDialog.AllowSomePages = true;                              // Enables Page Range option.
            printDialog.AllowSelection = richTextBox.SelectionLength > 0;   // Enables Selection option.
            printDialog.PrinterSettings.FromPage = printDialog.PrinterSettings.ToPage = 1;  // Nice default.
            CenterCursor();

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                Printer_FromPage = printDialog.PrinterSettings.FromPage;
                Printer_ToPage = printDialog.PrinterSettings.ToPage;
                Printer_CurrentPage = 1;
                Printer_PrintRange = printDialog.PrinterSettings.PrintRange;
                Printer_CurrentPosition = 0;

                printDocument.Print();
            }
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            int startPrintPos = 0;
            int endPrintPos = richTextBox.TextLength;

            if (Printer_PrintRange == PrintRange.Selection)
            {
                if (richTextBox.SelectionLength == 0)
                {
                    e.HasMorePages = false;
                    return;
                }

                startPrintPos = richTextBox.SelectionStart;
                endPrintPos = startPrintPos + richTextBox.SelectionLength;
                Printer_CurrentPosition = startPrintPos;
            }

            if (Printer_CurrentPage == 0)
                Printer_CurrentPosition = startPrintPos;

            // First, calculate the logical page we are about to print
            //int logicalPage = Printer_CurrentPage;// + 1; // Pages are 1-based

            // Check if we should print this page
            if (Printer_PrintRange == PrintRange.SomePages)
            {
                if (Printer_CurrentPage < Printer_FromPage)
                {
                    // Skip this page without printing anything
                    Printer_CurrentPosition = SkipOnePage(Printer_CurrentPosition, endPrintPos, e);
                    Printer_CurrentPage++;
                    e.HasMorePages = true;
                    return;
                }
                else if (Printer_CurrentPage > Printer_ToPage)
                {
                    e.HasMorePages = false;
                    FormatRangeDone(richTextBox);
                    return;
                }
            }

            // Actually print this page
            Printer_CurrentPosition = FormatRange(Printer_CurrentPosition, endPrintPos, e, richTextBox);

            // Are there more pages?
            e.HasMorePages = (Printer_CurrentPosition < endPrintPos);

            Printer_CurrentPage++;

            if (!e.HasMorePages)
            {
                FormatRangeDone(richTextBox);
            }
        }

        // Helper to simulate skipping one page without outputting anything
        private int SkipOnePage(int start, int end, PrintPageEventArgs e)
        {
            // Create a dummy graphics object to simulate formatting without printing
            using (var dummyBitmap = new Bitmap(1, 1))
            using (var g = Graphics.FromImage(dummyBitmap))
            {
                PrintPageEventArgs fakeArgs = new PrintPageEventArgs(g, e.MarginBounds, e.PageBounds, e.PageSettings);
                return FormatRange(start, end, fakeArgs, richTextBox);
            }
        }

        // FormatRange: Uses Win32 message to render RTF content to a device context
        private int FormatRange(int charFrom, int charTo, PrintPageEventArgs e, RichTextBox rtb)
        {
            IntPtr hdc = e.Graphics.GetHdc();

            FORMATRANGE fmtRange;
            fmtRange.chrg.cpMin = charFrom;
            fmtRange.chrg.cpMax = charTo;

            fmtRange.hdc = hdc;
            fmtRange.hdcTarget = hdc;

            RectangleF rect = e.MarginBounds;
            fmtRange.rc = new RECT
            {
                Top = (int)Math.Round(rect.Top * 14.4f),
                Bottom = (int)Math.Round(rect.Bottom * 14.4f),
                Left = (int)Math.Round(rect.Left * 14.4f),
                Right = (int)Math.Round(rect.Right * 14.4f)
            };
            fmtRange.rcPage = fmtRange.rc;

            IntPtr lParam = Marshal.AllocCoTaskMem(Marshal.SizeOf(fmtRange));
            Marshal.StructureToPtr(fmtRange, lParam, false);

            SendMessage(rtb.Handle, EM_FORMATRANGE, (IntPtr)1, lParam);
            int textPrinted = (int)SendMessage(rtb.Handle, EM_FORMATRANGE, (IntPtr)1, lParam);

            Marshal.FreeCoTaskMem(lParam);
            e.Graphics.ReleaseHdc(hdc);

            return textPrinted;
        }

        private void FormatRangeDone(RichTextBox rtb)
        {
            SendMessage(rtb.Handle, EM_FORMATRANGE, (IntPtr)0, IntPtr.Zero);
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private const int WM_USER = 0x0400;
        private const int EM_FORMATRANGE = WM_USER + 57;

        private void buttonFind_Click(object sender, EventArgs e)
        {
            string newRtfText = /*RtfHighlighter.*/HighlightSearchStringInRtf(richTextBox.Rtf, "ansi");
            richTextBox.Rtf = newRtfText;
        }


        //public class RtfHighlighter
        //{
        //public static string HighlightSearchStringInRtf(string rtf, string searchString, bool caseSensitive = false, bool wholeWord = false)
        //{
        //    if (string.IsNullOrEmpty(rtf) || string.IsNullOrEmpty(searchString))
        //        return rtf;

        //    // Load RTF into RichTextBox
        //    RichTextBox rtb = new RichTextBox();
        //    rtb.Rtf = rtf;
        //    string plainText = rtb.Text;

        //    // Build regex pattern
        //    string pattern = Regex.Escape(searchString);
        //    if (wholeWord)
        //        pattern = $@"\b{pattern}\b";

        //    RegexOptions options = caseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase;
        //    Regex regex = new Regex(pattern, options);

        //    // Find matches
        //    MatchCollection matches = regex.Matches(plainText);
        //    if (matches.Count == 0)
        //        return rtf;

        //    // Apply yellow background highlight to matches
        //    foreach (Match match in matches)
        //    {
        //        rtb.Select(match.Index, match.Length);
        //        rtb.SelectionBackColor = Color.Yellow;
        //        RichTextBoxScroller.ScrollToOffsetCenter(richTextBox, match.Index);
        //    }

        //    return rtb.Rtf;
        //}
        //public static string HighlightSearchStringInRtf(string rtf, string searchText)
        //{
        //    if (string.IsNullOrEmpty(rtf) || string.IsNullOrEmpty(searchText))
        //        return rtf;

        //    // Ensure searchText is RTF-escaped
        //    string escapedSearch = Regex.Escape(searchText);

        //    // Find or insert the color table
        //    var colorTableRegex = new Regex(@"(\\colortbl[^;]*;)", RegexOptions.IgnoreCase);
        //    Match match = colorTableRegex.Match(rtf);
        //    int yellowIndex;

        //    if (match.Success)
        //    {
        //        // Append yellow if not already present
        //        if (!match.Value.Contains(@"\red255\green255\blue0"))
        //        {
        //            string newColorTable = match.Value.Insert(match.Value.Length - 1, @"\red255\green255\blue0;");
        //            rtf = rtf.Remove(match.Index, match.Length).Insert(match.Index, newColorTable);
        //            yellowIndex = CountColorEntries(newColorTable);
        //        }
        //        else
        //        {
        //            yellowIndex = GetColorIndex(match.Value, @"\red255\green255\blue0");
        //        }
        //    }
        //    else
        //    {
        //        // No color table found; insert after \rtf1... header
        //        var headerMatch = Regex.Match(rtf, @"{\\rtf1[^\n]*");
        //        if (!headerMatch.Success) return rtf; // invalid RTF

        //        yellowIndex = 1;
        //        string colorTable = @"{\colortbl;\red255\green255\blue0;}";
        //        int insertPos = headerMatch.Index + headerMatch.Length;
        //        rtf = rtf.Insert(insertPos, colorTable);
        //    }

        //    // Insert highlight tags around the search string
        //    string highlightStart = $@"\highlight{yellowIndex} ";
        //    string highlightEnd = @"\highlight0 ";

        //    string rtfEscapedSearch = Regex.Escape(searchText);
        //    var contentRegex = new Regex(rtfEscapedSearch, RegexOptions.IgnoreCase);
        //    rtf = contentRegex.Replace(rtf, $"{highlightStart}{searchText}{highlightEnd}", 1); // first occurrence only

        //    return rtf;
        //}

        //private static int CountColorEntries(string colorTable)
        //{
        //    return new Regex(@"\\red\d+\\green\d+\\blue\d+;").Matches(colorTable).Count;
        //}

        //private static int GetColorIndex(string colorTable, string color)
        //{
        //    var matches = new Regex(@"\\red\d+\\green\d+\\blue\d+;").Matches(colorTable);
        //    for (int i = 0; i < matches.Count; i++)
        //    {
        //        if (matches[i].Value.Contains(color)) return i + 1; // RTF color index starts at 1
        //    }
        //    return 1;
        //}
        //}

        public static class RichTextBoxScroller
        {
            [DllImport("user32.dll")]
            private static extern int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

            private const int EM_GETFIRSTVISIBLELINE = 0x00CE;
            private const int EM_LINESCROLL = 0x00B6;

            public static void ScrollToOffsetCenter(RichTextBox rtb, int offset)
            {
                if (offset < 0 || offset > rtb.TextLength)
                    return;

                int lineIndex = rtb.GetLineFromCharIndex(offset);

                // Get the number of visible lines in the RichTextBox
                int charIndexTopLeft = rtb.GetCharIndexFromPosition(new System.Drawing.Point(1, 1));
                int firstVisibleLine = rtb.GetLineFromCharIndex(charIndexTopLeft);
                int charIndexBottomLeft = rtb.GetCharIndexFromPosition(new System.Drawing.Point(1, rtb.ClientSize.Height - 1));
                int lastVisibleLine = rtb.GetLineFromCharIndex(charIndexBottomLeft);

                int visibleLines = lastVisibleLine - firstVisibleLine;
                int targetTopLine = Math.Max(0, lineIndex - visibleLines / 2);

                int currentTopLine = SendMessage(rtb.Handle, EM_GETFIRSTVISIBLELINE, IntPtr.Zero, IntPtr.Zero);
                int delta = targetTopLine - currentTopLine;

                SendMessage(rtb.Handle, EM_LINESCROLL, IntPtr.Zero, (IntPtr)delta);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct FORMATRANGE
        {
            public IntPtr hdc;
            public IntPtr hdcTarget;
            public RECT rc;
            public RECT rcPage;
            public CHARRANGE chrg;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left, Top, Right, Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct CHARRANGE
        {
            public int cpMin;
            public int cpMax;
        }

        //  OLD PRINTING CODE.  MINIMAL PRINTING CAPABILITY.
        //private string documentText;
        //private int currentPageIndex;
        //private PrintDocument printDocument;    
        //    private void buttonPrint_Click(object sender, EventArgs e)
        //    {
        //        PrintDialog printDialog = new PrintDialog();
        //        printDialog.Document = printDocument;

        //        // Initialize PrintDocument
        //        printDocument = new PrintDocument();
        //        printDocument.PrintPage += PrintDocument_PrintPage;

        //        if (printDialog.ShowDialog() == DialogResult.OK)
        //        {
        //            documentText = richTextBox.Text; // Store text
        //            currentPageIndex = 0;
        //            printDocument.Print();
        //        }
        //    }


        //    private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        //    {
        //        Font printFont = richTextBox.Font;
        //        float lineHeight = printFont.GetHeight(e.Graphics);
        //        float yPosition = e.MarginBounds.Top;
        //        int charactersOnPage = 0, linesPerPage = 0;

        //        // Measure how much text fits on the page
        //        e.Graphics.MeasureString(documentText.Substring(currentPageIndex), printFont,
        //                                 e.MarginBounds.Size, StringFormat.GenericTypographic,
        //                                 out charactersOnPage, out linesPerPage);

        //        // Print the text that fits
        //        e.Graphics.DrawString(documentText.Substring(currentPageIndex, charactersOnPage),
        //                              printFont, Brushes.Black, e.MarginBounds);

        //        // Move index forward
        //        currentPageIndex += charactersOnPage;

        //        // More pages to print?
        //        e.HasMorePages = (currentPageIndex < documentText.Length);
        //    }


        //    //public static void CenterCursorInButton(this Button but)
        //    //{
        //    //    Cursor.Position = new Point(but.Left + but.Width / 2, but.Top + but.Height / 2);
        //    //}
        //}
        public static string HighlightSearchStringInRtf(string rtf, string searchString, bool caseSensitive = false, bool wholeWord = false)
        {
            if (string.IsNullOrEmpty(rtf) || string.IsNullOrEmpty(searchString))
                return rtf;

            // Load RTF into RichTextBox
            RichTextBox rtb = new RichTextBox();
            rtb.Rtf = rtf;
            string plainText = rtb.Text;

            // Build regex pattern
            string pattern = Regex.Escape(searchString);
            if (wholeWord)
                pattern = $@"\b{pattern}\b";

            RegexOptions options = caseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase;
            Regex regex = new Regex(pattern, options);

            // Find matches
            MatchCollection matches = regex.Matches(plainText);
            if (matches.Count == 0)
                return rtf;

            // Apply yellow background highlight to matches
            foreach (Match match in matches)
            {
                rtb.Select(match.Index, match.Length);
                rtb.SelectionBackColor = Color.Yellow;
 // COMMENTED OUT TO COMPILE:               RichTextBoxScroller.ScrollToOffsetCenter(richTextBox, match.Index);
            }

            return rtb.Rtf;
        }

        private void DocMgr_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Home)
            {
                // Ctrl + Home is pressed
                ScrollToBeginning(richTextBox);
                // MessageBox.Show("Ctrl + Home was pressed.");
                e.Handled = true; // Optional: Prevent further processing
            }

            if (e.Control && e.KeyCode == Keys.End)
            {
                // Ctrl + End is pressed
                ScrollToEnd(richTextBox);
                // MessageBox.Show("Ctrl + Home was pressed.");
                e.Handled = true; // Optional: Prevent further processing
            }
        }
        private void ScrollToBeginning(RichTextBox richTextBox)
        {
            richTextBox.SelectionStart = 0; // Set selection start to the beginning
            richTextBox.ScrollToCaret(); // Scroll to the caret
        }

        private void ScrollToEnd(RichTextBox richTextBox)
        {
            richTextBox.SelectionStart = richTextBox.Text.Length; // Set selection start to the end
            richTextBox.ScrollToCaret(); // Scroll to the caret
        }
    }

    public static class WindowExtensions
    {
        public static bool IsOutOfScreenBounds(this Form form)
        {
            // Get the bounds of all screens
            Rectangle totalScreenBounds = Rectangle.Empty;
            foreach (var screen in Screen.AllScreens)
            {
                totalScreenBounds = Rectangle.Union(totalScreenBounds, screen.Bounds);
            }

            // Check if the form is outside the total screen bounds
            return !totalScreenBounds.Contains(form.Bounds);
        }
    }

    public static class RtfMarginHelper
    {
        public static MarginSt GetMarginsFromString(string rtf)
        {
            int? left = null, right = null, top = null, bottom = null;

            var matches = Regex.Matches(rtf, @"\\marg([lrtb])(-?\d+)");
            foreach (Match m in matches)
            {
                switch (m.Groups[1].Value)
                {
                    case "l": left = int.Parse(m.Groups[2].Value); break;
                    case "r": right = int.Parse(m.Groups[2].Value); break;
                    case "t": top = int.Parse(m.Groups[2].Value); break;
                    case "b": bottom = int.Parse(m.Groups[2].Value); break;
                }
            }

            if (left.HasValue && right.HasValue && top.HasValue && bottom.HasValue)
                return new MarginSt (left.Value, right.Value, top.Value, bottom.Value);

            return MarginSt.NullMargin;      // Margins not fully specified
        }

        public static MarginSt GetMargins(string rtfFilePath)
        {
            string rawRtf = File.ReadAllText(rtfFilePath);
            return RtfMarginHelper.GetMarginsFromString(rawRtf);
        }

        /// <summary>
        /// Set the margins in the given rtf file.  Uses twips (1/1440 inch).
        /// </summary>
        public static void SetMarginsInDoc(string rtfFilePath, MarginSt mar)
        {
            if (mar.IsNull())
                return;

            string rawRtf = File.ReadAllText(rtfFilePath);
            string rtfWithMargins = AddMarginsToRtf(rawRtf, mar);
            File.WriteAllText(rtfFilePath, rtfWithMargins);
        }

        /// AddMarginsToRtf
        /// 
        /// <summary>
        /// Insert margin info into the given rtf string.
        /// </summary>
        public static string AddMarginsToRtf(string rawRtf, MarginSt mar)
        {            
            int left = (int)Math.Round(mar.Left);       // Store as twips:
            int right = (int)Math.Round(mar.Right);
            int top = (int)Math.Round(mar.Top);
            int bottom = (int)Math.Round(mar.Bottom);

            string rtfWithMargins = RtfMarginHelper.SetMarginsInString(rawRtf, left, right, top, bottom);
            return rtfWithMargins;
        }

        public static string SetMarginsInString(string rtf, int left, int right, int top, int bottom)
        {
            return RtfMarginHelper.SetRtfMargins(rtf, left, right, top, bottom);
        }

        public static string SetRtfMargins(string rtf, int leftTwips, int rightTwips, int topTwips, int bottomTwips)
        {
            // Remove any existing margin settings
            string cleanedRtf = Regex.Replace(rtf, @"\\marg[lrtb]-?\d+", "");
            cleanedRtf = Regex.Replace(cleanedRtf, @"\\marg[lrtb]sxn-?\d+", "");  // Takes out section margins.  Kludge.

            // Build new margin string
            string marginTags = $"\\margl{leftTwips}\\margr{rightTwips}\\margt{topTwips}\\margb{bottomTwips}";

            // Find insertion point: right after \paperw...\paperh... block
            var match = Regex.Match(cleanedRtf, @"(\\paperw\d+\\paperh\d+)");
            if (match.Success)
            {
                int insertPos = match.Index + match.Length;
                return cleanedRtf.Insert(insertPos, marginTags);
            }

            match = Regex.Match(cleanedRtf, @"(\\paperh\d+\\paperw\d+)");
            if (match.Success)
            {
                int insertPos = match.Index + match.Length;
                return cleanedRtf.Insert(insertPos, marginTags);
            }

            var headerMatch = Regex.Match(rtf, @"(\\rtf\d+)", RegexOptions.Singleline);
            if (headerMatch.Success)
            {
                int insertIndex = headerMatch.Index + headerMatch.Length;
                //string paperAndMargins = "\\paperw12240\\paperh15840\\margl1440\\margr1440\\margt1440\\margb1440";
                rtf = rtf.Insert(insertIndex, marginTags);
                return rtf;
            }

            // If no paper size or header found, return original.
            return rtf;
        }

        public static string InsertRtfMargins(string rtf, int leftTwips, int rightTwips, int topTwips, int bottomTwips)
        {
            // Define the margin tags
            string marginTags = $"\\margl{leftTwips}\\margr{rightTwips}\\margt{topTwips}\\margb{bottomTwips}";

            // Find the paper size tag to inject after it (usually near \paperw and \paperh)
            int insertIndex = rtf.IndexOf(@"\paperw");
            if (insertIndex == -1) return rtf;

            // Move to the end of the line containing paper settings (e.g., end of \paperhNNNN)
            int nextSpace = rtf.IndexOf('\\', insertIndex + 1);
            if (nextSpace == -1) return rtf;

            // Insert the margin settings right after paper settings
            return rtf.Insert(nextSpace, marginTags);
        }
    }

    public struct MarginSt
    {
        public float Left;
        public float Right;
        public float Top;
        public float Bottom;
        public static readonly MarginSt NullMargin = new MarginSt(-1, -1, -1, -1);

        public MarginSt(float left, float right, float top, float bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }

        internal bool IsNull()
        {
            return Left == -1 && Right == -1 && Top == -1 && Bottom == -1;
        }
    }
}