using System;

namespace ConsloleVCS
{
    class FileVersion
    {
        private static string ToReadableSize(double size)
        {
            if (size == -1) return "";
            if (size > 1073741824)
            {
                size /= 1073741824;
                return size.ToString("0.##") + " Gb";
            }
            else if (size > 1048576)
            {
                size /= 1048576;
                return size.ToString("0.##") + " Gb";
            }
            else if (size > 1024)
            {
                size /= 1024;
                return size.ToString("0.##") + " Gb";
            }
            else
                return size + " b";
        }
        public string Name { get; set; }
        public double Size { get; set; }
        public string Created { get; set; }
        public string Modified { get; set; }
        public string Label { get; set; }
        private const string stringFormat =
@"file: {0} {1}
    size: {2} {3}
    created: {4} {5}
    modified: {6} {7}
";
        public string ToString(string label = "", double lsize = -1, string lcreated = "", string lmodified = "")
        {
            return String.Format(stringFormat,
                                Name, label,
                                ToReadableSize(Size), ToReadableSize(lsize),
                                Created, lcreated,
                                Modified, lmodified);
        }
        public void Log(ConsoleColor color, string data)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(data);
            Console.ResetColor();
        }
    }
}
