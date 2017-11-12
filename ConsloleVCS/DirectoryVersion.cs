using System.Collections.Generic;
using System.IO;

namespace ConsloleVCS
{
    class DirectoryVersion
    {
        public string Path { get; set; }
        public string Name
        {
            get
            {
                DirectoryInfo dir = new DirectoryInfo(Path);
                return dir.Name;
            }
        }
        public List<FileVersion> FileList = new List<FileVersion>();
        public void Init()  
        {
            DirectoryInfo dir = new DirectoryInfo(Path);
            FileInfo[]files = dir.GetFiles();
            foreach(FileInfo file in files)
            {
                FileList.Add(new FileVersion()
                {
                    Name = file.Name,
                    Size = file.Length,
                    Created = file.CreationTime.ToString("dd/MM/yyyy"),
                    Modified = file.LastWriteTime.ToString("dd/MM/yyyy"),
                    Label = ""
                });
            }
        }
        public List<FileVersion> GetFiles()
        {
            return FileList;
        }
    }
}
