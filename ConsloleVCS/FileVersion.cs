using System;

namespace ConsloleVCS
{
    class FileVersion
    {
        private static string ToReadableSize(double size) //I only need this function to get a proper displaying, actual data is stored in double, not string
        {
            if (size == -1) return ""; //needed this for size labels, aka "when the size changes"
            if (size > 1073741824)
            {
                size /= 1073741824;
                return size.ToString("0.##") + " Gb";
            }
            else if (size > 1048576)
            {
                size /= 1048576;
                return size.ToString("0.##") + " Mb";
            }
            else if (size > 1024)
            {
                size /= 1024;
                return size.ToString("0.##") + " Kb";
            }
            else
                return size + " b";
        }
        public string Name { get; set; } //Name
        public double Size { get; set; } //Size
        public string Created { get; set; } //When made
        public string Modified { get; set; } //When changed
        public string Label { get; set; } //A label. Either removed or added
        private const string stringFormat = //this is where magic happens
@"file: {0} {1}
    size: {2} {3}
    created: {4} {5}
    modified: {6} {7}
";
        public string ToString(string label = "", double lsize = -1, string lcreated = "", string lmodified = "") //optional parameters. As you see, need label used as one too, since it's more than just "removed" or "added" when written in Status
        {
            string temp = ""; //that's where the magic fails...
            if (lsize >= 0) temp = "<-- "; //I need an extra variable ssince size is double
            return String.Format(stringFormat,
                                Name, label,
                                ToReadableSize(Size), temp + ToReadableSize(lsize),
                                Created, lcreated,
                                Modified, lmodified);
        }
        public void Log(ConsoleColor color, string data) //this one is basically to write the previous function. Plus colors. Could use in one method tbh.
        {
            Console.ForegroundColor = color;
            Console.WriteLine(data);
            Console.ResetColor();
        }
    }
}
