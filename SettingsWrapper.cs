using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace DocMgr
{
    //public class SettingsWrapper
    //{
    //    [Category("Paths")]
    //    [Description("Specify the file or folder path.")]
    //    [Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
    //    public string BackupsAndArchivesFolder
    //    {
    //        get => Properties.Settings.Default.BackupsAndArchivesFolder; // Access the setting
    //        set
    //        {
    //            Properties.Settings.Default.BackupsAndArchivesFolder = value; // Update the setting
    //            Properties.Settings.Default.Save();        // Persist the change
    //        }
    //    }

    //    [Category("General Settings")]
    //    [Description("The defaut font.")]
    //    public Font DefaultFont { get; set; }

    //}

    public class FolderPathEditor : UITypeEditor
    {
        // Indicate that this editor provides a modal dialog.
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        // Show the FolderBrowserDialog when the user clicks the ellipsis button.
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (provider != null)
            {
                // Get the IWindowsFormsEditorService to display dialogs.
                IWindowsFormsEditorService editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                if (editorService != null)
                {
                    using (FolderBrowserDialog dialog = new FolderBrowserDialog())
                    {
                        dialog.Description = "Select a Folder"; // Dialog title/description.
                        dialog.ShowNewFolderButton = true; // Allow the user to create new folders.

                        // Set the initial selected path (if any).
                        if (value is string folderPath)
                        {
                            dialog.SelectedPath = folderPath;
                        }

                        // Show the dialog and return the selected folder path if OK was pressed.
                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            return dialog.SelectedPath;
                        }
                    }
                }
            }

            // Return the original value if no folder is selected.
            return value;
        }
    }
}
