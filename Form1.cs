using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace DocMgr
{
    public partial class DocMgr : Form
    {
        readonly int BUTTON_SPACING = 40;
        readonly Font font = new Font("Calibri", 14, FontStyle.Bold);
        string? CurrentFilePath;
        string? ProjectPath, lastDocName;
        bool loadingDoc = false;

        private Doc? Root = new Doc("Root");
        private static Point ButtonListStart { get; } = new Point(10, 80);
        public DocMgr()
        {
            InitializeComponent();
            richTextBox.VScroll += (s, e) => {
                HandleScroll();
            };
            richTextBox.MouseWheel += (s, e) => {
                HandleScroll();
            };
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
            WM_VSCROLL = 0x0115
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

        private void HandleScroll()
        {
            int nPos = GetScrollPosition();

            foreach (var doc in Root.SubDocs)
                if (doc.DocName + ':' == DocName.Text)
                {
                    doc.ScrollPos = nPos;
                    string text = System.Text.Json.JsonSerializer.Serialize<Doc>(Root);
                    File.WriteAllText(ProjectPath, text);
                    return;
                }
        }

        private int GetScrollPosition()
        {
            return GetScrollPos(richTextBox.Handle, (int)ScrollBarType.SbVert);
        }

        private bool SetScrollPosition(int nPos)
        {
            return PostMessage(richTextBox.Handle, (int)Message.WM_VSCROLL,
                (IntPtr)((int)ScrollBarCommands.SB_THUMBPOSITION + 0x10000 * nPos), (IntPtr)0);
        }

        private void ButtonSaveAs_Click(object sender, EventArgs e)
        {
        }

        private void DocMgr_Load(object sender, EventArgs e)
        {
            ProjectPath = GetLastProjectPath();

            CenterToScreen();

            if (ProjectPath != null)
            {
                LoadProject(ProjectPath, out Root);
                MakeButtons(Root.SubDocs);
                richTextBox.Clear();

                if (Root.SubDocs == null)
                    Root.SubDocs = new List<Doc>();

                if (Root.SubDocs.Count == 1)
                    ClickSingleButton();            // Auto-load the single document.


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
            Point next = ButtonListStart;
            RemoveOldButtons();

            if (subDocs == null)
                return;

            foreach (Doc doc in subDocs)
            {
                // Make configurable button:
                Button b = new Button();
                b.Text = "&" + doc.DocName;
                b.Font = font;
                b.Name = doc.DocName;
                b.Tag = doc.DocPath;
                b.Click += SelectDocClick;
                b.Location = next;
                next.Y += BUTTON_SPACING;
                b.Size = new Size(120, 30);
                Controls.Add(b);
            }
        }

        private void RemoveOldButtons()     // Remove every document button.
        {                                   // These are the buttons with non-null Tag.
            bool changed;

            do
            {
                changed = false;

                foreach (Control cont in Controls)
                    if (cont is Button && ((cont as Button).Tag != null  ||
                        (lastDocName != null && (cont as Button).Name != null  && (cont as Button).Name == lastDocName)))
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
            loadingDoc = true;

            richTextBox.Clear();
            Button but = sender as Button;
            DocName.Text = but.Name + ':';

            if (but.Tag != null)
            {
                CurrentFilePath = but.Tag.ToString();

                if (File.Exists(CurrentFilePath))
                {
                    richTextBox.LoadFile(CurrentFilePath);
                    //richTextBox.AutoScrollOffset = new Point(100, 100);
                    ScrollToDocPosition(but.Name);
                    buttonSaveDoc.Enabled = false;
                    buttonRemoveDoc.Enabled = ProjectPath != null;
                }
                else
                    MessageBox.Show("File '" + CurrentFilePath + "' was not found.");
            }
            else
                MessageBox.Show("Can't select document.");

            if (richTextBox.Text.Length == 0)
                richTextBox.Font = font;

            buttonRemoveDoc.Enabled = true;
            richTextBox.Focus();
            loadingDoc = false;
        }

        private void ScrollToDocPosition(string name)
        {
            foreach (var doc in Root.SubDocs)
                if (doc.DocName == name)
                {
                    SetScrollPosition(doc.ScrollPos);
                    return;
                }
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
                        LoadProjectDlg.AddProject(ProjectName.Text, projectPath);
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
                buttonRemoveDoc.Enabled = false;
            }

            buttonRemoveDoc.Enabled = false;
            CurrentFilePath = null;
        }

        private void SaveProject(string projectPath, Doc? root)
        {
            string stringDoc = System.Text.Json.JsonSerializer.Serialize(root);
            File.WriteAllText(projectPath, stringDoc);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DocMgr_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (int)Keys.Escape)
                buttonClose_Click(sender, e);           // User wants to exit.

            if (e.KeyChar == 19)                        // Ctrl-S.  Civilized way to do this not apparent.
                buttonSaveDoc_Click(sender, e);         // Save document.

            if (DocName.Text.Length == 0)
            {
                DocName.Text = "Document";              // Default name for new document.
                buttonRemoveDoc.Enabled = false;
            }

        }

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            DocMgr_KeyPress(sender, e);                 // Forward key to parent form.
        }

        private void buttonLoadProj_Click(object sender, EventArgs e)
        {
            ProjectPath = SelectProjectFile();
            buttonSaveDoc.Enabled = false;
            loadingDoc = true;

            if (ProjectPath != null)
            {
                LoadProject(ProjectPath, out Root);
                MakeButtons(Root.SubDocs);
                richTextBox.Clear();
                DocName.Text = "";

                if (Root.SubDocs.Count == 1)
                    ClickSingleButton();
            }

            loadingDoc = false;
        }

        private string? SelectProjectFile()
        {
            string? projectPath = null;
            LoadProjectDlg dlg = new LoadProjectDlg();
            DialogResult dr = dlg.ShowDialog();

            if (dr == DialogResult.OK)
                projectPath = dlg.selectedPath;
            
            return projectPath;
        }

        public static string? GetLastProjectPath()
        {
            RegistryKey? key = Registry.CurrentUser.CreateSubKey(
                @"Software\PatternScope Systems\DocMgr",
                RegistryKeyPermissionCheck.ReadWriteSubTree);
            object? obj = key.GetValue("LastProjectPath");

            return (obj == null) ? null : obj.ToString();
        }

        public static string? SelectFile(string filter)           // General method to select a file.
        {                                                   // Used to select project & document files.
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                string? path = GetLastProjectPath();        // From Registry.

                if (path == null)
                    path = "c:\\";                          // Default path.

                openFileDialog.InitialDirectory = path;
                openFileDialog.Filter = filter;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                    return openFileDialog.FileName;

                return null;
            }
        }

        private void buttonLoadDoc_Click(object sender, EventArgs e)        // Add & load an existing .rtf
        {                                                                   // file into the current project.
            string? docPath = SelectFile("rtf files (*.rtf)|*.rtf|All files (*.*)|*.*");
            loadingDoc = true;

            if (docPath != null)
            {
                string docName = Path.GetFileNameWithoutExtension(docPath);

                if (!DocAlreadyInProject(docName))
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
            foreach (Control cont in Controls)
                if (cont is Button && cont.Name == docName)
                    return true;

            return false;
        }

        private void WriteUpdatedPath()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = System.Text.Json.JsonSerializer.Serialize(Root, options);
            string? path = GetLastProjectPath();

            if (path != null)
                File.WriteAllText(path, jsonString);
        }

        private void buttonSaveDoc_Click(object sender, EventArgs e)
        {
            if (CurrentFilePath != null && DocName.Text.Length > 0)
            {
                // DOESN'T WORK FOR EMPTY DOC:
                richTextBox.SaveFile(CurrentFilePath);
                // DFW: File.WriteAllText(CurrentFilePath, richTextBox.Rtf);
                //Root = new Doc("Root");
                //string text = System.Text.Json.JsonSerializer.Serialize<Doc>(Root);
                //File.WriteAllText(CurrentFilePath, text);

                if (DocName.Text[0] == '*')
                    DocName.Text = DocName.Text.Remove(0, 2);
            }

            buttonSaveDoc.Enabled = false;
            richTextBox.Focus();
        }

        private void buttonRemoveDoc_Click(object sender, EventArgs e)      // Just removes from project.
        {                                                                   // .rtf file is untouched.
            if (DocName.Text == null || DocName.Text.Length == 0 || ProjectPath == null  ||  Root == null)
                return;

            lastDocName = DocName.Text;

            if (lastDocName[0] == '*')
                lastDocName = lastDocName.Remove(0, 2);

            if (lastDocName.EndsWith(':'))
                lastDocName = lastDocName.Remove(lastDocName.Length - 1, 1);

            foreach (Doc doc in Root.SubDocs)
                if (doc.DocName == lastDocName)
                {
                    richTextBox.Clear();
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

                if (DocName.Text[0] != '*')         // Indicate document modified:
                    DocName.Text = DocName.Text.Insert(0, "* ");
            }
        }

        private void ButtonNewDoc_Click(object sender, EventArgs e)
        {
            FormCreateDoc fcd = new FormCreateDoc();

            var tmp = fcd.ShowDialog();

            bool success = fcd.labelPath.Text.Length > 0 && fcd.textBoxDocName.Text.Length > 0;

            if (success == true  &&  Root != null)
            {
                CurrentFilePath = fcd.labelPath.Text;
                Root.AddDoc(fcd.labelPath.Text, fcd.textBoxDocName.Text);
                MakeButtons(Root.SubDocs);
                buttonSaveDoc.Enabled = true;
                SaveProject(ProjectPath, Root);
                DocName.Text = Path.GetFileNameWithoutExtension(CurrentFilePath);
                richTextBox.Clear();
                richTextBox.Font = font;
                richTextBox.Focus();
            }
        }

        private void ButtonNewProj_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
                ofd.CheckFileExists = false;
                ofd.CheckPathExists = false;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    CurrentFilePath = ofd.FileName;
                    Root = new Doc("Root");
                    SaveProject(CurrentFilePath, Root);
                    richTextBox.Clear();
                    ProjectName.Text = Path.GetFileNameWithoutExtension(CurrentFilePath);
                    DocName.Text = "";
                    MakeButtons(Root.SubDocs);
                }
            }
        }

        //public static void CenterCursorInButton(this Button but)
        //{
        //    Cursor.Position = new Point(but.Left + but.Width / 2, but.Top + but.Height / 2);
        //}
    }
}