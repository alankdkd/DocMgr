namespace DocMgr
{
    partial class DocMgr
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.richTextBox = new System.Windows.Forms.RichTextBox();
            this.DocName = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.ProjectName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonLoadProj = new System.Windows.Forms.Button();
            this.buttonLoadDoc = new System.Windows.Forms.Button();
            this.buttonRemoveDoc = new System.Windows.Forms.Button();
            this.buttonSaveDoc = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.ButtonNewDoc = new System.Windows.Forms.Button();
            this.ButtonNewProj = new System.Windows.Forms.Button();
            this.ButtonSaveAs = new System.Windows.Forms.Button();
            this.buttonNumberLines = new System.Windows.Forms.Button();
            this.toolTips = new System.Windows.Forms.ToolTip(this.components);
            this.buttonOpenFolder = new System.Windows.Forms.Button();
            this.buttonBackUpFile = new System.Windows.Forms.Button();
            this.buttonBackUpProject = new System.Windows.Forms.Button();
            this.buttonArchiveFile = new System.Windows.Forms.Button();
            this.buttonArchiveProject = new System.Windows.Forms.Button();
            this.buttonProperties = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBox
            // 
            this.richTextBox.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.richTextBox.Location = new System.Drawing.Point(143, 81);
            this.richTextBox.Name = "richTextBox";
            this.richTextBox.Size = new System.Drawing.Size(922, 864);
            this.richTextBox.TabIndex = 0;
            this.richTextBox.Text = "";
            this.richTextBox.TextChanged += new System.EventHandler(this.richTextBox_TextChanged);
            this.richTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.richTextBox1_KeyPress);
            // 
            // DocName
            // 
            this.DocName.AutoSize = true;
            this.DocName.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.DocName.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.DocName.Location = new System.Drawing.Point(143, 56);
            this.DocName.Name = "DocName";
            this.DocName.Size = new System.Drawing.Size(0, 23);
            this.DocName.TabIndex = 2;
            this.DocName.MouseHover += new System.EventHandler(this.DocName_MouseHover);
            // 
            // buttonClose
            // 
            this.buttonClose.BackColor = System.Drawing.SystemColors.Control;
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.buttonClose.Location = new System.Drawing.Point(1090, 16);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(175, 31);
            this.buttonClose.TabIndex = 3;
            this.buttonClose.Text = "&Close";
            this.buttonClose.UseVisualStyleBackColor = false;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // ProjectName
            // 
            this.ProjectName.AutoSize = true;
            this.ProjectName.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.ProjectName.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ProjectName.Location = new System.Drawing.Point(971, 64);
            this.ProjectName.Name = "ProjectName";
            this.ProjectName.Size = new System.Drawing.Size(0, 18);
            this.ProjectName.TabIndex = 4;
            this.ProjectName.MouseHover += new System.EventHandler(this.ProjectName_MouseHover);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(918, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 18);
            this.label1.TabIndex = 5;
            this.label1.Text = "Project: ";
            // 
            // buttonLoadProj
            // 
            this.buttonLoadProj.BackColor = System.Drawing.SystemColors.Control;
            this.buttonLoadProj.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.buttonLoadProj.Location = new System.Drawing.Point(142, 16);
            this.buttonLoadProj.Name = "buttonLoadProj";
            this.buttonLoadProj.Size = new System.Drawing.Size(175, 31);
            this.buttonLoadProj.TabIndex = 6;
            this.buttonLoadProj.Text = "Load &Project...";
            this.buttonLoadProj.UseVisualStyleBackColor = false;
            this.buttonLoadProj.Click += new System.EventHandler(this.buttonLoadProj_Click);
            // 
            // buttonLoadDoc
            // 
            this.buttonLoadDoc.BackColor = System.Drawing.SystemColors.Control;
            this.buttonLoadDoc.Enabled = false;
            this.buttonLoadDoc.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.buttonLoadDoc.Location = new System.Drawing.Point(360, 16);
            this.buttonLoadDoc.Name = "buttonLoadDoc";
            this.buttonLoadDoc.Size = new System.Drawing.Size(175, 31);
            this.buttonLoadDoc.TabIndex = 7;
            this.buttonLoadDoc.Text = "&Load Document...";
            this.buttonLoadDoc.UseVisualStyleBackColor = false;
            this.buttonLoadDoc.Click += new System.EventHandler(this.buttonLoadDoc_Click);
            // 
            // buttonRemoveDoc
            // 
            this.buttonRemoveDoc.BackColor = System.Drawing.SystemColors.Control;
            this.buttonRemoveDoc.Enabled = false;
            this.buttonRemoveDoc.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.buttonRemoveDoc.Location = new System.Drawing.Point(1090, 214);
            this.buttonRemoveDoc.Name = "buttonRemoveDoc";
            this.buttonRemoveDoc.Size = new System.Drawing.Size(175, 31);
            this.buttonRemoveDoc.TabIndex = 8;
            this.buttonRemoveDoc.Text = "&Remove Document";
            this.buttonRemoveDoc.UseVisualStyleBackColor = false;
            this.buttonRemoveDoc.Click += new System.EventHandler(this.buttonRemoveDoc_Click);
            // 
            // buttonSaveDoc
            // 
            this.buttonSaveDoc.BackColor = System.Drawing.SystemColors.Control;
            this.buttonSaveDoc.Enabled = false;
            this.buttonSaveDoc.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.buttonSaveDoc.Location = new System.Drawing.Point(578, 16);
            this.buttonSaveDoc.Name = "buttonSaveDoc";
            this.buttonSaveDoc.Size = new System.Drawing.Size(175, 31);
            this.buttonSaveDoc.TabIndex = 9;
            this.buttonSaveDoc.Text = "&Save Document";
            this.buttonSaveDoc.UseVisualStyleBackColor = false;
            this.buttonSaveDoc.Click += new System.EventHandler(this.buttonSaveDoc_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label2.Location = new System.Drawing.Point(9, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 18);
            this.label2.TabIndex = 10;
            this.label2.Text = "Documents:";
            // 
            // ButtonNewDoc
            // 
            this.ButtonNewDoc.BackColor = System.Drawing.SystemColors.Control;
            this.ButtonNewDoc.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.ButtonNewDoc.Location = new System.Drawing.Point(1090, 81);
            this.ButtonNewDoc.Name = "ButtonNewDoc";
            this.ButtonNewDoc.Size = new System.Drawing.Size(175, 31);
            this.ButtonNewDoc.TabIndex = 11;
            this.ButtonNewDoc.Text = "&New Document...";
            this.ButtonNewDoc.UseVisualStyleBackColor = false;
            this.ButtonNewDoc.Click += new System.EventHandler(this.ButtonNewDoc_Click);
            // 
            // ButtonNewProj
            // 
            this.ButtonNewProj.BackColor = System.Drawing.SystemColors.Control;
            this.ButtonNewProj.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.ButtonNewProj.Location = new System.Drawing.Point(1090, 146);
            this.ButtonNewProj.Name = "ButtonNewProj";
            this.ButtonNewProj.Size = new System.Drawing.Size(175, 31);
            this.ButtonNewProj.TabIndex = 12;
            this.ButtonNewProj.Text = "New Pro&ject...";
            this.ButtonNewProj.UseVisualStyleBackColor = false;
            this.ButtonNewProj.Click += new System.EventHandler(this.ButtonNewProj_Click);
            // 
            // ButtonSaveAs
            // 
            this.ButtonSaveAs.BackColor = System.Drawing.SystemColors.Control;
            this.ButtonSaveAs.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.ButtonSaveAs.Location = new System.Drawing.Point(796, 16);
            this.ButtonSaveAs.Name = "ButtonSaveAs";
            this.ButtonSaveAs.Size = new System.Drawing.Size(175, 31);
            this.ButtonSaveAs.TabIndex = 13;
            this.ButtonSaveAs.Text = "Save &As...";
            this.ButtonSaveAs.UseVisualStyleBackColor = false;
            this.ButtonSaveAs.Click += new System.EventHandler(this.ButtonSaveAs_Click);
            // 
            // buttonNumberLines
            // 
            this.buttonNumberLines.BackColor = System.Drawing.SystemColors.Control;
            this.buttonNumberLines.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.buttonNumberLines.Location = new System.Drawing.Point(1090, 353);
            this.buttonNumberLines.Name = "buttonNumberLines";
            this.buttonNumberLines.Size = new System.Drawing.Size(175, 31);
            this.buttonNumberLines.TabIndex = 14;
            this.buttonNumberLines.Text = "&Number Lines";
            this.buttonNumberLines.UseVisualStyleBackColor = false;
            this.buttonNumberLines.Click += new System.EventHandler(this.buttonNumberLines_Click);
            // 
            // buttonOpenFolder
            // 
            this.buttonOpenFolder.BackColor = System.Drawing.SystemColors.Control;
            this.buttonOpenFolder.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.buttonOpenFolder.Location = new System.Drawing.Point(1090, 284);
            this.buttonOpenFolder.Name = "buttonOpenFolder";
            this.buttonOpenFolder.Size = new System.Drawing.Size(175, 31);
            this.buttonOpenFolder.TabIndex = 15;
            this.buttonOpenFolder.Text = "&Open Folder";
            this.buttonOpenFolder.UseVisualStyleBackColor = false;
            this.buttonOpenFolder.Click += new System.EventHandler(this.buttonOpenFolder_Click);
            // 
            // buttonBackUpFile
            // 
            this.buttonBackUpFile.BackColor = System.Drawing.SystemColors.Control;
            this.buttonBackUpFile.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.buttonBackUpFile.Location = new System.Drawing.Point(1090, 422);
            this.buttonBackUpFile.Name = "buttonBackUpFile";
            this.buttonBackUpFile.Size = new System.Drawing.Size(175, 31);
            this.buttonBackUpFile.TabIndex = 16;
            this.buttonBackUpFile.Text = "&Back Up File";
            this.buttonBackUpFile.UseVisualStyleBackColor = false;
            this.buttonBackUpFile.Click += new System.EventHandler(this.buttonBackUpFile_Click);
            // 
            // buttonBackUpProject
            // 
            this.buttonBackUpProject.BackColor = System.Drawing.SystemColors.Control;
            this.buttonBackUpProject.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.buttonBackUpProject.Location = new System.Drawing.Point(1090, 465);
            this.buttonBackUpProject.Name = "buttonBackUpProject";
            this.buttonBackUpProject.Size = new System.Drawing.Size(175, 31);
            this.buttonBackUpProject.TabIndex = 17;
            this.buttonBackUpProject.Text = "Back Up &Project";
            this.buttonBackUpProject.UseVisualStyleBackColor = false;
            this.buttonBackUpProject.Click += new System.EventHandler(this.buttonBackUpProject_Click);
            // 
            // buttonArchiveFile
            // 
            this.buttonArchiveFile.BackColor = System.Drawing.SystemColors.Control;
            this.buttonArchiveFile.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.buttonArchiveFile.Location = new System.Drawing.Point(1090, 508);
            this.buttonArchiveFile.Name = "buttonArchiveFile";
            this.buttonArchiveFile.Size = new System.Drawing.Size(175, 31);
            this.buttonArchiveFile.TabIndex = 18;
            this.buttonArchiveFile.Text = "&Archive File";
            this.buttonArchiveFile.UseVisualStyleBackColor = false;
            this.buttonArchiveFile.Click += new System.EventHandler(this.buttonArchiveFile_Click);
            // 
            // buttonArchiveProject
            // 
            this.buttonArchiveProject.BackColor = System.Drawing.SystemColors.Control;
            this.buttonArchiveProject.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.buttonArchiveProject.Location = new System.Drawing.Point(1090, 551);
            this.buttonArchiveProject.Name = "buttonArchiveProject";
            this.buttonArchiveProject.Size = new System.Drawing.Size(175, 31);
            this.buttonArchiveProject.TabIndex = 19;
            this.buttonArchiveProject.Text = "&A&rchive Project";
            this.buttonArchiveProject.UseVisualStyleBackColor = false;
            this.buttonArchiveProject.Click += new System.EventHandler(this.buttonArchiveProject_Click);
            // 
            // buttonProperties
            // 
            this.buttonProperties.BackColor = System.Drawing.SystemColors.Control;
            this.buttonProperties.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.buttonProperties.Location = new System.Drawing.Point(1090, 619);
            this.buttonProperties.Name = "buttonProperties";
            this.buttonProperties.Size = new System.Drawing.Size(175, 31);
            this.buttonProperties.TabIndex = 20;
            this.buttonProperties.Text = "&Properties...";
            this.buttonProperties.UseVisualStyleBackColor = false;
            this.buttonProperties.Click += new System.EventHandler(this.buttonProperties_Click);
            // 
            // DocMgr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(1296, 985);
            this.Controls.Add(this.buttonProperties);
            this.Controls.Add(this.buttonArchiveProject);
            this.Controls.Add(this.buttonArchiveFile);
            this.Controls.Add(this.buttonBackUpProject);
            this.Controls.Add(this.buttonBackUpFile);
            this.Controls.Add(this.buttonOpenFolder);
            this.Controls.Add(this.buttonNumberLines);
            this.Controls.Add(this.ButtonSaveAs);
            this.Controls.Add(this.ButtonNewProj);
            this.Controls.Add(this.ButtonNewDoc);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonSaveDoc);
            this.Controls.Add(this.buttonRemoveDoc);
            this.Controls.Add(this.buttonLoadDoc);
            this.Controls.Add(this.buttonLoadProj);
            this.Controls.Add(this.ProjectName);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.DocName);
            this.Controls.Add(this.richTextBox);
            this.Controls.Add(this.label1);
            this.Name = "DocMgr";
            this.Text = "DocMgr";
            this.Load += new System.EventHandler(this.DocMgr_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DocMgr_KeyPress);
            this.Resize += new System.EventHandler(this.DocMgr_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RichTextBox richTextBox;
        private Label DocName;
        private Button buttonClose;
        private Label ProjectName;
        private Label label1;
        private Button buttonLoadProj;
        private Button buttonLoadDoc;
        private Button buttonRemoveDoc;
        private Button buttonSaveDoc;
        private Label label2;
        private Button ButtonNewDoc;
        private Button ButtonNewProj;
        private Button ButtonSaveAs;
        private Button buttonNumberLines;
        private ToolTip toolTips;
        private Button buttonOpenFolder;
        private Button buttonBackUpFile;
        private Button buttonBackUpProject;
        private Button buttonArchiveFile;
        private Button buttonArchiveProject;
        private Button buttonProperties;
    }
}