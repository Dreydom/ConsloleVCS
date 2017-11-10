using System;

namespace ConsloleVCS
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Добро пожаловать в систему контроля версий v0.2!");
            Console.WriteLine("Используйте команду Help чтобы увидеть список доступных команд или команду Exit для выхода из приложения.");
            do
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
            } while (true);
        }
    }
}
