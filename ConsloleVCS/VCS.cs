using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;

namespace ConsloleVCS
{
    class VCS
    {
        public static List<DirectoryVersion> DirectoryList = new List<DirectoryVersion>();
        public static DirectoryVersion ActiveDirectory { get; set; }
        private bool HasMethod(string methodName)
        {
            var type = this.GetType();
            return type.GetMethod(methodName) != null;
        }
        public void Init(string parameter)
        {
            if (Directory.Exists(parameter))
            {
                ActiveDirectory = new DirectoryVersion() {Path = parameter};
                ActiveDirectory.Init();
                DirectoryList.Add(ActiveDirectory);
                Console.WriteLine("Путь инициализирован. Папка добавлена в ветор");
            }
            else
            {
                Console.WriteLine("Ошибка: Указанного пути не существует.");
            }

        }
        public void Status()
        {
            ActiveDirectory.Log();
        }
        public void Hello(string parameter)
        {
            Console.WriteLine("Привет! Параметр, который вы ввели — {0}", parameter);
        }
        public void Lmao()
        {
            Console.WriteLine("AYYY LMAO");
        }
        public void Exit()
        {
            Environment.Exit(1);
        }

        public void ReadCommand(string command, string parameter = "")
        {
            if (String.IsNullOrEmpty(command)) return;
            if (!this.HasMethod(command))
            {
                Console.WriteLine("Ошибка: Нет команды с именем \"{0}\".", command);
                return;
            }
            else
            {
                MethodInfo method = this.GetType().GetMethod(command);
                try
                {
                    if (parameter == "")
                        method.Invoke(this, null);
                    else
                        method.Invoke(this, new[] { parameter });
                }
                catch (TargetParameterCountException e)
                {
                    Console.WriteLine("Ошибка: {0}", e.Message);
                    return;
                }
            }
            return;
        }
    }
}
