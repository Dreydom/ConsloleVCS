using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsloleVCS
{
    class DirectoryVersion
    {
        public string Path { get; set; } //Path of the certain folder
        public string Name //Name of that folder. If not the listbranch function, this would never be used
        {
            get
            {
                DirectoryInfo dir = new DirectoryInfo(Path); //getting the name
                return dir.Name;
            }
        }
        public List<FileVersion> FileList = new List<FileVersion>(); //every directory has a list of files...
        public void Init(params string[] parameters) //...getting them here
        {
            DirectoryInfo dir = new DirectoryInfo(Path);
            FileInfo[]files = dir.GetFiles();
            foreach(FileInfo file in files)
            {
                if (!parameters.Contains(file.Name))
                    FileList.Add(new FileVersion()
                    {
                        Name = file.Name,
                        Size = file.Length,
                        Created = file.CreationTime.ToString(), //some sexy formatting
                        Modified = file.LastWriteTime.ToString(), //screw US and some other countries for using MM/dd instead
                        Label = "" //it's needed for new and removed labels only. None could be in a new folder
                    });
            }
        }
    }
}
