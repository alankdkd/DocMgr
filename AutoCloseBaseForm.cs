using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocMgr
{
    public class AutoCloseBaseForm : Form
    {
        private System.Windows.Forms.Timer _clickTracker;

        public AutoCloseBaseForm()
        {
            _clickTracker = new System.Windows.Forms.Timer();
            _clickTracker.Interval = 50; // 20 times per second
            _clickTracker.Tick += ClickTracker_Tick;
            _clickTracker.Start();
        }

        protected virtual void ClickTracker_Tick(object sender, EventArgs e)
        {
            // Check if the mouse is pressed AND if it's outside the form bounds
            if (Control.MouseButtons != MouseButtons.None)
            {
                if (!this.DesktopBounds.Contains(Cursor.Position))
                {
                    this.Close();
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _clickTracker?.Stop();
                _clickTracker?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
