using System;
using System.Security.Policy;
using System.Xml;
using System.Xml.Serialization;
namespace CVS
{
    [Serializable]
    public class MyFile
    {
        public string FileType { get; set; }
        public string FileName { get; set; }
        public string ExFile { get; set; }
        public string Size { get; set; }
        public string CreatingDate { get; set; }
        public string ModifiedDate { get; set; }

        public MyFile()
        {
            
        }

        public MyFile(string type, string name, string ext, string s, string cdate, string mdate)
        {
            FileType = type;
            FileName = name;
            ExFile = ext;
            Size = s;
            CreatingDate = cdate;
            ModifiedDate = mdate;
        }
    }
}
