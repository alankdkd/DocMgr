namespace DocMgr
{
    partial class LoadProjectDlg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            listBox1 = new ListBox();
            buttonBrowse = new Button();
            buttonClose = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(63, 66);
            label1.Margin = new Padding(5, 0, 5, 0);
            label1.Name = "label1";
            label1.Size = new Size(137, 25);
            label1.TabIndex = 0;
            label1.Text = "Select Project:";
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 25;
            listBox1.Location = new Point(68, 120);
            listBox1.Margin = new Padding(5);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(441, 554);
            listBox1.TabIndex = 1;
            listBox1.Click += listBox1_Click;
            listBox1.DoubleClick += listBox1_DoubleClick;
            // 
            // buttonBrowse
            // 
            buttonBrowse.Location = new Point(545, 123);
            buttonBrowse.Margin = new Padding(5);
            buttonBrowse.Name = "buttonBrowse";
            buttonBrowse.Size = new Size(135, 65);
            buttonBrowse.TabIndex = 3;
            buttonBrowse.Text = "&Browse...";
            buttonBrowse.UseVisualStyleBackColor = true;
            buttonBrowse.Click += buttonBrowse_Click;
            // 
            // buttonClose
            // 
            buttonClose.Location = new Point(545, 220);
            buttonClose.Margin = new Padding(5);
            buttonClose.Name = "buttonClose";
            buttonClose.Size = new Size(135, 65);
            buttonClose.TabIndex = 4;
            buttonClose.Text = "&Close";
            buttonClose.UseVisualStyleBackColor = true;
            buttonClose.Click += buttonClose_Click;
            // 
            // LoadProjectDlg
            // 
            AutoScaleDimensions = new SizeF(11F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(744, 750);
            Controls.Add(buttonClose);
            Controls.Add(buttonBrowse);
            Controls.Add(listBox1);
            Controls.Add(label1);
            Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            Margin = new Padding(5);
            Name = "LoadProjectDlg";
            Text = "LoadProject";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ListBox listBox1;
        private Button buttonBrowse;
        private Button buttonClose;
    }
}