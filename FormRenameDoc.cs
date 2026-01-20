using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DocMgr
{
    public partial class FormRenameDoc : Form
    {
        string NewName, Extension;
        public string FolderForDoc;// { get => folderForDoc; set => folderForDoc = value; }
        //public string FolderForDoc { get => folderForDoc; set => folderForDoc = value; }

        public FormRenameDoc(string currentFilePath)
        {
            InitializeComponent();
            FolderForDoc = Path.GetDirectoryName(currentFilePath);
            labelPath.Text = FolderForDoc;
            labelCurrentDocName.Text = Path.GetFileNameWithoutExtension(currentFilePath);
            Extension = Path.GetExtension(currentFilePath);
            //DFW: buttonRename.CenterCursorInButton();
            Point here = new Point(buttonRename.Left + buttonRename.Width / 2, buttonRename.Top + buttonRename.Height / 2 + 6);
            Cursor.Position = PointToScreen(here);
            this.Shown += MyForm_Shown; //textNewDocName.Focus();
            DialogResult = DialogResult.Cancel;
        }

        private void MyForm_Shown(object sender, EventArgs e)
        {
            textNewDocName.Focus();
            this.Shown -= MyForm_Shown;
        }

        public string GetNewPath()
        {
            return NewName;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            textNewDocName.Text = "";
            Close();
        }

        private void buttonRename_Click(object sender, EventArgs e)
        {
            if (textNewDocName.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please enter a name for the new document.");
                textNewDocName.Focus();
                return;
            }

            NewName = Path.Combine(FolderForDoc, textNewDocName.Text.Trim());

            NewName += Extension;    // Keep the same extension as the original document.

            if (File.Exists(NewName))
            {
                MessageBox.Show($"File {NewName} already exists.  Please enter a new document name.");
                textNewDocName.SelectAll();
                textNewDocName.Focus();
                return;
            }

            //MessageBox.Show($"Document will be renamed to {Path.GetFileNameWithoutExtension(NewName)}.");
            DialogResult = DialogResult.OK;
            Close();
        }

        private void textNewDocName_TextChanged(object sender, EventArgs e)
        {
            buttonRename.Enabled = true;// (textNewDocName.Text.Trim().Length > 0);
        }
    }
}
