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
                foreach (DirectoryVersion dir in DirectoryList)
                {
                    if (parameter == dir.Path)
                    {
                        ActiveDirectory = dir;
                        Console.WriteLine("Путь инициализирован.");
                        return;
                    }
                }
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
            Console.WriteLine("Отслеживаемая папка: {0}", ActiveDirectory.Path);
            ActiveDirectory.Log();
        }
        public void Listbranch()
        {
            Console.WriteLine("Список отслеживаемых папок:");
            foreach (DirectoryVersion dir in DirectoryList)
            {
                Console.WriteLine(dir.Name());
            }
        }
        public void Help()
        {
            Console.WriteLine("Список команд:");
            Console.WriteLine("Init [dir_path] — инициализация СКВ для папки, путь к которой указан в dir_path.");
            Console.WriteLine("Status — отображение статуса отслеживаемых файлов последней проинициализированной папки.");
            Console.WriteLine("Add [file_path] — добавить файл под версионный контроль.");
            Console.WriteLine("Remove [file_path] – удалить файл из-под версионного контроля.");
            Console.WriteLine("Apply [dir_path] – сохранить все изменения в отслеживаемой папке (удалить все метки к файлам и сохранить изменения в них).");
            Console.WriteLine("Listbranch -  показать все отслеживаемые папки.");
            Console.WriteLine("Checkout [dir_path] OR [dir_number] – перейти к указанной отслеживаемой директории");

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
