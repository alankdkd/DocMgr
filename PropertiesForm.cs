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
    public partial class PropertiesForm : Form
    {
        public PropertiesForm()
        {
            InitializeComponent();
        }

        //ORIG: internal void SetProperties(Properties.Settings settings)
        internal void SetProperties(DocMgr.MySettings settings)
        {
            propertyGrid.SelectedObject = settings;
        }

        public bool SaveSettings { get; set; }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveSettings = true;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            SaveSettings = false;
            Close();
        }
    }
}
