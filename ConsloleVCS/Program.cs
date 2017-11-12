using System;

namespace ConsloleVCS
{
    class Program
    {
        static void Main(string[] args)
        {
            VCS vcs = new VCS();
            vcs.Start();
            Console.WriteLine("Добро пожаловать в систему контроля версий v0.9!");
            Console.WriteLine("Используйте команду Help чтобы увидеть список доступных команд или команду Exit для выхода из приложения.");
            do
            {
                Console.ForegroundColor = ConsoleColor.White;
                string[] arr = Console.ReadLine().Split(new[] { ' ' }, 2);
                Console.ResetColor();
                string command = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(arr[0].ToLower());
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
