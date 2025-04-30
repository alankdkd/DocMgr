namespace DocMgr
{
    partial class PropertiesForm
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
            propertyGrid = new PropertyGrid();
            buttonSave = new Button();
            buttonCancel = new Button();
            SuspendLayout();
            // 
            // propertyGrid
            // 
            propertyGrid.Location = new Point(23, 22);
            propertyGrid.Name = "propertyGrid";
            propertyGrid.Size = new Size(754, 390);
            propertyGrid.TabIndex = 0;
            // 
            // buttonSave
            // 
            buttonSave.Font = new Font("Calibri", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            buttonSave.Location = new Point(804, 49);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(123, 43);
            buttonSave.TabIndex = 1;
            buttonSave.Text = "&Save";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += buttonSave_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Font = new Font("Calibri", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            buttonCancel.Location = new Point(804, 129);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(123, 43);
            buttonCancel.TabIndex = 2;
            buttonCancel.Text = "&Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // PropertiesForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(958, 441);
            Controls.Add(buttonCancel);
            Controls.Add(buttonSave);
            Controls.Add(propertyGrid);
            Name = "PropertiesForm";
            Text = "DocMgr Properties";
            ResumeLayout(false);
        }

        #endregion

        private PropertyGrid propertyGrid;
        private Button buttonSave;
        private Button buttonCancel;
    }
}