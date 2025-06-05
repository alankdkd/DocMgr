using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocMgr
{
    public partial class FindForm : Form
    {
        RichTextBox richTextBox;
        string DocName, ProjName;
        //List<int> listOffset;
        //List<int> listWidth;
        MatchCollection Matches;
        List<(string docName, string projectPath)> DocList;
        (string docName, string projectPath)? CurrentDoc = null;
        int CurrentDocNum = 0;
        string? CurrentProjectName;
        Doc CurrentProjectInfo;
        bool DirectionForward;
        Dictionary<string, string> projMap;     // Map project name to path.
        Doc Root;

        public FindForm(RichTextBox box, Doc projectRoot, string docName)
        {
            InitializeComponent();
            richTextBox = box;
            Root = projectRoot;
            DocName = docName;
            ProjName = projectRoot.DocName;
            CurrentProjectName = "";
            this.StartPosition = FormStartPosition.Manual;

            // Set specific screen coordinates (e.g., X = 200, Y = 150)
            this.Location = new Point(box.Right + 20, box.Bottom - Height);
            Cursor.Position = new Point(textString.Left + 10, textString.Top + 8);
            buttonFind.CenterCursorInButton();
            this.textString.Focus();
        }

        private void buttonFind_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            GetDocsInScope(DocList);
            DocList = GetDocsWithInstances(DocList);
            buttonNext.Enabled = DocList.Count > 0;
            buttonPrevious.Enabled = DocList.Count > 0;
            DirectionForward = radioForward.Checked;
            CurrentDocNum = (DirectionForward)  ?  0  :  DocList.Count - 1;
            CurrentProjectName = null;
            Cursor.Current = Cursors.Default;

            //buttonNext_Click(null, null);
        }

        private void GetDocsInScope(object doclist)     // Get the doc/project pairs to process.
        {
            DocList = new();

            if (radioCurrentDoc.Checked)
            {
                if (DocName != ""  &&  ProjName != "")
                    DocList.Add(new(DocName, Root.DocPath));                // The only doc.
            }
            else
                if (radioCurrentProject.Checked)
                {
                    if (ProjName != "")
                        AddProjectsDocsToList(DocList, Root);       // All docs in project.
                }
                else
                    if (radioAllProjects.Checked)
                        AddAllProjectsDocsToList(DocList);        // All projects.
        }

        private List<(string docName, string projectPath)> GetDocsWithInstances(List<(string docName, string projectPath)> docList)
        {
            List<(string docName, string projectPath)> docsWithList = new();    // With instances.
            HashSet<string> projectNames = new();                               // Proj-name set for summary.
            bool matchCase = checkMatchCase.Checked;
            bool matchWholeWord = checkMatchWholeWord.Checked;
            int numMatches, totalMatches = 0;
            string summary;

            foreach (var doc in docList)
                if (DocContainsString(doc, textString.Text, matchCase, matchWholeWord, out numMatches))
                {
                    docsWithList.Add(doc);
                    totalMatches += numMatches;
                    AddProjectToSet(projectNames, doc);
                }

            summary = FormatFindResults(totalMatches, projectNames, docsWithList);
            labelFindResults.Text = summary;
            return docsWithList;
        }

        private bool DocContainsString((string docName, string projectPath) doc, string searchString,
            bool matchCase, bool matchWholeWord, out int numMatches)
        {
            Doc? newDocRoot;

            LoadProject(doc.projectPath, out newDocRoot);

            if (newDocRoot == null)
            {
                MessageBox.Show($"Warning: Project not found at {doc.projectPath} in DocContainsString().");
                numMatches = 0;
                return false;
            }

            string pathToDoc = GetPathToDoc(doc.docName, newDocRoot);

            if (pathToDoc.Length == 0  ||  !File.Exists(pathToDoc))
            {
                if (pathToDoc.Length == 0)
                    MessageBox.Show($"Warning: Document {doc.docName} not found in project.");
                else
                    MessageBox.Show($"Warning: Document not found at {pathToDoc}.");

                numMatches = 0;
                return false;
            }

            richTextBox.LoadFile(pathToDoc);
            string rtf = richTextBox.Rtf;

            string highlightRtf = HighlightSearchStringInRtf(rtf, searchString, matchCase, matchWholeWord);
            richTextBox.Rtf = highlightRtf;

            if (Matches == null)
                numMatches = 0;
            else
                numMatches = Matches.Count();

            return numMatches > 0;
        }

        private void AddProjectToSet(HashSet<string> projectNames, (string docName, string projectPath) doc)
        {
            string projectName = Path.GetFileNameWithoutExtension(doc.projectPath);
            projectNames.Add(projectName);
        }

        private string FormatFindResults(int totalMatches, HashSet<string> projectNames,
            List<(string docName, string projectPath)> docsWithList)
        {
            StringBuilder sb = new();

            if (totalMatches == 0)
                sb.Append("No matches found");
            else
            {
                sb.Append($"Found {totalMatches} instance");

                if (totalMatches != 1)
                    sb.Append("s");

                if (!radioCurrentDoc.Checked)
                {
                    sb.Append($" in {docsWithList.Count} document");

                    if (docsWithList.Count != 1)
                        sb.Append("s");
                }

                if (radioAllProjects.Checked)
                {
                    sb.Append($" in {projectNames.Count} project");

                    if (projectNames.Count != 1)
                        sb.Append("s");
                }
            }

            sb.Append(".");
            return sb.ToString();
        }

        private void AddProjectsDocsToList(List<(string docName, string projectPath)> docList, Doc project)
        {
            if (project.DocPath.Length == 0)
            {
                MessageBox.Show($"Warning: Couldn't get path to project {project.DocName}.");
                return;
            }

            foreach (Doc doc in project.SubDocs)               // Add all of this project's docs to Doc List:
                DocList.Add(new (doc.DocName, project.DocPath));
        }

        private void AddAllProjectsDocsToList(List<(string docName, string projectPath)> docList)
        {
            LoadProjectDlg LoadProj = new();                // Add all projects' docs to Doc List:
            projMap = LoadProjectDlg.GetProjMap();

            foreach (KeyValuePair<string, string> pair in projMap)
            {
                string docName = pair.Key;
                Doc project;
                LoadProject(pair.Value, out project);
                AddProjectsDocsToList(docList, project);
            }
        }

        //private void buttonClose_Click(object sender, EventArgs e)
        //{
        //    //Close();
        //}

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (DocList.Count == 0)
                return;

            CurrentDoc = DocList[CurrentDocNum];

            if (++CurrentDocNum == DocList.Count)
                CurrentDocNum = 0;

            buttonNext.Enabled = buttonPrevious.Enabled = (DocList.Count > 1);
            DisplayCurrentDoc(CurrentDoc);
        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            if (DocList.Count == 0)
                return;

            CurrentDoc = DocList[CurrentDocNum];

            if (--CurrentDocNum == -1)
                CurrentDocNum = DocList.Count - 1;

            DisplayCurrentDoc(CurrentDoc);
        }

        private void DisplayCurrentDoc((string docName, string projectPath)? currentDoc)
        {
            if (currentDoc == null)
            {
                MessageBox.Show("Warning: currentDoc is null in DisplayCurrentDoc().  Continuing...");
                return;
            }

            if (currentDoc.Value.projectPath  !=  CurrentProjectName)
                if (!LoadCurrentProject(currentDoc))            // Project out of date.  Load project.
                    return;

            string CurrentDocPath = GetPathToDoc(currentDoc.Value.docName, CurrentProjectInfo);

            if (CurrentDocPath.Length == 0)
                return;

            richTextBox.LoadFile(CurrentDocPath);
        }

        private string GetPathToProject(string projName)
        {
            foreach (Doc doc in Root.SubDocs)
                if (doc.DocName == projName)
                    return doc.DocPath;

            return "";
        }

        private string GetPathToDoc(string docName, Doc currentProjectInfo)
        {
            foreach (Doc doc in currentProjectInfo.SubDocs)     // Go thru project's docs:
                if (doc.DocName == docName)
                    return doc.DocPath;                         // Found it!

            return "";                                          // Signal not found.
        }

        private bool LoadCurrentProject((string docName, string projectPath)? currentDoc)
        {
            string projPath = currentDoc.Value.projectPath;
            CurrentProjectName = Path.GetFileNameWithoutExtension(projPath);

            if (string.IsNullOrEmpty(projPath) || !File.Exists(projPath))
            {
                MessageBox.Show($"Warning: Project not found at {projPath} in LoadCurrentProject().  Continuing...");
                return false;
            }

            //WRONG  CurrentProjectInfo = new Doc(projPath, CurrentProjectName);
            LoadProject(projPath, out CurrentProjectInfo);
            return true;
        }

        private void LoadProject(string projectPath, out Doc? Root)     // This duplicates functionality in Form1.cs.  Ugh.  Need Project class.
        {
            Root = null;

            if (File.Exists(projectPath))
            {                               // Load project file:
                string? docList = File.ReadAllText(projectPath);
// CONNECT THIS TO CONTROL IN Form1:        ProjectName.Text = Path.GetFileNameWithoutExtension(projectPath);

                if (docList != null)
                    try
                    {
                        Root = System.Text.Json.JsonSerializer.Deserialize<Doc>(docList);
                        Root.DocPath = projectPath;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Problem loading project at {projectPath}: " + ex.Message);
                        //Root = new Doc("Root");
                        return;
                    }
            }
            else
                MessageBox.Show($"Warning: Project not found at {projectPath} in LoadProject().");
        }

        public string HighlightSearchStringInRtf(string rtf, string searchString, bool caseSensitive = false, bool wholeWord = false)
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
            Matches = regex.Matches(plainText);
            if (Matches.Count == 0)
                return rtf;

            // Apply yellow background highlight to matches
            foreach (Match match in Matches)
            {
                rtb.Select(match.Index, match.Length);
                rtb.SelectionBackColor = Color.Yellow;
                RichTextBoxScroller.ScrollToOffsetCenter(richTextBox, match.Index);
            }

            return rtb.Rtf;
        }
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
    }
}
