using System;

namespace ConsloleVCS
{
    class FileVersion
    {
        public string Name { get; set; }
        private string size;
        public string Size
        {
            get { return size; }
            set
            {
                double temp = Convert.ToDouble(value);
                if (temp > 1073741824)
                {
                    temp /= 1073741824;
                    size = temp.ToString("0.##") + " Gb";
                }
                else if (temp > 1048576)
                {
                    temp /= 1048576;
                    size = temp.ToString("0.##") + " Gb";
                }
                else if (temp > 1024)
                {
                    temp /= 1024;
                    size = temp.ToString("0.##") + " Gb";
                }
                else
                    size = value + " b";
            }
        }
        public string Created { get; set; }
        public string Modified { get; set; }
        public string Label { get; set; }
        public ConsoleColor Color { get; set; }
        public override string ToString()
        {
            return String.Format(
@"file: {0} {1}
    size: {2}
    created: {3}
    modified: {4}
", Name, Label, Size, Created, Modified);
        }
        public void Log()
        {
            Console.ForegroundColor = Color;
            Console.WriteLine(ToString());
            Console.ResetColor();
        }
    }
}
