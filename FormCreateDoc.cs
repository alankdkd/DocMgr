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
    public partial class FormCreateDoc : Form
    {
        string InitialFolder;

        public FormCreateDoc(string initialFolder = @"C:\")
        {
            InitializeComponent();
            buttonClose.Enabled = true;
            buttonSelect.CenterCursorInButton();
            InitialFolder = initialFolder;
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            labelPath.Text = string.Empty;
            textBoxDocName.Text = string.Empty;

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "rtf files (*.rtf)|*.rtf|All files (*.*)|*.*";
                ofd.CheckFileExists = false;
                ofd.CheckPathExists = false;
                ofd.InitialDirectory = InitialFolder;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    labelPath.Text = ofd.FileName;
                    textBoxDocName.Text = Path.GetFileNameWithoutExtension(ofd.FileName);
                    textBoxDocName.Enabled = true; ;
                    buttonCreate.Enabled = true;
                    DialogResult = DialogResult.OK;
                }

                buttonCreate.Enabled = textBoxDocName.Text.Length > 0;
            }
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            if (File.Exists(labelPath.Text))
            {
                MessageBox.Show("File " + labelPath.Text + " already exists.");
                return;
            }

            using (FileStream fs = File.Create(labelPath.Text))
                using (var sw = new StreamWriter(fs))
            {                                           // Crude kludge.  C# malfunctions saving blank RTF file.
                sw.Write(@"{\rtf1\ansi\ansicpg1252\deff0\nouicompat\deflang1033{\fonttbl{\f0\b1\fnil\fcharset0 Calibri;}}\
{\*\generator Riched20 10.0.22621}\viewkind4\uc1\
\pard\sa200\sl276\slmult1\f0\b1\fs22\lang9\par\
}");
                Close();
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormCreateDoc_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (int)Keys.Escape)
                buttonClose_Click(sender, e);           // User wants to exit.
        }
    }
}
