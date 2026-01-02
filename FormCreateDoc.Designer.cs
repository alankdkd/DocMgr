namespace DocMgr
{
    partial class FormCreateDoc
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
            buttonSelect = new Button();
            buttonCreate = new Button();
            label2 = new Label();
            label3 = new Label();
            labelPath = new Label();
            textBoxDocName = new TextBox();
            buttonClose = new Button();
            SuspendLayout();
            // 
            // buttonSelect
            // 
            buttonSelect.Font = new Font("Calibri", 20.25F, FontStyle.Bold);
            buttonSelect.Location = new Point(925, 36);
            buttonSelect.Name = "buttonSelect";
            buttonSelect.Size = new Size(46, 38);
            buttonSelect.TabIndex = 1;
            buttonSelect.Text = "...";
            buttonSelect.UseVisualStyleBackColor = true;
            buttonSelect.Click += buttonSelect_Click;
            // 
            // buttonCreate
            // 
            buttonCreate.Enabled = false;
            buttonCreate.Location = new Point(826, 95);
            buttonCreate.Name = "buttonCreate";
            buttonCreate.Size = new Size(145, 37);
            buttonCreate.TabIndex = 2;
            buttonCreate.Text = "&Create";
            buttonCreate.UseVisualStyleBackColor = true;
            buttonCreate.Click += buttonCreate_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(110, 43);
            label2.Name = "label2";
            label2.Size = new Size(66, 23);
            label2.TabIndex = 4;
            label2.Text = "Folder:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(28, 95);
            label3.Name = "label3";
            label3.Size = new Size(148, 23);
            label3.TabIndex = 5;
            label3.Text = "Document Name:";
            // 
            // labelPath
            // 
            labelPath.AutoSize = true;
            labelPath.Location = new Point(184, 45);
            labelPath.MaximumSize = new Size(730, 23);
            labelPath.MinimumSize = new Size(730, 23);
            labelPath.Name = "labelPath";
            labelPath.Size = new Size(730, 23);
            labelPath.TabIndex = 6;
            // 
            // textBoxDocName
            // 
            textBoxDocName.Location = new Point(182, 92);
            textBoxDocName.Name = "textBoxDocName";
            textBoxDocName.Size = new Size(337, 31);
            textBoxDocName.TabIndex = 7;
            textBoxDocName.TextChanged += textBoxDocName_TextChanged;
            // 
            // buttonClose
            // 
            buttonClose.DialogResult = DialogResult.Cancel;
            buttonClose.Enabled = false;
            buttonClose.Location = new Point(826, 165);
            buttonClose.Name = "buttonClose";
            buttonClose.Size = new Size(145, 37);
            buttonClose.TabIndex = 8;
            buttonClose.Text = "&C&lose";
            buttonClose.UseVisualStyleBackColor = true;
            buttonClose.Click += buttonClose_Click;
            // 
            // FormCreateDoc
            // 
            AcceptButton = buttonCreate;
            AutoScaleDimensions = new SizeF(10F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDark;
            ClientSize = new Size(1006, 255);
            Controls.Add(buttonClose);
            Controls.Add(textBoxDocName);
            Controls.Add(labelPath);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(buttonCreate);
            Controls.Add(buttonSelect);
            Font = new Font("Calibri", 14.25F, FontStyle.Bold);
            KeyPreview = true;
            Margin = new Padding(4, 5, 4, 5);
            Name = "FormCreateDoc";
            Text = "Create New Document";
            Shown += FormCreateDoc_Shown;
            KeyPress += FormCreateDoc_KeyPress;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button buttonSelect;
        private Button buttonCreate;
        private Label label2;
        private Label label3;
        internal Label labelPath;
        internal TextBox textBoxDocName;
        private Button buttonClose;
    }
}