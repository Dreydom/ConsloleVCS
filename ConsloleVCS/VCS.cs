using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Web.Helpers;
using System.Linq;

namespace ConsloleVCS
{
    class VCS
    {
        public static List<DirectoryVersion> DirectoryList = new List<DirectoryVersion>(); //A list of all directories under control
        public static DirectoryVersion ActiveDirectory { get; set; } //this field is used to link to the directory I'm working in rn
        private bool HasMethod(string methodName)//Well. You can tell what it's doing by the name
        {
            var type = this.GetType();
            return type.GetMethod(methodName) != null;
        } 
        public void Start() //Every app launch should start with this method
        {
            if (File.Exists("data.txt")) //I didn't put much imagination to it
            {
                DirectoryList = Json.Decode<List<DirectoryVersion>>(File.ReadAllText("data.txt")); //be envy on how easy data extraction is >;D
            }
        }
        public void Init(string parameter) //this method is used to add a new directory to my directory list
        {
            if (Directory.Exists(parameter))
            {
                foreach (DirectoryVersion dir in DirectoryList)
                {
                    if (parameter == dir.Path)
                    {
                        Console.WriteLine("Путь уже инициализирован."); //the path was already initialized
                        return;
                    }
                }
                ActiveDirectory = new DirectoryVersion() {Path = parameter};
                ActiveDirectory.Init();
                DirectoryList.Add(ActiveDirectory);
                Console.WriteLine("Путь инициализирован. Папка добавлена в список."); //the path was initialized. The folder was added to the list
            }
            else
            {
                Console.WriteLine("Ошибка: Указанного пути не существует."); //error. The path doesn't exist
            }

        }
        public void Status() //behold my scariest creation ];DDD
        {
            if (ActiveDirectory == null)
            {
                Console.WriteLine("Ошибка: Отслеживаемая папка не выбрана. Используйте команды Init или Checkout для выбора активной папки."); //error. The active directory wasn't chosen. Use Init or Checkout to choose the ActiveDirectory
                return;
            }
            Console.WriteLine("Отслеживаемая папка: {0}", ActiveDirectory.Path);
            List<FileVersion> oldfiles = ActiveDirectory.FileList; //more temp variables
            List<FileInfo> newfiles = new DirectoryInfo(ActiveDirectory.Path).GetFiles().ToList(); //for the god of temp variables
            List<FileVersion> files = new List<FileVersion>(); //mooooooooreee
            foreach (FileInfo newfile in newfiles)
            {
                files.Add(new FileVersion() { Name = newfile.Name }); //i still couldn't find a way to use Concat
            }
            foreach (FileVersion file in oldfiles)
            {
                files.Add(new FileVersion() { Name = file.Name }); //shame.
            }
            files = files.GroupBy(p => p.Name).Select(g => g.First()).ToList(); //i feel guilty. This line is used to remove duplicates from my temp list.
            foreach (FileVersion file in files) //the following text is not suitable to view for kids under 12 and people with weak mental stability
            {
                int indnew = newfiles.FindIndex(item => item.Name == file.Name); //returns -1 if not found
                int indold = oldfiles.FindIndex(item => item.Name == file.Name); //returns -1 if not found
                if (indnew >= 0 && indold == -1) // new file (not in my dir list)
                {
                    FileInfo nf = newfiles[indnew];
                    file.Name = nf.Name;
                    file.Size = nf.Length;
                    file.Created = nf.CreationTime.ToString();
                    file.Modified = nf.LastWriteTime.ToString();
                    file.Log(ConsoleColor.Green, file.ToString("<-- new", -1, "", "")); //unfortunately FileVersion doesn't have Log() method
                    continue;
                }
                else if (indnew == -1 && indold >=0) //deleted file (only in my dir list)
                {
                    FileVersion of = oldfiles[indold];
                    of.Log(ConsoleColor.Red, of.ToString("<-- deleted"));
                    continue;
                }
                else //anything else. Didn't add the check up for when file doesn't exist at all, since it's not supposed to appear. Yeah, I like taking the risks B>
                {
                    FileInfo nf = newfiles[indnew]; //many temp variables below, since it's easier to handle empty ones than write a separate case for each "if"
                    FileVersion of = oldfiles[indold];
                    double lsize = -1;
                    string lcreated = "";
                    string lmodified = "";
                    ConsoleColor color = ConsoleColor.Green; //If nothing happens to the file, color remains green
                    if (of.Label == "<-- removed") //If I'm lucky enough, the program doesn't go any further and logs the file as it was before I removed it ;>
                    {
                        color = ConsoleColor.Red;
                        of.Log(color, of.ToString(of.Label));
                        continue;
                    }
                    else //If not, I'll have to make a few checkups
                    {
                        if (of.Size != nf.Length) //if size has changed. You won't see how much if the size is more than 1KB and the change is only a few bytes.
                        {
                            lsize = nf.Length;
                            color = ConsoleColor.Red;
                        }
                        if (of.Created != nf.CreationTime.ToString()) //if creation time has changed. How??? NOT MY PROBLEM.
                        {
                            lcreated = "<--" + nf.CreationTime.ToString(); //you might ask, where is the arrow for Size? Well, it's double. Watch FileVersion if you want to know how I could overcome that.
                            color = ConsoleColor.Red;
                        }
                        if (of.Modified !=nf.LastWriteTime.ToString()) //If the file was modified
                        {
                            lmodified = "<--" + nf.LastWriteTime.ToString();
                            color = ConsoleColor.Red;
                        }
                        of.Log(color, of.ToString(of.Label, lsize, lcreated, lmodified)); //adding "continue" below would slow it down, I suppose.
                    }
                }

            }
        }
        public void Add(string parameter) //a function to get unwatched files under control
        {
            if (ActiveDirectory == null)
            {
                Console.WriteLine("Ошибка: Отслеживаемая папка не выбрана. Используйте команды Init или Checkout для выбора активной папки."); //Same as above
                return;
            }
            string dirpath = ActiveDirectory.Path;
            DirectoryInfo dir = new DirectoryInfo(dirpath);
            if (!parameter.Contains(dirpath)) parameter = parameter.Insert(0, dirpath + "\\"); //it's all is needed in case if a user likes to write full paths. They must be sick, but w/e.
            if (File.Exists(parameter)) //there is still a chance that they used a wrong path. Then they wouldn't get a pice of this sexy code below to be performed.
            {
                FileInfo file = new FileInfo(parameter);
                List<FileVersion> FileList = ActiveDirectory.FileList;
                foreach (FileVersion checkfile in FileList) //check if it's in the list already
                {
                    if (checkfile.Name == file.Name)
                    {
                        if (checkfile.Label != "<-- removed") //...and if wasn't removed...
                        {
                            Console.WriteLine("Указанный файл уже находится под версионным контролем."); //"the file is already under control"
                            return; //... then the function won't be performed
                        }
                        else //but if it was removed earlier, we change it to 'added' and refresh the fields
                        {
                            checkfile.Size = file.Length;
                            checkfile.Created = file.CreationTime.ToString();
                            checkfile.Modified = file.LastWriteTime.ToString();
                            checkfile.Label = "<-- added";
                            Console.WriteLine("Файл добавлен обратно в версионный контроль."); //"file was added back under control"
                            return;
                        }
                    }
                }
                FileList.Add(new FileVersion() //if it's not in the list
                {
                    Name = file.Name,
                    Size = file.Length,
                    Created = file.CreationTime.ToString(),
                    Modified = file.LastWriteTime.ToString(),
                    Label = "<-- added"
                });
                Console.WriteLine("Новый файл добавлен в версионный контроль."); //A new file was added under control
            }
            else Console.WriteLine("Ошибка: Файл не найден."); //that's what a user that can't into paths gets
        }
        public void Remove(string parameter) //this could be used for... idk. Files that change once a second?
        {
            if (ActiveDirectory == null)
            {
                Console.WriteLine("Ошибка: Отслеживаемая папка не выбрана. Используйте команды Init или Checkout для выбора активной папки."); //same error line
                return;
            }
            string dirpath = ActiveDirectory.Path;
            DirectoryInfo dir = new DirectoryInfo(dirpath);
            if (!parameter.Contains(dirpath)) parameter = parameter.Insert(0, dirpath + "\\"); //same path-or-name check
            if (File.Exists(parameter))
            {
                FileInfo file = new FileInfo(parameter);
                List<FileVersion> FileList = ActiveDirectory.FileList;
                foreach (FileVersion checkfile in FileList)
                {
                    if (checkfile.Name == file.Name)
                    {
                        if (checkfile.Label == "<-- removed") //if the file already has 'removed' label
                        {
                            Console.WriteLine("Файл уже убран из версионного контроля"); //the file was already removed
                            return;
                        }
                        else
                        {
                            checkfile.Label = "<-- removed";
                            Console.WriteLine("Файл убран из версионного контроля"); //file was removed
                        }
                    }
                }
            }
            else Console.WriteLine("Ошибка: Файл не найден."); //file wasn't found. As you see, the whole function is an copy of Add() method
        }
        public void Apply()
        {
            if (ActiveDirectory == null)
            {
                Console.WriteLine("Ошибка: Отслеживаемая папка не выбрана. Используйте команды Init или Checkout для выбора активной папки."); //same error line
                return;
            }
            List<string> removed = new List<string>();
            foreach (FileVersion file in ActiveDirectory.FileList)
            {
                if (file.Label == "<-- removed")
                {
                    removed.Add(file.Name);
                }
            }
            ActiveDirectory.FileList.Clear();
            ActiveDirectory.Init(removed.ToArray());
            Console.WriteLine("Сохранены все изменения в папке: {0}", ActiveDirectory.Path);
            return;
        }
        public void Listbranch() //my favorite function
        {
            if (DirectoryList.Count == 0)
            {
                Console.WriteLine("Список пуст.");
            }
            Console.WriteLine("Список отслеживаемых папок:");
            int dirnumber = 1;
            foreach (DirectoryVersion dir in DirectoryList)
            {
                Console.WriteLine("{0}) {1}", dirnumber++, dir.Name); //probably it should list paths over names... well, we'll see if the teacher notices ;>
            }
        }
        public void Checkout(string parameter) //here we choose the other active directory
        {
            if (int.TryParse(parameter, out int i)) //would be cool if a user just uses an index...
            {
                ActiveDirectory = DirectoryList[i - 1]; 
                Console.WriteLine("Отслеживаемая папка: {0}", ActiveDirectory.Path); //the folder we work in rn
                return;
            }
            else if (!Directory.Exists(parameter)) //...cause if not, that guy with crooky fingers might misspell the path again
            {
                Console.WriteLine("Ошибка: Указанный путь не существует."); //error. The path doesn't exist.
                return;
            }
            else
            {
                int inddir = DirectoryList.FindIndex(item => item.Path == parameter); //getting the index. Told you that indexes are cool.
                if (inddir == -1)
                {
                    Console.WriteLine("Ошибка: Данная папка не инициализирована."); //the folder is not in DirectoryList
                    return;
                }
                ActiveDirectory = DirectoryList[inddir];
                Console.WriteLine("Отслеживаемая папка: {0}", ActiveDirectory.Path); //the folder we work in rn
                return;
            }
        }
        public void Help()  //this is a help command. Is it helpful? Doubt.
        {
            Console.WriteLine("Список команд:"); //list of commands
            Console.WriteLine("Init [dir_path] — инициализация СКВ для папки, путь к которой указан в dir_path."); //initializes a folder
            Console.WriteLine("Status — отображение статуса отслеживаемых файлов текущей отслеживаемой директории."); //shows the status of the active directory
            Console.WriteLine("Add [file_path] — добавить файл под версионный контроль."); //add a file under control
            Console.WriteLine("Remove [file_path] – удалить файл из-под версионного контроля."); //remove the file from control
            Console.WriteLine("Apply [dir_path] – сохранить все изменения в отслеживаемой папке (удалить все метки к файлам и сохранить изменения в них)."); //save the changes
            Console.WriteLine("Listbranch -  показать все отслеживаемые папки."); //yet again, my favorite - lists the folders we look into
            Console.WriteLine("Checkout [dir_path] OR [dir_number] – перейти к указанной отслеживаемой директории"); //it's like goto, but with folders
        }
        public void Exit()
        {
            File.WriteAllText("data.txt", Json.Encode(DirectoryList));
            Environment.Exit(1); //I wanted to use Application.Close, but there was something wrong with it.
        }

        public void ReadCommand(string command, string parameter = "") //This functions runs a command and parameters you write. Note how the second parameter is optional.
        {
            if (String.IsNullOrEmpty(command)) return; //happens sometimes. It will just add a newline thanks to this checkup.
            if (parameter == "Start") return; //should not be read as a command
            if (!this.HasMethod(command))
            {
                Console.WriteLine("Ошибка: Нет команды с именем \"{0}\".", command); //error: there is no command with such a name
                return;
            }
            else
            {
                MethodInfo method = this.GetType().GetMethod(command); //getting the method we want to run
                try
                {
                    if (String.IsNullOrEmpty(parameter)) //no parameters
                        method.Invoke(this, null);
                    else //one parameter
                        method.Invoke(this, new[] { parameter });
                }
                catch (TargetParameterCountException e)
                {
                    Console.WriteLine("Ошибка: {0}", e.Message); //Error. Wrong amount of parameters
                    return;
                }
            }
            return;
        }
    }
}
