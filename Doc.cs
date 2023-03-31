using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Microsoft.Win32;
using System.IO;

namespace DocMgr
{
    public class Doc
    {
        public Doc(string? docName, string? path = null)
        {
            DocName = docName;
            DocPath = path;
            SubDocs = new List<Doc>();
        }

        public Doc()
        {
            DocName = null;
            DocPath = null;
            SubDocs = new List<Doc>();
        }

        public void AddDoc(string? docPath, string? name = "")
        {
            if (name.Length == 0)
                name = Path.GetFileNameWithoutExtension(docPath);

            SubDocs.Add(new Doc(name, docPath));
        }
           
        public string? DocName { get; set; }
        public string? DocPath { get; set; }
        public List<Doc> SubDocs { get; set; }
    }
}
