using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
namespace CVS { 

    class Program 
    { 
        private static void SaveCVS(MyCVS CVS)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(MyCVS));

            using (FileStream fs = new FileStream("cvs.xml", FileMode.Create))
            {
                formatter.Serialize(fs, CVS);
            }
        }

        private static MyCVS LoadCVS()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(MyCVS));

            if (File.Exists("cvs.xml"))
            {
                using (FileStream fs = new FileStream("cvs.xml", FileMode.Open))
                {
                    return new MyCVS((MyCVS)formatter.Deserialize(fs));
                }
            }
            return new MyCVS();

        }

        public static void Main() 
        { 
            MyCVS Play = LoadCVS(); 
            Play.Run();
            SaveCVS(Play);
            Environment.Exit(0); 
        } 


    } 
}
