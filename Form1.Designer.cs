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
            this.DocName.Location = new System.Drawing.Point(143, 55);
            this.DocName.Name = "DocName";
            this.DocName.Size = new System.Drawing.Size(0, 23);
            this.DocName.TabIndex = 2;
            // 
            // buttonClose
            // 
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.buttonClose.Location = new System.Drawing.Point(1114, 14);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(76, 31);
            this.buttonClose.TabIndex = 3;
            this.buttonClose.Text = "&Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // ProjectName
            // 
            this.ProjectName.AutoSize = true;
            this.ProjectName.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.ProjectName.Location = new System.Drawing.Point(971, 65);
            this.ProjectName.Name = "ProjectName";
            this.ProjectName.Size = new System.Drawing.Size(0, 15);
            this.ProjectName.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(918, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "Project: ";
            // 
            // buttonLoadProj
            // 
            this.buttonLoadProj.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.buttonLoadProj.Location = new System.Drawing.Point(142, 16);
            this.buttonLoadProj.Name = "buttonLoadProj";
            this.buttonLoadProj.Size = new System.Drawing.Size(183, 31);
            this.buttonLoadProj.TabIndex = 6;
            this.buttonLoadProj.Text = "Load &Project...";
            this.buttonLoadProj.UseVisualStyleBackColor = true;
            this.buttonLoadProj.Click += new System.EventHandler(this.buttonLoadProj_Click);
            // 
            // buttonLoadDoc
            // 
            this.buttonLoadDoc.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.buttonLoadDoc.Location = new System.Drawing.Point(360, 16);
            this.buttonLoadDoc.Name = "buttonLoadDoc";
            this.buttonLoadDoc.Size = new System.Drawing.Size(183, 31);
            this.buttonLoadDoc.TabIndex = 7;
            this.buttonLoadDoc.Text = "&Load Document";
            this.buttonLoadDoc.UseVisualStyleBackColor = true;
            this.buttonLoadDoc.Click += new System.EventHandler(this.buttonLoadDoc_Click);
            // 
            // buttonRemoveDoc
            // 
            this.buttonRemoveDoc.Enabled = false;
            this.buttonRemoveDoc.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.buttonRemoveDoc.Location = new System.Drawing.Point(796, 16);
            this.buttonRemoveDoc.Name = "buttonRemoveDoc";
            this.buttonRemoveDoc.Size = new System.Drawing.Size(183, 31);
            this.buttonRemoveDoc.TabIndex = 8;
            this.buttonRemoveDoc.Text = "&Remove Document";
            this.buttonRemoveDoc.UseVisualStyleBackColor = true;
            this.buttonRemoveDoc.Click += new System.EventHandler(this.buttonRemoveDoc_Click);
            // 
            // buttonSaveDoc
            // 
            this.buttonSaveDoc.Enabled = false;
            this.buttonSaveDoc.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.buttonSaveDoc.Location = new System.Drawing.Point(578, 16);
            this.buttonSaveDoc.Name = "buttonSaveDoc";
            this.buttonSaveDoc.Size = new System.Drawing.Size(183, 31);
            this.buttonSaveDoc.TabIndex = 9;
            this.buttonSaveDoc.Text = "&Save Document";
            this.buttonSaveDoc.UseVisualStyleBackColor = true;
            this.buttonSaveDoc.Click += new System.EventHandler(this.buttonSaveDoc_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(9, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 15);
            this.label2.TabIndex = 10;
            this.label2.Text = "Documents:";
            // 
            // DocMgr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1202, 985);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonSaveDoc);
            this.Controls.Add(this.buttonRemoveDoc);
            this.Controls.Add(this.buttonLoadDoc);
            this.Controls.Add(this.buttonLoadProj);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ProjectName);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.DocName);
            this.Controls.Add(this.richTextBox);
            this.Name = "DocMgr";
            this.Text = "DocMgr";
            this.Load += new System.EventHandler(this.DocMgr_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DocMgr_KeyPress);
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
    }
}