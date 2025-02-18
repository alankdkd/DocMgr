using Microsoft.Win32;

namespace DocMgr
{
    public partial class LoadProjectDlg : Form
    {
        private Dictionary<string, string> projMap = new Dictionary<string, string>();
        public string selectedPath = "";

        public LoadProjectDlg()
        {
            InitializeComponent();

            ReadPreviousProjects();
            AddProjectsToListBox();
            listBox1.Enabled = projMap.Count > 0;
            DialogResult = DialogResult.Cancel;
        }

        private void AddProjectsToListBox()
        {
            foreach (string projName in projMap.Keys)
                listBox1.Items.Add(projName);
        }

        private void ReadPreviousProjects()
        {
            int numProjects = GetNumProjects();

            for (int i = 0; i < numProjects; i++)
                ReadNextProject(i);
        }

        private void ReadNextProject(int i)
        {
            RegistryKey? key;
            string projIndex = "Project" + i;
            key = Registry.CurrentUser.OpenSubKey(@"Software\PatternScope Systems\DocMgr\"
                + projIndex, RegistryKeyPermissionCheck.ReadWriteSubTree);

            if (key == null)
                return;

            object? obj = key.GetValue("Name");

            if (obj == null || !(obj is string projName))
                return;

            obj = key.GetValue("Path");

            if (obj == null || !(obj is string projPath))
                return;

            projMap[projName] = projPath;
        }

        private static int GetNumProjects()
        {
            RegistryKey? key = Registry.CurrentUser.OpenSubKey(
                @"Software\PatternScope Systems\DocMgr",
                RegistryKeyPermissionCheck.ReadWriteSubTree);

            if (key == null)
                return 0;           // Taint none!

            object? obj = key.GetValue("NumProjects");

            if (obj == null)
                return 0;           // Taint none!

            return (int)obj;
        }
        public static void AddProject(string name, string path)
        {
            if (ProjectAlreadyThere(name))
                return;

            int numProjects = GetNumProjects();
            string projIndex = "Project" + numProjects++;

            RegistryKey? key = Registry.CurrentUser.CreateSubKey(
                @"Software\PatternScope Systems\DocMgr\" + projIndex,
                RegistryKeyPermissionCheck.ReadWriteSubTree);

            if (key != null)
            {
                key.SetValue("Name", name);
                key.SetValue("Path", path);
                key = Registry.CurrentUser.OpenSubKey(
                   @"Software\PatternScope Systems\DocMgr",
                   RegistryKeyPermissionCheck.ReadWriteSubTree);
                key.SetValue("NumProjects", numProjects);
            }
        }

        private static bool ProjectAlreadyThere(string name)
        {
            int numProjects = GetNumProjects();

            for (int i = 0; i < numProjects; ++i)
                if (GetProjectName(i) == name)
                    return true;                // Project already there.

            return false;
        }

        private static string GetProjectName(int i)
        {
            string projIndex = "Project" + i;

            RegistryKey? key = Registry.CurrentUser.OpenSubKey(
                @"Software\PatternScope Systems\DocMgr\" + projIndex,
                RegistryKeyPermissionCheck.ReadWriteSubTree);

            if (key == null)
                return "";

            object obj = key.GetValue("Name");

            if (obj == null || !(obj is string))
                return null;

            //ORIG: return key.GetValue("Name").ToString();
            return obj.ToString();
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            listBox1_Click(sender, e);
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            string? projectPath = DocMgr.SelectFile("json files (*.json)|*.json|All files (*.*)|*.*");
            RegistryKey? key;

            if (projectPath != null)
            {                                               // Project file selected:
                selectedPath = projectPath.ToString();
                key = Registry.CurrentUser.CreateSubKey(
                    @"Software\PatternScope Systems\DocMgr",
                    RegistryKeyPermissionCheck.ReadWriteSubTree);
                key.SetValue("LastProjectPath", projectPath.ToString());
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
                return;

            string projName = listBox1.SelectedItem.ToString();
            selectedPath = projMap[projName];
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
