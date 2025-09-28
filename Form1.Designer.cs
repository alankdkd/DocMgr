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
            components = new System.ComponentModel.Container();
            richTextBox = new RichTextBox();
            DocName = new Label();
            buttonClose = new Button();
            ProjectName = new Label();
            label1 = new Label();
            buttonLoadProj = new Button();
            buttonLoadDoc = new Button();
            buttonRemoveDoc = new Button();
            buttonSaveDoc = new Button();
            label2 = new Label();
            ButtonNewDoc = new Button();
            ButtonNewProj = new Button();
            ButtonSaveAs = new Button();
            buttonNumberLines = new Button();
            toolTips = new ToolTip(components);
            buttonBackUpFile = new Button();
            buttonBackUpProject = new Button();
            buttonProperties = new Button();
            buttonPrint = new Button();
            buttonArchiveFile = new Button();
            buttonArchiveProject = new Button();
            buttonFind = new Button();
            buttonOpenFolder = new Button();
            SuspendLayout();
            // 
            // richTextBox
            // 
            richTextBox.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            richTextBox.Location = new Point(143, 81);
            richTextBox.Name = "richTextBox";
            richTextBox.Size = new Size(922, 864);
            richTextBox.TabIndex = 0;
            richTextBox.Text = "";
            richTextBox.TextChanged += richTextBox_TextChanged;
            richTextBox.KeyPress += richTextBox1_KeyPress;
            // 
            // DocName
            // 
            DocName.AutoSize = true;
            DocName.Font = new Font("Calibri", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            DocName.ForeColor = SystemColors.ButtonHighlight;
            DocName.Location = new Point(143, 56);
            DocName.Name = "DocName";
            DocName.Size = new Size(0, 23);
            DocName.TabIndex = 2;
            DocName.MouseHover += DocName_MouseHover;
            // 
            // buttonClose
            // 
            buttonClose.BackColor = SystemColors.Control;
            buttonClose.DialogResult = DialogResult.Cancel;
            buttonClose.Font = new Font("Calibri", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            buttonClose.Location = new Point(1090, 16);
            buttonClose.Name = "buttonClose";
            buttonClose.Size = new Size(175, 31);
            buttonClose.TabIndex = 3;
            buttonClose.Text = "&Close";
            buttonClose.UseVisualStyleBackColor = false;
            buttonClose.Click += buttonClose_Click;
            // 
            // ProjectName
            // 
            ProjectName.AutoSize = true;
            ProjectName.Font = new Font("Calibri", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            ProjectName.ForeColor = SystemColors.ButtonHighlight;
            ProjectName.Location = new Point(971, 64);
            ProjectName.Name = "ProjectName";
            ProjectName.Size = new Size(0, 18);
            ProjectName.TabIndex = 4;
            ProjectName.MouseHover += ProjectName_MouseHover;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Calibri", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = SystemColors.ButtonHighlight;
            label1.Location = new Point(918, 64);
            label1.Name = "label1";
            label1.Size = new Size(59, 18);
            label1.TabIndex = 5;
            label1.Text = "Project: ";
            // 
            // buttonLoadProj
            // 
            buttonLoadProj.BackColor = SystemColors.Control;
            buttonLoadProj.Font = new Font("Calibri", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            buttonLoadProj.Location = new Point(142, 16);
            buttonLoadProj.Name = "buttonLoadProj";
            buttonLoadProj.Size = new Size(175, 31);
            buttonLoadProj.TabIndex = 6;
            buttonLoadProj.Text = "Load Pro&ject...";
            buttonLoadProj.UseVisualStyleBackColor = false;
            buttonLoadProj.Click += buttonLoadProj_Click;
            // 
            // buttonLoadDoc
            // 
            buttonLoadDoc.BackColor = SystemColors.Control;
            buttonLoadDoc.Enabled = false;
            buttonLoadDoc.Font = new Font("Calibri", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            buttonLoadDoc.Location = new Point(360, 16);
            buttonLoadDoc.Name = "buttonLoadDoc";
            buttonLoadDoc.Size = new Size(175, 31);
            buttonLoadDoc.TabIndex = 7;
            buttonLoadDoc.Text = "&Load Document...";
            toolTips.SetToolTip(buttonLoadDoc, "Display an existing document and add it to the project.");
            buttonLoadDoc.UseVisualStyleBackColor = false;
            buttonLoadDoc.Click += buttonLoadDoc_Click;
            // 
            // buttonRemoveDoc
            // 
            buttonRemoveDoc.BackColor = SystemColors.Control;
            buttonRemoveDoc.Enabled = false;
            buttonRemoveDoc.Font = new Font("Calibri", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            buttonRemoveDoc.Location = new Point(1090, 214);
            buttonRemoveDoc.Name = "buttonRemoveDoc";
            buttonRemoveDoc.Size = new Size(175, 31);
            buttonRemoveDoc.TabIndex = 8;
            buttonRemoveDoc.Text = "&Remove Document";
            toolTips.SetToolTip(buttonRemoveDoc, "Remove document from project but do NOT delete it.");
            buttonRemoveDoc.UseVisualStyleBackColor = false;
            buttonRemoveDoc.Click += buttonRemoveDoc_Click;
            // 
            // buttonSaveDoc
            // 
            buttonSaveDoc.BackColor = SystemColors.Control;
            buttonSaveDoc.Enabled = false;
            buttonSaveDoc.Font = new Font("Calibri", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            buttonSaveDoc.Location = new Point(578, 16);
            buttonSaveDoc.Name = "buttonSaveDoc";
            buttonSaveDoc.Size = new Size(175, 31);
            buttonSaveDoc.TabIndex = 9;
            buttonSaveDoc.Text = "&Save Document";
            toolTips.SetToolTip(buttonSaveDoc, "Save the currently displayed document.  Ctrl-S");
            buttonSaveDoc.UseVisualStyleBackColor = false;
            buttonSaveDoc.Click += buttonSaveDoc_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Calibri", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            label2.ForeColor = SystemColors.ButtonHighlight;
            label2.Location = new Point(9, 60);
            label2.Name = "label2";
            label2.Size = new Size(82, 18);
            label2.TabIndex = 10;
            label2.Text = "Documents:";
            // 
            // ButtonNewDoc
            // 
            ButtonNewDoc.BackColor = SystemColors.Control;
            ButtonNewDoc.Font = new Font("Calibri", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            ButtonNewDoc.Location = new Point(1090, 81);
            ButtonNewDoc.Name = "ButtonNewDoc";
            ButtonNewDoc.Size = new Size(175, 31);
            ButtonNewDoc.TabIndex = 11;
            ButtonNewDoc.Text = "&New Document...";
            toolTips.SetToolTip(ButtonNewDoc, "Add new blank document to the project.");
            ButtonNewDoc.UseVisualStyleBackColor = false;
            ButtonNewDoc.Click += ButtonNewDoc_Click;
            // 
            // ButtonNewProj
            // 
            ButtonNewProj.BackColor = SystemColors.Control;
            ButtonNewProj.Font = new Font("Calibri", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            ButtonNewProj.Location = new Point(1090, 146);
            ButtonNewProj.Name = "ButtonNewProj";
            ButtonNewProj.Size = new Size(175, 31);
            ButtonNewProj.TabIndex = 12;
            ButtonNewProj.Text = "New Pro&ject...";
            ButtonNewProj.UseVisualStyleBackColor = false;
            ButtonNewProj.Click += ButtonNewProj_Click;
            // 
            // ButtonSaveAs
            // 
            ButtonSaveAs.BackColor = SystemColors.Control;
            ButtonSaveAs.Font = new Font("Calibri", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            ButtonSaveAs.Location = new Point(796, 16);
            ButtonSaveAs.Name = "ButtonSaveAs";
            ButtonSaveAs.Size = new Size(175, 31);
            ButtonSaveAs.TabIndex = 13;
            ButtonSaveAs.Text = "Save &As...";
            toolTips.SetToolTip(ButtonSaveAs, "Save a copy of the current document to a file without adding it to the project.");
            ButtonSaveAs.UseVisualStyleBackColor = false;
            ButtonSaveAs.Click += ButtonSaveAs_Click;
            // 
            // buttonNumberLines
            // 
            buttonNumberLines.BackColor = SystemColors.Control;
            buttonNumberLines.Font = new Font("Calibri", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            buttonNumberLines.Location = new Point(1090, 353);
            buttonNumberLines.Name = "buttonNumberLines";
            buttonNumberLines.Size = new Size(175, 31);
            buttonNumberLines.TabIndex = 14;
            buttonNumberLines.Text = "N&umber Lines";
            toolTips.SetToolTip(buttonNumberLines, "Add auto-numbering to the selected text.");
            buttonNumberLines.UseVisualStyleBackColor = false;
            buttonNumberLines.Click += buttonNumberLines_Click;
            // 
            // buttonBackUpFile
            // 
            buttonBackUpFile.BackColor = SystemColors.Control;
            buttonBackUpFile.Font = new Font("Calibri", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            buttonBackUpFile.Location = new Point(1090, 422);
            buttonBackUpFile.Name = "buttonBackUpFile";
            buttonBackUpFile.Size = new Size(175, 31);
            buttonBackUpFile.TabIndex = 16;
            buttonBackUpFile.Text = "&Back Up Document";
            toolTips.SetToolTip(buttonBackUpFile, "Update the single backup copy with incremental progress until it's ready to archive.");
            buttonBackUpFile.UseVisualStyleBackColor = false;
            buttonBackUpFile.Click += buttonBackUpFile_Click;
            // 
            // buttonBackUpProject
            // 
            buttonBackUpProject.BackColor = SystemColors.Control;
            buttonBackUpProject.Font = new Font("Calibri", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            buttonBackUpProject.Location = new Point(1090, 465);
            buttonBackUpProject.Name = "buttonBackUpProject";
            buttonBackUpProject.Size = new Size(175, 31);
            buttonBackUpProject.TabIndex = 17;
            buttonBackUpProject.Text = "Bac&k Up Project";
            toolTips.SetToolTip(buttonBackUpProject, "Update the single backup copy with incremental progress until it's ready to archive.");
            buttonBackUpProject.UseVisualStyleBackColor = false;
            buttonBackUpProject.Click += buttonBackUpProject_Click;
            // 
            // buttonProperties
            // 
            buttonProperties.BackColor = SystemColors.Control;
            buttonProperties.Font = new Font("Calibri", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            buttonProperties.Location = new Point(1090, 687);
            buttonProperties.Name = "buttonProperties";
            buttonProperties.Size = new Size(175, 31);
            buttonProperties.TabIndex = 20;
            buttonProperties.Text = "Prop&erties...";
            toolTips.SetToolTip(buttonProperties, "Set the Backup/Archive folder, margins for printing, or default font.");
            buttonProperties.UseVisualStyleBackColor = false;
            buttonProperties.Click += buttonProperties_Click;
            // 
            // buttonPrint
            // 
            buttonPrint.BackColor = SystemColors.Control;
            buttonPrint.Font = new Font("Calibri", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            buttonPrint.Location = new Point(1090, 619);
            buttonPrint.Name = "buttonPrint";
            buttonPrint.Size = new Size(175, 31);
            buttonPrint.TabIndex = 21;
            buttonPrint.Text = "&Print...";
            toolTips.SetToolTip(buttonPrint, "Print document or selection.  Ctrl-P");
            buttonPrint.UseVisualStyleBackColor = false;
            buttonPrint.Click += buttonPrint_Click;
            // 
            // buttonArchiveFile
            // 
            buttonArchiveFile.BackColor = SystemColors.Control;
            buttonArchiveFile.Font = new Font("Calibri", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            buttonArchiveFile.Location = new Point(1090, 508);
            buttonArchiveFile.Name = "buttonArchiveFile";
            buttonArchiveFile.Size = new Size(175, 31);
            buttonArchiveFile.TabIndex = 18;
            buttonArchiveFile.Text = "Arc&hive Document";
            toolTips.SetToolTip(buttonArchiveFile, "Save a time-stamped permanent copy.");
            buttonArchiveFile.UseVisualStyleBackColor = false;
            buttonArchiveFile.Click += buttonArchiveFile_Click;
            // 
            // buttonArchiveProject
            // 
            buttonArchiveProject.BackColor = SystemColors.Control;
            buttonArchiveProject.Font = new Font("Calibri", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            buttonArchiveProject.Location = new Point(1090, 551);
            buttonArchiveProject.Name = "buttonArchiveProject";
            buttonArchiveProject.Size = new Size(175, 31);
            buttonArchiveProject.TabIndex = 19;
            buttonArchiveProject.Text = "Archi&ve Project";
            toolTips.SetToolTip(buttonArchiveProject, "Save a time-stamped permanent copy.");
            buttonArchiveProject.UseVisualStyleBackColor = false;
            buttonArchiveProject.Click += buttonArchiveProject_Click;
            // 
            // buttonFind
            // 
            buttonFind.BackColor = SystemColors.Control;
            buttonFind.Font = new Font("Calibri", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            buttonFind.Location = new Point(1090, 755);
            buttonFind.Name = "buttonFind";
            buttonFind.Size = new Size(175, 31);
            buttonFind.TabIndex = 22;
            buttonFind.Text = "&Find...";
            toolTips.SetToolTip(buttonFind, "Find a string in the current document, project, or all projects.  Ctrl-F");
            buttonFind.UseVisualStyleBackColor = false;
            buttonFind.Click += buttonFind_Click;
            // 
            // buttonOpenFolder
            // 
            buttonOpenFolder.BackColor = SystemColors.Control;
            buttonOpenFolder.Font = new Font("Calibri", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            buttonOpenFolder.Location = new Point(1090, 284);
            buttonOpenFolder.Name = "buttonOpenFolder";
            buttonOpenFolder.Size = new Size(175, 31);
            buttonOpenFolder.TabIndex = 15;
            buttonOpenFolder.Text = "&Open Folder...";
            toolTips.SetToolTip(buttonOpenFolder, "Open browser on folder containing the document.  Ctrl-O");
            buttonOpenFolder.UseVisualStyleBackColor = false;
            buttonOpenFolder.Click += buttonOpenFolder_Click;
            // 
            // DocMgr
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDarkDark;
            ClientSize = new Size(1296, 985);
            Controls.Add(buttonFind);
            Controls.Add(buttonPrint);
            Controls.Add(buttonProperties);
            Controls.Add(buttonArchiveProject);
            Controls.Add(buttonArchiveFile);
            Controls.Add(buttonBackUpProject);
            Controls.Add(buttonBackUpFile);
            Controls.Add(buttonOpenFolder);
            Controls.Add(buttonNumberLines);
            Controls.Add(ButtonSaveAs);
            Controls.Add(ButtonNewProj);
            Controls.Add(ButtonNewDoc);
            Controls.Add(label2);
            Controls.Add(buttonSaveDoc);
            Controls.Add(buttonRemoveDoc);
            Controls.Add(buttonLoadDoc);
            Controls.Add(buttonLoadProj);
            Controls.Add(ProjectName);
            Controls.Add(buttonClose);
            Controls.Add(DocName);
            Controls.Add(richTextBox);
            Controls.Add(label1);
            Name = "DocMgr";
            Text = "DocMgr";
            WindowState = FormWindowState.Maximized;
            Load += DocMgr_Load;
            KeyDown += DocMgr_KeyDown;
            KeyPress += DocMgr_KeyPress;
            Resize += DocMgr_Resize;
            ResumeLayout(false);
            PerformLayout();
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
        private Button buttonPrint;
        private Button buttonFind;
    }
}