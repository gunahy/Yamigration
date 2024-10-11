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
                IEmployeeLoader? employeeLoader = null;

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

                // Генерация логинов и паролей
                List<string> createdLogins = new List<string>();
                string filePathOutput = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "migration.csv");

                using (StreamWriter writer = new StreamWriter(filePathOutput))
                {
                    writer.WriteLine("\"login\";\"password\";\"first_name\";\"last_name\";\"middle_name\";\"gender\";\"birthday\";\"language\"");
                    foreach (var employee in employees)
                    {
                        employee.GenerateLogin(formatChoice);

                        // Проверка, что Login не пустой
                        if (string.IsNullOrWhiteSpace(employee.Login))
                        {
                            throw new InvalidInputException("Ошибка: Логин не может быть NULL.");
                        }

                        // Запись данных в файл
                        writer.WriteLine($"\"{employee.Login}\";\"{employee.Password}\";\"{employee.FirstName}\";\"{employee.LastName}\";\"{employee.MiddleName}\";\"{employee.Gender}\";\"01.01.2019\";\"ru\"");
                        createdLogins.Add(employee.Login);

                        // Вывод логина и пароля на экран
                        Console.WriteLine($"Создан аккаунт {employee.Login}@ms11.ru\nПароль {employee.Password}");
                    }
                }
                Console.WriteLine($"Файл сохранен в {filePathOutput}");
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