using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        RichTextBox richTextBox, tempRTBox = new();
        string DocName, ProjName;
        //List<int> listOffset;
        //List<int> listWidth;
        MatchCollection Matches;
        List<(string docName, string projectPath)> DocList;
        (string docName, string projectPath)? CurrentDoc = null;
        int CurrentDocNum = 0;
        string? CurrentProjectName;
        string? CurrentDocPath = null;
        string SearchString;
        Doc CurrentProjectInfo;
        bool DirectionForward = true;
        int TotalMatches, NumMatches;
        int MatchOrderInDoc;
        int OrangeStart, OrangeLength;
        bool MatchCase;
        bool MatchWholeWord;

        Dictionary<string, string> projMap;     // Map project name to path.
        Doc Root;
        DocMgr mainForm;
        public string? DisplayedDoc = null;


        public FindForm(RichTextBox box, Doc projectRoot, string docName, DocMgr callingForm)
        {
            InitializeComponent();
            richTextBox = box;
            Root = projectRoot;
            DocName = docName;
            ProjName = projectRoot.DocName;
            mainForm = callingForm;
            CurrentProjectName = "";
            this.StartPosition = FormStartPosition.Manual;

            // Set specific screen coordinates (e.g., X = 200, Y = 150)
            this.Location = new Point(box.Right + 20, box.Bottom - Height);
            Cursor.Position = new Point(textString.Left + 10, textString.Top + 8);
            this.textString.Focus();
        }

        private void buttonFind_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            labelFindResults.Text = "";
            labelInstanceOrder.Text = "";
            GetDocsInScope(DocList);
            DocList = GetDocsWithInstances(DocList);
            buttonNext.Enabled = DocList.Count > 0;
            buttonPrevious.Enabled = DocList.Count > 0;

            if (DocList.Count == 0)
            {
                labelFindResults.Text = "Not found.";
                return;
            }

            CurrentDocNum = GetNumberOfDoc(DocName, DocList);
            CurrentProjectName = null;
            DisplayedDoc = "";

            ShowDoc(DocList[CurrentDocNum]);
            buttonNext.Focus();
            Cursor.Current = Cursors.Default;

            //buttonNext_Click(null, null);
        }

        private int GetNumberOfDoc(string docName, List<(string docName, string projectPath)> docList)
        {
            int docNum = 0;
            foreach (var doc in docList)
                if (doc.docName == docName)
                    return docNum;
                else
                    ++docNum;

            return 0;           // Shouldn't happen.
        }

        private void GetDocsInScope(object doclist)     // Get the doc/project pairs to process.
        {
            DocList = new();

            if (radioCurrentDoc.Checked)
            {
                if (DocName != "" && ProjName != "")
                    DocList.Add(new(DocName, Root.DocPath));      // The only doc.
            }
            else
                if (radioCurrentProject.Checked)
            {
                if (ProjName != "")
                    AddProjectsDocsToList(DocList, Root);     // All docs in project.
            }
            else
                    if (radioAllProjects.Checked)
                AddAllProjectsDocsToList(DocList);        // All projects.
        }

        private List<(string docName, string projectPath)> GetDocsWithInstances(List<(string docName, string projectPath)> docList)
        {
            List<(string docName, string projectPath)> docsWithList = new();    // With instances.
            HashSet<string> projectNames = new();                               // Proj-name set for summary.
            MatchCase = checkMatchCase.Checked;
            MatchWholeWord = checkMatchWholeWord.Checked;
            string summary, position;
            TotalMatches = 0;

            SearchString = textString.Text;

            foreach (var doc in docList)
                if (DocContainsString(doc, SearchString, MatchCase, MatchWholeWord, out NumMatches))
                {
                    docsWithList.Add(doc);
                    TotalMatches += NumMatches;
                    AddProjectToSet(projectNames, doc);
                }

            summary = FormatFindResults(TotalMatches, projectNames, docsWithList);
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

            if (pathToDoc.Length == 0 || !File.Exists(pathToDoc))
            {
                if (pathToDoc.Length == 0)
                    MessageBox.Show($"Warning: Document {doc.docName} not found in project.");
                else
                    MessageBox.Show($"Warning: Document not found at {pathToDoc}.");

                numMatches = 0;
                return false;
            }

            tempRTBox.LoadFile(pathToDoc);
            string rtf = tempRTBox.Rtf;

            FindMatchesInString(rtf, searchString, matchCase, matchWholeWord);

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
                DocList.Add(new(doc.DocName, project.DocPath));
        }

        private void AddAllProjectsDocsToList(List<(string docName, string projectPath)> docList)
        {
            LoadProjectDlg LoadProj = new();                // Add all projects' docs to Doc List:
            projMap = LoadProjectDlg.GetProjMap();

            foreach (KeyValuePair<string, string> pair in projMap)
            {
                string docName = pair.Key;
                Doc project;

                if (!LoadProject(pair.Value, out project))
                    continue;

                AddProjectsDocsToList(docList, project);
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            DirectionForward = true;
            MoveHighlightedWord(DirectionForward);
        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            DirectionForward = false;
            MoveHighlightedWord(DirectionForward);
        }

        private void MoveHighlightedWord(bool moveMatchUp)
        {
            tempRTBox.Select(OrangeStart, OrangeLength);
            tempRTBox.SelectionBackColor = Color.Yellow;

            if (moveMatchUp)
                ++MatchOrderInDoc;
            else
                --MatchOrderInDoc;

            if (MatchOrderInDoc == -1)
            {
                --CurrentDocNum;

                if (CurrentDocNum == -1)
                    CurrentDocNum = DocList.Count() - 1;

                ShowDoc(DocList[CurrentDocNum]);
                return;
            }

            if (MatchOrderInDoc == NumMatches)
            {
                ++CurrentDocNum;

                if (CurrentDocNum == DocList.Count)
                    CurrentDocNum = 0;

                ShowDoc(DocList[CurrentDocNum]);
                return;
            }

            Match match = Matches[MatchOrderInDoc];

            tempRTBox.Select(match.Index, match.Length);
            tempRTBox.SelectionBackColor = Color.Orange;
            OrangeStart = match.Index;
            OrangeLength = match.Length;

            richTextBox.Rtf = tempRTBox.Rtf;

            int firstVisibleCharIndex = richTextBox.GetCharIndexFromPosition(new Point(0, 0));
            int lastVisibleCharIndex = richTextBox.GetCharIndexFromPosition(new Point(0, richTextBox.ClientSize.Height));

            if (match.Index < firstVisibleCharIndex || match.Index > lastVisibleCharIndex)
            {
                // It's off-screen — move caret and scroll
                richTextBox.SelectionStart = match.Index;
                richTextBox.SelectionLength = match.Length;
                richTextBox.ScrollToCaret();
            }
            else
            {
                // It's already visible — just update the selection
                richTextBox.SelectionStart = match.Index;
                richTextBox.SelectionLength = match.Length;
            }
        }

        private void ShowDoc((string docName, string projectPath)? currentDoc)
        {
            if (currentDoc == null)
            {
                MessageBox.Show("Warning: currentDoc is null in DisplayCurrentDoc().  Continuing...");
                return;
            }

            bool loadDoc = false;
            string position = $"Showing {CurrentDocNum + 1} of {TotalMatches}";
            labelInstanceOrder.Text = position;

            if (currentDoc.Value.projectPath != CurrentProjectName)
            {
                loadDoc = true;                             // If loading project, need to load doc.

                if (!LoadCurrentProject(currentDoc))        // Project out of date.  Load project.
                    return;
            }

            if (currentDoc.Value.docName != DisplayedDoc)
                loadDoc = true;                             // If displayed doc different, load current.

            if (loadDoc)
            {
                CurrentDocPath = GetPathToDoc(currentDoc.Value.docName, CurrentProjectInfo);

                if (CurrentDocPath.Length == 0)
                    return;

                tempRTBox.LoadFile(CurrentDocPath);
                DisplayedDoc = currentDoc.Value.docName;
            }

            SearchString = textString.Text;
            MatchCase = checkMatchCase.Checked;
            MatchWholeWord = checkMatchWholeWord.Checked;
            FindMatchesInString(tempRTBox.Rtf, SearchString, MatchCase, MatchWholeWord);

            string highlightedRtf = HighlightSearchStringInRtf(tempRTBox);

            NumMatches = Matches.Count();

            if (NumMatches == 0)
                return;

            MatchOrderInDoc = DirectionForward ? 0 : NumMatches - 1;

            Match match = Matches[MatchOrderInDoc];
            OrangeStart = match.Index;
            OrangeLength = match.Length;
            tempRTBox.Select(OrangeStart, OrangeLength);
            tempRTBox.SelectionBackColor = Color.Orange;

            richTextBox.Rtf = tempRTBox.Rtf;

            if (mainForm != null)
            {
                mainForm.DocName.Text = currentDoc.Value.docName + ':';
                mainForm.ProjectName.Text = Path.GetFileNameWithoutExtension(CurrentProjectInfo.DocPath);
            }

            RichTextBoxScroller.ScrollToOffsetCenter(richTextBox, match.Index);
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
            return LoadProject(projPath, out CurrentProjectInfo);
        }

        private bool LoadProject(string projectPath, out Doc? Root)     // This duplicates functionality in Form1.cs.  Ugh.  Need Project class.
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
                        return true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Problem loading project at {projectPath}: " + ex.Message);
                        //Root = new Doc("Root");
                        return false;
                    }
            }

            MessageBox.Show($"Warning: Project not found at {projectPath} in LoadProject().");
            return false;
        }

        /// <summary>
        /// Locate the offset and length of each match in the text and set Matches collection.
        /// </summary>
        /// <returns></returns>
        public void FindMatchesInString(string rtf, string searchString, bool caseSensitive = false, bool wholeWord = false)
        {
            if (string.IsNullOrEmpty(rtf) || string.IsNullOrEmpty(searchString))
                return;

            string plainText;

            // Load RTF into RichTextBox
            using (RichTextBox rtb = new RichTextBox())
            {
                rtb.Rtf = rtf;
                plainText = rtb.Text;
            }

            // Build regex pattern
            string pattern = Regex.Escape(searchString);
            if (wholeWord)
                pattern = $@"\b{pattern}\b";

            RegexOptions options = caseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase;
            Regex regex = new Regex(pattern, options);

            // Find matches:
            Matches = regex.Matches(plainText);

            //// Apply yellow background highlight to matches
            //foreach (Match match in Matches)
            //{
            //    rtb.Select(match.Index, match.Length);
            //    rtb.SelectionBackColor = Color.Yellow;
            //    RichTextBoxScroller.ScrollToOffsetCenter(richTextBox, match.Index);
            //}

            //return rtb.Rtf;
        }

        // BREAK OUT SEPARATE MATCH-FINDING AND YELLOW HIGHLIGHTING.
        /// <summary>
        /// Highlights match instances found in yellow.
        /// </summary>
        /// <returns></returns>
        public string HighlightSearchStringInRtf(RichTextBox rtb)
        {

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

        private void FindForm_Shown(object sender, EventArgs e)
        {
            buttonFind.CenterCursorInButton(0, 6);          // This has to be done after the ctor.
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            richTextBox.SelectAll();
            richTextBox.SelectionBackColor = Color.White;
            richTextBox.DeselectAll();

            if (CurrentDocPath != null)
                mainForm.CurrentFilePath = CurrentDocPath;

            if (!radioCurrentProject.Checked)
                DisplayedDoc = "";

        }
    }
}
