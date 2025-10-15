namespace DocMgr
{
    partial class FindForm
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
            textString = new TextBox();
            checkMatchCase = new CheckBox();
            checkMatchWholeWord = new CheckBox();
            radioCurrentDoc = new RadioButton();
            radioCurrentProject = new RadioButton();
            radioAllProjects = new RadioButton();
            groupBox1 = new GroupBox();
            buttonFind = new Button();
            buttonClose = new Button();
            buttonNext = new Button();
            buttonPrevious = new Button();
            labelInstanceOrder = new Label();
            labelFindResults = new Label();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(22, 24);
            label1.Name = "label1";
            label1.Size = new Size(56, 25);
            label1.TabIndex = 0;
            label1.Text = "Find:";
            // 
            // textString
            // 
            textString.Location = new Point(75, 21);
            textString.Name = "textString";
            textString.Size = new Size(549, 33);
            textString.TabIndex = 1;
            // 
            // checkMatchCase
            // 
            checkMatchCase.AutoSize = true;
            checkMatchCase.Location = new Point(22, 75);
            checkMatchCase.Name = "checkMatchCase";
            checkMatchCase.Size = new Size(131, 29);
            checkMatchCase.TabIndex = 2;
            checkMatchCase.Text = "Match Case";
            checkMatchCase.UseVisualStyleBackColor = true;
            // 
            // checkMatchWholeWord
            // 
            checkMatchWholeWord.AutoSize = true;
            checkMatchWholeWord.Location = new Point(171, 76);
            checkMatchWholeWord.Name = "checkMatchWholeWord";
            checkMatchWholeWord.Size = new Size(203, 29);
            checkMatchWholeWord.TabIndex = 3;
            checkMatchWholeWord.Text = "Match Whole Word";
            checkMatchWholeWord.UseVisualStyleBackColor = true;
            // 
            // radioCurrentDoc
            // 
            radioCurrentDoc.AutoSize = true;
            radioCurrentDoc.Checked = true;
            radioCurrentDoc.Location = new Point(49, 42);
            radioCurrentDoc.Name = "radioCurrentDoc";
            radioCurrentDoc.Size = new Size(197, 29);
            radioCurrentDoc.TabIndex = 4;
            radioCurrentDoc.TabStop = true;
            radioCurrentDoc.Text = "Current Document";
            radioCurrentDoc.UseVisualStyleBackColor = true;
            // 
            // radioCurrentProject
            // 
            radioCurrentProject.AutoSize = true;
            radioCurrentProject.Location = new Point(49, 77);
            radioCurrentProject.Name = "radioCurrentProject";
            radioCurrentProject.Size = new Size(167, 29);
            radioCurrentProject.TabIndex = 5;
            radioCurrentProject.TabStop = true;
            radioCurrentProject.Text = "Current Project";
            radioCurrentProject.UseVisualStyleBackColor = true;
            // 
            // radioAllProjects
            // 
            radioAllProjects.AutoSize = true;
            radioAllProjects.Location = new Point(49, 112);
            radioAllProjects.Name = "radioAllProjects";
            radioAllProjects.Size = new Size(129, 29);
            radioAllProjects.TabIndex = 6;
            radioAllProjects.TabStop = true;
            radioAllProjects.Text = "All Projects";
            radioAllProjects.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(radioCurrentDoc);
            groupBox1.Controls.Add(radioCurrentProject);
            groupBox1.Controls.Add(radioAllProjects);
            groupBox1.Location = new Point(340, 133);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(284, 148);
            groupBox1.TabIndex = 7;
            groupBox1.TabStop = false;
            groupBox1.Text = "Scope";
            // 
            // buttonFind
            // 
            buttonFind.Location = new Point(650, 19);
            buttonFind.Name = "buttonFind";
            buttonFind.Size = new Size(135, 65);
            buttonFind.TabIndex = 8;
            buttonFind.Text = "&Find";
            buttonFind.UseVisualStyleBackColor = true;
            buttonFind.Click += buttonFind_Click;
            // 
            // buttonClose
            // 
            buttonClose.DialogResult = DialogResult.Cancel;
            buttonClose.Location = new Point(650, 145);
            buttonClose.Name = "buttonClose";
            buttonClose.Size = new Size(135, 65);
            buttonClose.TabIndex = 9;
            buttonClose.Text = "&Close";
            buttonClose.UseVisualStyleBackColor = true;
            buttonClose.Click += buttonClose_Click;
            // 
            // buttonNext
            // 
            buttonNext.Enabled = false;
            buttonNext.Location = new Point(26, 395);
            buttonNext.Name = "buttonNext";
            buttonNext.Size = new Size(135, 65);
            buttonNext.TabIndex = 10;
            buttonNext.Text = "&Next";
            buttonNext.UseVisualStyleBackColor = true;
            buttonNext.Click += buttonNext_Click;
            // 
            // buttonPrevious
            // 
            buttonPrevious.Enabled = false;
            buttonPrevious.Location = new Point(190, 395);
            buttonPrevious.Name = "buttonPrevious";
            buttonPrevious.Size = new Size(135, 65);
            buttonPrevious.TabIndex = 11;
            buttonPrevious.Text = "&Previous";
            buttonPrevious.UseVisualStyleBackColor = true;
            buttonPrevious.Click += buttonPrevious_Click;
            // 
            // labelInstanceOrder
            // 
            labelInstanceOrder.AutoSize = true;
            labelInstanceOrder.Location = new Point(22, 340);
            labelInstanceOrder.Name = "labelInstanceOrder";
            labelInstanceOrder.Size = new Size(172, 25);
            labelInstanceOrder.TabIndex = 12;
            labelInstanceOrder.Text = "Showing X of XXX";
            // 
            // labelFindResults
            // 
            labelFindResults.AutoSize = true;
            labelFindResults.Location = new Point(22, 304);
            labelFindResults.Name = "labelFindResults";
            labelFindResults.Size = new Size(434, 25);
            labelFindResults.TabIndex = 13;
            labelFindResults.Text = "Found X instances in x documents in x projects.";
            // 
            // FindForm
            // 
            AcceptButton = buttonFind;
            AutoScaleDimensions = new SizeF(11F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = buttonClose;
            ClientSize = new Size(814, 478);
            Controls.Add(labelFindResults);
            Controls.Add(labelInstanceOrder);
            Controls.Add(buttonPrevious);
            Controls.Add(buttonNext);
            Controls.Add(buttonClose);
            Controls.Add(buttonFind);
            Controls.Add(checkMatchWholeWord);
            Controls.Add(checkMatchCase);
            Controls.Add(textString);
            Controls.Add(label1);
            Controls.Add(groupBox1);
            Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            Margin = new Padding(5);
            Name = "FindForm";
            Text = "Find";
            TopMost = true;
            Shown += FindForm_Shown;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox textString;
        private CheckBox checkMatchCase;
        private CheckBox checkMatchWholeWord;
        private RadioButton radioCurrentDoc;
        private RadioButton radioCurrentProject;
        private RadioButton radioAllProjects;
        private GroupBox groupBox1;
        private Button buttonFind;
        private Button buttonClose;
        private Button buttonNext;
        private Button buttonPrevious;
        private Label labelInstanceOrder;
        private Label labelFindResults;
    }
}