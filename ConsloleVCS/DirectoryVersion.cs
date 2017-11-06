using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsloleVCS
{
    class DirectoryVersion
    {
        public DirectoryVersion()
        {
        }

        public bool IsActive { get; set; }
        public string Path { get; set; }
        public List<FileVersion> FileList { get; set; }
    }
}
