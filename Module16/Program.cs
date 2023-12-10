using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;



namespace Module16
{
   
    

    class Program
    {
        static FileSystemWatcher watcher;
        static string directoryPath;
        static string logFilePath;

        static void Main()
        {
            Console.WriteLine("Добро пожаловать в приложение логирования изменений в файлах!");

            // Получаем путь к отслеживаемой директории и путь к лог-файлу от пользователя
            SetConfiguration();

            // Настраиваем и запускаем FileSystemWatcher
            SetupFileSystemWatcher();

            Console.WriteLine("Отслеживание запущено. Для остановки нажмите любую клавишу.");
            Console.ReadKey();

            // Останавливаем FileSystemWatcher при завершении работы
            watcher.EnableRaisingEvents = false;
        }

        static void SetConfiguration()
        {
            Console.Write("Введите путь к отслеживаемой директории: ");
            directoryPath = Console.ReadLine();

            Console.Write("Введите путь к лог-файлу: ");
            logFilePath = Console.ReadLine();
        }

        static void SetupFileSystemWatcher()
        {
            watcher = new FileSystemWatcher();

            // Устанавливаем отслеживаемую директорию
            watcher.Path = directoryPath;

            // Включаем отслеживание событий
            watcher.EnableRaisingEvents = true;

            // Указываем, какие события нам интересны
            watcher.Created += OnFileChanged;
            watcher.Deleted += OnFileChanged;
            watcher.Renamed += OnFileRenamed;
        }

        static void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            LogToFile($"[{DateTime.Now}] {e.ChangeType}: {e.FullPath}");
        }

        static void OnFileRenamed(object sender, RenamedEventArgs e)
        {
            LogToFile($"[{DateTime.Now}] {e.ChangeType}: {e.OldFullPath} => {e.FullPath}");
        }

        static void LogToFile(string logMessage)
        {
            try
            {
                // Записываем лог в указанный файл
                using (StreamWriter sw = File.AppendText(logFilePath))
                {
                    sw.WriteLine(logMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка записи в лог-файл: {ex.Message}");
            }
        }
    }
