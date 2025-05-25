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
    public partial class FindForm : Form
    {
        List<int> listOffset;
        List<int> listWidth;

        public FindForm(RichTextBox box)
        {
            InitializeComponent();
            listOffset = new();
            listWidth = new();
        }

        public List<int> ListOffset { get => listOffset; set => listOffset = value; }
        public List<int> ListWidth { get => listWidth; set => listWidth = value; }

        private void buttonFind_Click(object sender, EventArgs e)
        {

        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {

        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {

        }
    }
}
