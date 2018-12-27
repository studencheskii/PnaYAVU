using System;
using System.Xml;
using System.Xml.Serialization;
namespace CVS
{
    [Serializable]
    public class Label
    {
        public string TagName { get; set; }
        public string TagSize { get; set; }
        public string TagCreate { get; set; }
        public string TagModified { get; set; }
        public Label()
        {
            
        }
        public Label(string name = "", string size = "", string create = "", string mod = "")
        {  
            TagName = name;
            TagSize = size;
            TagCreate = create;
            TagModified = mod;
        }
    }
}
