namespace DocMgr
{
    partial class CreateFolderDialog
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
            label2 = new Label();
            textParentFolder = new TextBox();
            textProjName = new TextBox();
            buttonOk = new Button();
            buttonCancel = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Calibri", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(33, 45);
            label1.Name = "label1";
            label1.Size = new Size(123, 23);
            label1.TabIndex = 0;
            label1.Text = "Parent Folder:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Calibri", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(33, 96);
            label2.Name = "label2";
            label2.Size = new Size(123, 23);
            label2.TabIndex = 2;
            label2.Text = "Project Name:";
            // 
            // textParentFolder
            // 
            textParentFolder.Font = new Font("Calibri", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            textParentFolder.Location = new Point(162, 40);
            textParentFolder.Name = "textParentFolder";
            textParentFolder.Size = new Size(464, 31);
            textParentFolder.TabIndex = 3;
            // 
            // textProjName
            // 
            textProjName.Font = new Font("Calibri", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            textProjName.Location = new Point(162, 91);
            textProjName.Name = "textProjName";
            textProjName.Size = new Size(464, 31);
            textProjName.TabIndex = 4;
            // 
            // buttonOk
            // 
            buttonOk.DialogResult = DialogResult.OK;
            buttonOk.Font = new Font("Calibri", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            buttonOk.Location = new Point(667, 38);
            buttonOk.Name = "buttonOk";
            buttonOk.Size = new Size(94, 34);
            buttonOk.TabIndex = 5;
            buttonOk.Text = "&OK";
            buttonOk.UseVisualStyleBackColor = true;
            buttonOk.Click += buttonOk_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.DialogResult = DialogResult.Cancel;
            buttonCancel.Font = new Font("Calibri", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            buttonCancel.Location = new Point(667, 87);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(94, 34);
            buttonCancel.TabIndex = 6;
            buttonCancel.Text = "&Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // CreateFolderDialog
            // 
            AcceptButton = buttonOk;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDark;
            ClientSize = new Size(800, 164);
            Controls.Add(buttonCancel);
            Controls.Add(buttonOk);
            Controls.Add(textProjName);
            Controls.Add(textParentFolder);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "CreateFolderDialog";
            Load += CreateFolderDialog_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox textParentFolder;
        internal TextBox textProjName;
        private Button buttonOk;
        private Button buttonCancel;
    }
}