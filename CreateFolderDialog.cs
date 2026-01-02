using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocMgr
{
    public partial class CreateFolderDialog : Form
    {
        internal string NewFolderPath { get; set; }

        public CreateFolderDialog(string parentFolder)
        {
            InitializeComponent();
            this.CenterToScreen();
            textParentFolder.Text = parentFolder;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            string newFolderName = textProjName.Text;

            if (string.IsNullOrWhiteSpace(newFolderName))
            {
                MessageBox.Show("Folder name cannot be empty.");
                this.DialogResult = DialogResult.None;
                return;
            }

            NewFolderPath = Path.Combine(textParentFolder.Text, newFolderName);

            // Check if the folder exists, and create it if not
            try
            {
                if (!Directory.Exists(NewFolderPath))
                {
                    Directory.CreateDirectory(NewFolderPath);
                    Close();
                }
                else
                {
                    MessageBox.Show("Folder already exists: " + newFolderName);
                    this.DialogResult = DialogResult.None;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem creating folder: " + newFolderName);
                this.DialogResult = DialogResult.None;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CreateFolderDialog_Load(object sender, EventArgs e)
        {//DFW: textProjName.Focus();
            ActiveControl = textProjName;
        }

        private void CreateFolderDialog_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (int)Keys.Escape)
            {
                e.Handled = true;
                Close();                // User wants to exit.
            }
        }
    }
}
