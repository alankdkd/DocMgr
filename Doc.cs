using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocMgr
{
    internal class Doc
    {
        public Doc(string? docName, string? path = null)
        {
            DocName = docName;
            Path = path;
            SubDocs = new List<Doc>();
        }

        public Doc()
        {
            DocName = null;
            Path = null;
            SubDocs = new List<Doc>();
        }

        public string? DocName { get; set; }
        public string? Path { get; set; }
        public List<Doc> SubDocs { get; set; }
    }
}
