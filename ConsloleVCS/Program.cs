using System;

namespace ConsloleVCS
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] arr = Console.ReadLine().Split(new[] { ' ' }, 2);
            string command = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(arr[0].ToLower());
            VCS vcs = new VCS();
            if (arr.Length == 1)
            {
                vcs.ReadCommand(command);
            }
            else
            {
                string parameters = arr[1];
                vcs.ReadCommand(command, parameters);
            }
            Console.ReadLine();
        }
    }
}
