using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Yamigration
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                IEmployeeLoader employeeLoader = null;

                // Проверяем, передан ли параметр -f для загрузки файла
                if (args.Length >= 2 && args[0] == "-f")
                {
                    string filePath = args[1];
                    employeeLoader = new FileEmployeeLoader(filePath);
                }
                else
                {
                    // Ввод ФИО вручную через запятую
                    Console.WriteLine("Введите список ФИО через запятую (Пример: Иванов Иван Иванович, Петров Петр Петрович):");
                    string input = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        throw new InvalidInputException("Ошибка: Введено пустое значение.");
                    }

                    employeeLoader = new ManualEmployeeLoader(input);
                }

                // Ввод формата логина
                Console.WriteLine("Выберите формат логина (1 - name.surname, 2 - n.surname):");
                string formatChoice = Console.ReadLine();

                // Если введено пустое значение, устанавливаем вариант по умолчанию
                if (string.IsNullOrWhiteSpace(formatChoice))
                {
                    formatChoice = "1";
                }

                // Проверка на правильный ввод
                if (formatChoice != "1" && formatChoice != "2")
                {
                    throw new InvalidInputException("Ошибка: Некорректный выбор формата.");
                }

                // Загрузка списка сотрудников
                List<Employee> employees = employeeLoader.LoadEmployees();

                // Получаем путь к папке загрузок
                string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                string filePathOutput = Path.Combine(downloadsPath, "migration.csv");

                // Список для хранения созданных логинов
                List<string> createdLogins = new List<string>();

                // Открываем файл для записи
                using (StreamWriter writer = new StreamWriter(filePathOutput))
                {
                    // Записываем заголовок
                    writer.WriteLine("\"login\";\"password\";\"first_name\";\"last_name\";\"middle_name\";\"gender\";\"birthday\";\"language\"");

                    // Обработка каждого сотрудника
                    foreach (var employee in employees)
                    {
                        employee.GenerateLogin(formatChoice);

                        writer.WriteLine($"\"{employee.Login}\";\"{employee.Password}\";\"{employee.FirstName}\";\"{employee.LastName}\";\"{employee.MiddleName}\";\"{employee.Gender}\";\"01.01.2019\";\"ru\"");

                        createdLogins.Add(employee.Login);
                    }
                }

                // Вывод сообщения о созданных аккаунтах
                if (createdLogins.Count > 0)
                {
                    Console.WriteLine("Созданы аккаунты:");
                    foreach (var login in createdLogins)
                    {
                        Console.WriteLine($"{login}@ms11.ru");
                    }
                    Console.WriteLine($"Файл сохранен в {filePathOutput}");
                }
            }
            catch (InvalidInputException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Ошибка: Файл сотрудников не найден.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        }
    }
}