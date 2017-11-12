using System;

namespace ConsloleVCS
{
    class Program
    {
        static void Main(string[] args)
        {
            VCS vcs = new VCS(); //The object of my funky command class. This is basically the first line I wrote.
            vcs.Start(); //It's public, yet you can't access it by using a command. Hehe.
            Console.WriteLine("Добро пожаловать в систему контроля версий v1.0!"); //it sayswelcome. How nice ;>
            Console.WriteLine("Используйте команду Help чтобы увидеть список доступных команд или команду Exit для выхода из приложения."); //use either help or exit
            do
            {
                Console.ForegroundColor = ConsoleColor.White; //commands are written with this color
                string[] arr = Console.ReadLine().Split(new[] { ' ' }, 2); //the input string splits into the command and the parameter
                Console.ResetColor();//regular color to write command messages
                string command = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(arr[0].ToLower()); //this is all just for Title case ;D
                if (arr.Length == 1) //in case if no parameters
                {
                    vcs.ReadCommand(command); 
                }
                else
                {
                    string parameters = arr[1]; //if there are, we initialize and then use this variable. 
                    vcs.ReadCommand(command, parameters); //Could've just used arr[1] here though.
                }
            } while (true); //this is an endless loop, cirlce of life and death and countless reincarnations. Just press an [x] or use Exit command
        }
    }
}
