using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;

namespace ConsloleVCS
{
    class VCS
    {
        public List<DirectoryVersion> DirectoryList = new List<DirectoryVersion>();
        public DirectoryVersion ActiveDirectory { get; set; }
            /*{
                if (Directory.Exists(value))
                {
                    activeDirectory = value;
                    foreach (DirectoryVersion directory in DirectoryList)
                    {
                        directory.IsActive = false;
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка: Не существует указанного пути.");
                    return;
                }
            }*/
        
        private bool HasMethod(string methodName)
        {
            var type = this.GetType();
            return type.GetMethod(methodName) != null;
        }
        public void Hello(string parameter)
        {
            Console.WriteLine("Привет! Параметр, который вы ввели — {0}", parameter);
        }
        public void Lmao()
        {
            Console.WriteLine("AYYY LMAO");
        }

        public void Init(string parameter)
        {
            ActiveDirectory = new DirectoryVersion() { Path = parameter, IsActive = true };
            DirectoryList.Add(ActiveDirectory);
            Console.WriteLine("Путь инициализирован. Папка добавлена в ветор");

        }
        public void ReadCommand(string command, string parameter = "")
        {
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
