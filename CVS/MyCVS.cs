using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Reflection.Emit;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics.PerformanceData;

namespace CVS
{
    [Serializable]
    public class MyCVS
    {
        public List<MyDirectory> Dirs = new List<MyDirectory>();
        MyDirectory ActiveDirectory = null;

        public MyCVS()
        {

        }

        public MyCVS(MyCVS CVS)
        {
            if (CVS.Dirs.Count > 0)
            {
                foreach (MyDirectory di in CVS.Dirs)
                {
                    this.Dirs.Add(di);
                }
                ActiveDirectory = this.Dirs[this.Dirs.Count - 1];
            }
            else
            {
                ActiveDirectory = null;
            }
        }

        public void InitCVS(string dir_path)
        {
            Console.WriteLine(dir_path);
            if (Directory.Exists(dir_path))
            {
                ActiveDirectory = new MyDirectory(dir_path);
                Dirs.Add(ActiveDirectory);
            }
            else
            {
                Console.WriteLine("Директории не существует");
            }
        }
        private void GetStatus()
        {
            if (Dirs.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                MyDirectory d = this.ActiveDirectory;
                int i = 0;
                Label label;
                Console.WriteLine($"Directory: {d.GetPath()}");
                SetLabelNew();
                int count = d.ListLabels.Count;
                while(i < count)
                {
                    label = d.ListLabels[i];
                    string s = d.GetPath() + d.ListFiles[i].FileName + d.ListFiles[i].ExFile;
                    SetLabel(s, label, d.ListFiles[i]);
                    count = d.ListLabels.Count;
                    if (i < count)
                        PrintOneFile(d.ListFiles[i], label);
                    i++;
                    
                }
            }
            else
            {
                Console.WriteLine("Отсутствуют инициализированные каталоги");
            }

        }

        private void PrintOneFile(MyFile file, Label label)
        {
            ColorText(label);
            Console.WriteLine(file.FileType + ":");
            Console.WriteLine($"    {file.FileName + file.ExFile + label.TagName}");
            Console.WriteLine($"    size: {file.Size + label.TagSize}");
            Console.WriteLine($"    created: {file.CreatingDate + label.TagCreate}");
            Console.WriteLine($"    modified: {file.ModifiedDate + label.TagModified}");
        }

        private void SetLabel(string path, Label label, MyFile f)   
        {


            if (!File.Exists(path) && !Directory.Exists(path))
            {
                if (label.TagName == " <<--new" || label.TagName == " <<--removed")
                {
                    ActiveDirectory.ListFiles.Remove(f);
                    ActiveDirectory.ListLabels.Remove(label);
                }
                else
                {
                    label.TagName = " <<--deleted";
                }
            }
            else if (File.Exists(path))
            {
                FileInfo file = new FileInfo(path);
                if (f.CreatingDate != file.CreationTime.ToString())
                {
                    label.TagCreate = $" <<--{file.CreationTime.ToString()}";
                    label.TagModified = $" <<--{file.LastWriteTime.ToString()}";
                }
                else if (f.ModifiedDate != file.LastWriteTime.ToString())
                {
                    label.TagModified = $" <<--{file.LastWriteTime.ToString()}";
                }
                if (f.Size != ActiveDirectory.ComputeSize(file.Length))
                {
                    label.TagSize = $" <<--{ActiveDirectory.ComputeSize(file.Length)}";
                }

            }
            else if (Directory.Exists(path))
            {
                DirectoryInfo di = new DirectoryInfo(path);
                int size = ActiveDirectory.GetDirSize(path);
                if (f.CreatingDate != di.CreationTime.ToString())
                {
                    label.TagCreate = $" <<--{di.CreationTime.ToString()}";
                    label.TagModified = $" <<--{di.LastWriteTime.ToString()}";
                }
                else if (f.ModifiedDate != di.LastWriteTime.ToString())
                {
                    label.TagModified = $" <<--{di.LastWriteTime.ToString()}";
                }
                if (f.Size != ActiveDirectory.ComputeSize(size))
                {
                    label.TagSize = $" <<--{ActiveDirectory.ComputeSize(size)}";
                }

            }
        }

        private void ColorText(Label label)
        {
            if (label.TagName.Contains("deleted") || label.TagName.Contains("removed") || !(label.TagSize == "") || !(label.TagSize == ""))
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
        }

        private void SetLabelNew()
        {
            string dir_path = ActiveDirectory?.GetPath();
            int i = ActiveDirectory.ListFiles.Count;
            if ((dir_path != null))
            {
                DirectoryInfo DI = new DirectoryInfo(dir_path);

                foreach(FileInfo file in DI.GetFiles())
                {
                    int j = 0;
                    bool notHave = true;
                    foreach (MyFile myfile in ActiveDirectory.ListFiles)
                    {
                        string filename = myfile.FileName + myfile.ExFile;
                        if (filename == file.Name)
                        {
                            notHave = false;
                            if (ActiveDirectory.ListLabels[j].TagName.Contains(" <<--delete"))
                            {
                                ActiveDirectory.ListLabels[j].TagName = $" <<--new";
                            }
                        }
                        j++;
                    }
                    if (notHave)
                    {
                        ActiveDirectory.AddInListFiles(file);
                        ActiveDirectory.AddInLabel();
                        ActiveDirectory.ListLabels[i++].TagName = $" <<--new";
                    }
                }
                foreach(DirectoryInfo dir in DI.GetDirectories())
                {

                    int j = 0;
                    bool notHave = true;
                    foreach(MyFile myfile in ActiveDirectory.ListFiles)
                    {
                        string dirname = myfile.FileName + myfile.ExFile;
                        if(dirname == dir.Name)
                        {
                            notHave = false;
                            if (ActiveDirectory.ListLabels[j].TagName.Contains(" <<--delete"))
                            {
                                ActiveDirectory.ListLabels[j].TagName = $" <<--new";
                            }
                        }
                        j++;
                    }

                    if (notHave)
                    {
                        ActiveDirectory.AddInListFiles(dir);
                        ActiveDirectory.AddInLabel();
                        ActiveDirectory.ListLabels[i++].TagName = $" <<--new";
                    }
                }
            }
        }

        private void Add(string file_path)
        {
            string dir_path = ActiveDirectory?.GetPath();
            bool dir_p = dir_path != null;
            if (!dir_p)
            {
                Console.WriteLine("Ни одна директория не активирована.");
                return;
            }
            string path = ActiveDirectory.GetPath() + file_path;

            int i = 0;
            if (File.Exists(path))
            {
                FileInfo file = new FileInfo(path);
                foreach (MyFile f in ActiveDirectory.ListFiles)
                {
                    string s = f.FileName + f.ExFile;
                    if (file.Name == s)
                    {
                        Console.WriteLine("Added " + s);
                        ActiveDirectory.ListLabels[i].TagName = $" <<--added";
                    }
                    i++;
                }
            }
            else if (Directory.Exists(path))
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                foreach (MyFile f in ActiveDirectory.ListFiles)
                {
                    string s = f.FileName + f.ExFile;
                    if (dir.Name == s)
                    {
                        Console.WriteLine("Added " + s);
                        ActiveDirectory.ListLabels[i].TagName = $" <<--added";
                    }
                    i++;
                }
            }
            else
            {
                Console.WriteLine("Нет такого файла или каталога");
            }

        }
        private void Remove(string file_path)
        {
            string dir_path = ActiveDirectory?.GetPath();
            bool dir_p = dir_path != null;
            if (!dir_p)
            {
                Console.WriteLine("Ни одна директория не активирована.");
                return;
            }
            string path = ActiveDirectory.GetPath() + file_path;

            int i = 0;
            if (File.Exists(path))
            {
                FileInfo file = new FileInfo(path);
                foreach (MyFile f in ActiveDirectory.ListFiles)
                {
                    string s = f.FileName + f.ExFile;
                    if (file.Name == s)
                    {
                        if (ActiveDirectory.ListLabels[i].TagName == " <<--new")
                        {
                            Console.WriteLine("Not removed");
                            return;
                        }
                        Console.WriteLine("Removed " + s);
                        ActiveDirectory.ListLabels[i].TagName = $" <<--removed";
                    }
                    i++;
                }
            }
            else if (Directory.Exists(path))
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                foreach (MyFile f in ActiveDirectory.ListFiles)
                {
                    string s = f.FileName + f.ExFile;
                    if (dir.Name == s)
                    {
                        if (ActiveDirectory.ListLabels[i].TagName == " <<--new")
                        {
                            Console.WriteLine("Not removed");
                            return;
                        }
                        Console.WriteLine("Removed " + s);
                        ActiveDirectory.ListLabels[i].TagName = $" <<--removed";
                    }
                    i++;
                }
            }
            else
            {
                Console.WriteLine("Файла не существует");
            }
        }
        private void Apply()
        {
            int i = 0;
            Label label;
            while(i < ActiveDirectory.ListFiles.Count)
            {
                label = ActiveDirectory.ListLabels[i];
                if (label.TagName.Contains("deleted"))
                {
                    ActiveDirectory.ListFiles.Remove(ActiveDirectory.ListFiles[i]);
                    ActiveDirectory.ListLabels.Remove(label);
                }
                else
                {
                    WriteChanges(ActiveDirectory.ListFiles[i], label);
                    i++;
                }
            }
            Console.WriteLine("Applied");
        }

        private void WriteChanges(MyFile file, Label label)
        {
            if (label.TagName != "")
            {
                label.TagName = "";
            }
            if (label.TagSize != "")
            {
                file.Size = label.TagSize.Replace(" <<--", "");
                label.TagSize = "";
            }
            if (label.TagCreate != "")
            {
                file.CreatingDate = label.TagCreate.Replace(" <<--", "");
                label.TagCreate = "";
            }
            if (label.TagModified != "")
            {
                file.ModifiedDate = label.TagModified.Replace(" <<--", "");
                label.TagModified = "";
            }
        }

        private void ListBranch()
        {
            if (Dirs.Count > 0)
            {
                Console.WriteLine("Отслеживаемые папки:");
                foreach (MyDirectory d in Dirs)
                    Console.WriteLine(d.GetPath());
            }
            else
            {
                Console.WriteLine("Ни одна папка не инициализирована.");
            }
        }
        private void Checkout(int dir_number)
        {
            if (dir_number >= 0 && dir_number < Dirs.Count)
            {
                if (Dirs[dir_number].GetPath() == ActiveDirectory.GetPath())
                {
                    Console.WriteLine("Директория уже активна");
                }
                else
                {
                    Console.WriteLine("Успешно.");
                    Console.WriteLine($"Предыдущая активная директория: {ActiveDirectory.GetPath()}");
                    ActiveDirectory = Dirs[dir_number];
                    Console.WriteLine($"Активная директория: {ActiveDirectory.GetPath()}");
                }
            }
            else
            {
                Console.WriteLine("Отслеживаемой директории с таким номером не найдено.");
            }
        }
        private void Checkout(string dir_path)
        {
            if (dir_path == ActiveDirectory.GetPath())
            {
                Console.WriteLine("Директория уже активна");
                return;
            }

            foreach (MyDirectory di in Dirs)
            {
                if (di.GetPath() == dir_path)
                {
                    Console.WriteLine("Успешно.");
                    Console.WriteLine($"Предыдущая активная директория: {ActiveDirectory.GetPath()}");
                    ActiveDirectory = di;
                    Console.WriteLine($"Активная директория: {ActiveDirectory.GetPath()}");
                    return;
                }
            }
            Console.WriteLine("Отслеживаемой директории с таким путем не существует.");
        }
        public void Run()
        {
            
            string input = "";
            string path;
            Console.Write("> ");
            input = Console.ReadLine();
            input.Trim();
            while (input != "exit")
            {
                if (input.StartsWith("init"))
                {
                    bool b = true;
                    path = input.Replace("init ", "");
                    if (!path.EndsWith(@"\"))
                        path = path + @"\";
                    foreach (MyDirectory d in Dirs)
                    {
                        if (d.GetPath() == path)
                        {
                            b = false;
                            break;
                        }
                    }
                    if (b)
                        InitCVS(path);
                }
                else if (input == "status")
                {
                    GetStatus();
                }
                else if (input.StartsWith("add"))
                {
                    path = input.Replace("add ", "");
                    Add(path);
                }
                else if (input.StartsWith("remove"))
                {
                    path = input.Replace("remove ", "");
                    Remove(path);
                }
                else if (input.StartsWith("apply"))
                {
                    // path = input.Replace("apply ", "");
                    Apply();
                }
                else if (input == "list branch")
                {
                    ListBranch();
                    Console.WriteLine();
                }
                else if (input.StartsWith("checkout"))
                {
                    path = input.Replace("checkout ", "");
                    int num;
                    if (Int32.TryParse(path, out num))
                    {
                        this.Checkout(num);
                    }
                    else
                    {
                        if ((path != null) && !path.EndsWith(@"\"))
                            path = path + @"\";
                        this.Checkout(path);
                    }
                }
                else if (input == "help")
                {
                    Console.WriteLine("init [dir_path] – инициализация СКВ для папки с файлами (или без), путь к которой указан в dir_path.");
                    Console.WriteLine("status – отображение статуса отслеживаемых файлов последней проинициализированной папки");
                    Console.WriteLine("(какие файлы отслеживаются, краткая информация по ним)");
                    Console.WriteLine(" Note: красным выделяется измененный файл, зеленым, соответственно, нет.");
                    Console.WriteLine(" Note (метки к файлам):");
                    Console.WriteLine(" added – файл добавлен с помощью команды add;");
                    Console.WriteLine(" removed – файл был убран из под версионного контроля командой remove;");
                    Console.WriteLine(" deleted – файл был либо удален, либо перемещен из папки, которая находится под версионным контролем;");
                    Console.WriteLine(" new – файл, который был либо создан, либо перемещен в папку, которая находится под версионным контролем.");
                    Console.WriteLine();
                    Console.WriteLine("add [file_path] – добавить файл под версионный контроль.");
                    Console.WriteLine("remove [file_path] – удалить файл из-под версионного контроля.");
                    Console.WriteLine("apply [dir_path] – сохранить все изменения в отслеживаемой папке (удалить все метки к файлам и сохранить изменения в них).");
                    Console.WriteLine("list branch - показать все отслеживаемые папки.");
                    Console.WriteLine("checkout [dir_path] OR [dir_number] – перейти к указанной отслеживаемой директории.");
                    Console.WriteLine(" Note: команды add, remove и т.д., которые работают с файлами используют неполный путь к файлу,");
                    Console.WriteLine(" то есть путь к папке должен добавляться приложением, в зависимости от того, ");
                    Console.WriteLine(" какая директория активна.");
                    Console.WriteLine(" Пример:");
                    Console.WriteLine(@"Активна директория C:\Users\Administrator\Projects\MyTest\");
                    Console.WriteLine("Что пишем: add work.txt");
                    Console.WriteLine(@"Что происходит: add C:\Users\Administrator\Projects\MyTest\work.txt");
                }
                else
                {
                    Console.WriteLine("Ошибка. Введите \"help\" чтобы посмотреть справку.");
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("> ");
                input = Console.ReadLine();
                input.Trim();
            }
        }
    }
}
