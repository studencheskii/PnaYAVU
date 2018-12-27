using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
namespace CVS
{
    [Serializable]
    public class MyDirectory
    {
        public string Path;
        public List<MyFile> ListFiles = new List<MyFile>();
        public List<Label> ListLabels = new List<Label>();

        public MyDirectory()
        {

        }

        public MyDirectory(string p)
        {
            Path = p;
            InitFilesList();
        }

        public void InitFilesList()
        {
            FileInfo file = null;
            foreach (var fi in Directory.GetFiles(Path))
            {
                file = new FileInfo(fi);
                AddInListFiles(file);
                AddInLabel();
            }
            DirectoryInfo dir = null;
            foreach (var di in Directory.GetDirectories(Path))
            {
                dir = new DirectoryInfo(di);
                AddInListFiles(dir);
                AddInLabel();
            }
        }

        public void AddInLabel()
        {
            Label label = new Label(String.Empty,String.Empty, String.Empty, String.Empty);
            ListLabels.Add(label);
        }
        public void AddInListFiles(FileInfo file)
        {
            string NameWithExt = System.IO.Path.GetFileNameWithoutExtension(file.Name);

            string size = ComputeSize(file.Length);

            MyFile f = new MyFile("File",NameWithExt, file.Extension, size,
                                file.CreationTime.ToString(), file.LastWriteTime.ToString());
            
            ListFiles.Add(f);
        }
        public void AddInListFiles(DirectoryInfo dir)
        {
            string size = ComputeSize(GetDirSize(dir.FullName));
            MyFile f = new MyFile("Catalog", dir.Name, "", size, dir.CreationTime.ToString(), dir.LastWriteTime.ToString());
            ListFiles.Add(f);
        }
        //public void SetPath(string p) { Path = p; }
        public string GetPath() => this.Path;


        public int GetDirSize(string path)
        {
            int size = 0;
            string[] files = Directory.GetFiles(path);
            foreach (var file in files)
                size += file.Length;
            string[] dirs = Directory.GetDirectories(path);
            foreach (string dir in dirs)
                
                size += GetDirSize(dir);
            return size;
        }

        public string ComputeSize(double bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            return String.Format("{0:0.##} {1}", len, sizes[order]);
        }
    }
}
