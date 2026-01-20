namespace DocMgr
{
    partial class FormRenameDoc
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
            buttonClose = new Button();
            labelPath = new Label();
            label3 = new Label();
            label2 = new Label();
            buttonRename = new Button();
            textNewDocName = new TextBox();
            label1 = new Label();
            labelCurrentDocName = new Label();
            SuspendLayout();
            // 
            // buttonClose
            // 
            buttonClose.DialogResult = DialogResult.Cancel;
            buttonClose.Font = new Font("Calibri", 14.25F, FontStyle.Bold);
            buttonClose.Location = new Point(833, 191);
            buttonClose.Name = "buttonClose";
            buttonClose.Size = new Size(145, 37);
            buttonClose.TabIndex = 14;
            buttonClose.Text = "&C&lose";
            buttonClose.UseVisualStyleBackColor = true;
            buttonClose.Click += buttonClose_Click;
            // 
            // labelPath
            // 
            labelPath.AutoSize = true;
            labelPath.Font = new Font("Calibri", 14.25F, FontStyle.Bold);
            labelPath.Location = new Point(191, 71);
            labelPath.MaximumSize = new Size(730, 23);
            labelPath.MinimumSize = new Size(730, 23);
            labelPath.Name = "labelPath";
            labelPath.Size = new Size(730, 23);
            labelPath.TabIndex = 12;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Calibri", 14.25F, FontStyle.Bold);
            label3.Location = new Point(178, 130);
            label3.Name = "label3";
            label3.Size = new Size(209, 23);
            label3.TabIndex = 11;
            label3.Text = "CurrentDocument Name:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Calibri", 14.25F, FontStyle.Bold);
            label2.Location = new Point(117, 69);
            label2.Name = "label2";
            label2.Size = new Size(66, 23);
            label2.TabIndex = 10;
            label2.Text = "Folder:";
            // 
            // buttonRename
            // 
            buttonRename.Enabled = false;
            buttonRename.Font = new Font("Calibri", 14.25F, FontStyle.Bold);
            buttonRename.Location = new Point(833, 123);
            buttonRename.Name = "buttonRename";
            buttonRename.Size = new Size(145, 37);
            buttonRename.TabIndex = 9;
            buttonRename.Text = "&Rename";
            buttonRename.UseVisualStyleBackColor = true;
            buttonRename.Click += buttonRename_Click;
            // 
            // textNewDocName
            // 
            textNewDocName.Font = new Font("Calibri", 14.25F, FontStyle.Bold);
            textNewDocName.Location = new Point(391, 197);
            textNewDocName.Name = "textNewDocName";
            textNewDocName.Size = new Size(337, 31);
            textNewDocName.TabIndex = 16;
            textNewDocName.TextChanged += textNewDocName_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Calibri", 14.25F, FontStyle.Bold);
            label1.Location = new Point(198, 200);
            label1.Name = "label1";
            label1.Size = new Size(189, 23);
            label1.TabIndex = 15;
            label1.Text = "New Document Name:";
            // 
            // labelCurrentDocName
            // 
            labelCurrentDocName.AutoSize = true;
            labelCurrentDocName.Font = new Font("Calibri", 14.25F, FontStyle.Bold);
            labelCurrentDocName.Location = new Point(391, 130);
            labelCurrentDocName.Name = "labelCurrentDocName";
            labelCurrentDocName.Size = new Size(208, 23);
            labelCurrentDocName.TabIndex = 17;
            labelCurrentDocName.Text = "Current Document Name";
            // 
            // FormRenameDoc
            // 
            AcceptButton = buttonRename;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDark;
            CancelButton = buttonClose;
            ClientSize = new Size(1008, 339);
            Controls.Add(labelCurrentDocName);
            Controls.Add(textNewDocName);
            Controls.Add(label1);
            Controls.Add(buttonClose);
            Controls.Add(labelPath);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(buttonRename);
            Name = "FormRenameDoc";
            Text = "FormRenameDoc";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonClose;
        internal Label labelPath;
        private Label label3;
        private Label label2;
        private Button buttonRename;
        internal TextBox textNewDocName;
        private Label label1;
        private Label labelCurrentDocName;
    }
}