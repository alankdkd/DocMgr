using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Win32;

namespace DocMgr
{
    public partial class DocMgr : Form
    {
        readonly int BUTTON_SPACING = 40;
        readonly Font font = new Font("Calibri", 14, FontStyle.Bold);
        string? currentFilePath;
        bool loadingDoc = false;

        private Doc Root = new Doc("Root");
        private static Point ButtonListStart { get; } = new Point(10, 80);
        public DocMgr()
        {
            InitializeComponent();
        }

        //string fileName = "DocMgr.json";
        private void DocMgr_Load(object sender, EventArgs e)
        {
            string? lastProjectPath = GetLastProjectPath();

            CenterToScreen();

            if (lastProjectPath != null)
            {
                LoadProject(lastProjectPath, out Root);
                MakeButtons(Root.SubDocs);
                richTextBox.Clear();

                if (Root.SubDocs.Count == 1)
                    ClickSingleButton();


            // NEXT STEPS: Need to serialize default font.  Then add button to use font dlg to set default font.
            // (LATER) When new doc created, set font from default font.
            // Get code project font mgr running.
            // Read/save rtf from control to/from .rtf file.

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

                //var options = new JsonSerializerOptions { WriteIndented = true };
                //string jsonString = System.Text.Json.JsonSerializer.Serialize(doc, options);
                //File.WriteAllText(fileName, jsonString);


                // Deserializes doc:
                //string docs;
                //docs = File.ReadAllText(fileName);
                //MessageBox.Show(docs);
                //var docOut = JsonConvert.DeserializeObject(docs);

                // Make configurable button:
                //Button b = new Button();
                //b.Text = "&New";
                //b.Font = new Font("Calibri", 14, FontStyle.Bold);
                //b.Name = "FirstDoc";
                //b.Tag = @"C:\Users\alank\Documents\DocMgr\temp large.rtf";
                //b.Click += SelectDocClick;
                //b.Location = ButtonListStart;
                //b.Size = new Size(120, 30);
                //Controls.Add(b);
            }
        }

        private void ClickSingleButton()
        {
            foreach(Control cont in Controls)
                if (cont is Button  &&  cont.Tag != null)
                {
                    SelectDocClick(cont, new EventArgs());
                    return;
                }    
        }

        private void MakeButtons(List<Doc>? subDocs)
        {
            Point next = ButtonListStart;

            RemoveOldButtons();

            foreach(Doc doc in subDocs)
            {
                // Make configurable button:
                Button b = new Button();
                b.Text = "&" + doc.DocName;
                b.Font = font;
                b.Name = doc.DocName;
                b.Tag = doc.Path;
                b.Click += SelectDocClick;
                b.Location = next;
                next.Y += BUTTON_SPACING;
                b.Size = new Size(120, 30);
                Controls.Add(b);
            }
        }

        private void RemoveOldButtons()
        {
            bool changed;

            do
            {
                changed = false;

                foreach (Control cont in Controls)
                    if (cont is Button && (cont as Button).Tag != null)
                    {
                        Controls.Remove(cont as Button);
                        changed = true;
                        break;
                    }
            }
            while (changed);
       }

        private void SelectDocClick(object sender, EventArgs e)
        {
            loadingDoc = true;

            richTextBox.Clear();
            Button but = sender as Button;
            DocName.Text = but.Name + ':';
            currentFilePath = but.Tag.ToString();

            if (File.Exists(currentFilePath))
            {
                richTextBox.LoadFile(currentFilePath);
                buttonSaveDoc.Enabled = false;
            }
            else
                MessageBox.Show("File '" + currentFilePath + "' was not found.");

            richTextBox.Focus();
            loadingDoc = false;
        }

        private void ClickButtonWithName(string buttonName)
        {
            foreach (Control cont in Controls)
                if (cont is Button && cont.Name == buttonName)
                {
                    SelectDocClick(cont, new EventArgs());
                    buttonSaveDoc.Enabled = false;
                    return;
                }

        }
        private void LoadProject(string projectPath, out Doc Root)
        {
            if (File.Exists(projectPath))
            {                               // Load project file:
                string docList;
                docList = File.ReadAllText(projectPath);

                try
                {
                    Root = System.Text.Json.JsonSerializer.Deserialize<Doc>(docList);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Problem loading project: " + ex.Message);
                    Root = new Doc("Root");
                    return;
                }
                ProjectName.Text = Path.GetFileNameWithoutExtension(projectPath);
            }
            else
            {                               // File not found:
                MessageBox.Show("Project file '" + projectPath + "' not found.");
                Root = new Doc("Root");
                DocName.Text = "";
            }
        }

        private string? GetLastProjectPath()
        {
            RegistryKey? key = Registry.CurrentUser.CreateSubKey(@"Software\PatternScope Systems\DocMgr");
            object? obj = key.GetValue("LastProjectPath");

            return (obj == null)  ?  null  :  obj.ToString();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DocMgr_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (int) Keys.Escape)
                buttonClose_Click(sender, e);           // User wants to exit.

            //richTextBox.Modified = true;

            if (DocName.Text.Length == 0)
                DocName.Text = "Document";              // Default name for new document.
        }

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            DocMgr_KeyPress(sender, e);                 // Forward key to parent form.
        }

        private void buttonLoadProj_Click(object sender, EventArgs e)
        {
            string? projectPath = SelectProjectFile();
            buttonSaveDoc.Enabled = false;
            loadingDoc = true;

            if (projectPath != null)
            {
                LoadProject(projectPath, out Root);
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
            string? projectPath = SelectFile("json files (*.json)|*.json|All files (*.*)|*.*");
            RegistryKey? key;

            if (projectPath != null)
            {                                               // Project file selected:
                key = Registry.CurrentUser.CreateSubKey(@"Software\PatternScope Systems\DocMgr");
                key.SetValue("LastProjectPath", projectPath.ToString());
            }

            return projectPath;
        }

        private string? SelectFile(string filter)
        {
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

        private void buttonLoadDoc_Click(object sender, EventArgs e)
        {
            string? docPath = SelectFile("rtf files (*.rtf)|*.rtf|All files (*.*)|*.*");
            loadingDoc = true;

            if (docPath != null)
            {
                string docName = Path.GetFileNameWithoutExtension(docPath);

                if (!DocAlreadyInProject(docName))
                {
                    AddPathToRoot(docPath);
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
            File.WriteAllText(GetLastProjectPath(), jsonString);
        }

        private void AddPathToRoot(string docPath)
        {
            string name = Path.GetFileNameWithoutExtension(docPath);
            Root.SubDocs.Add(new Doc(name, docPath));
        }

        private void buttonSaveDoc_Click(object sender, EventArgs e)
        {
            if (currentFilePath != null)
            {
                richTextBox.SaveFile(currentFilePath);

                if (DocName.Text[0] == '*')
                    DocName.Text = DocName.Text.Remove(0, 1);
            }

            buttonSaveDoc.Enabled = false;
        }

        private void buttonRemoveDoc_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!loadingDoc)
            {
                buttonSaveDoc.Enabled = true;

                if (DocName.Text[0] != '*')         // Indicate docuement modified:
                    DocName.Text = DocName.Text.Insert(0, "* ");
            }
        }
    }
}