using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yamigration
{
    class FileEmployeeLoader : IEmployeeLoader
    {
        private string _filePath;

        public FileEmployeeLoader(string filePath)
        {
            _filePath = filePath;
        }

        // Реализация метода для загрузки сотрудников
        public List<Employee> LoadEmployees()
        {
            var employees = new List<Employee>();

            // Чтение файла построчно
            foreach (var line in File.ReadAllLines(_filePath))
            {
                // Разделение строки на части
                string[] nameParts = line.Trim().Split(' ');

                if (nameParts.Length >= 2 && nameParts.Length <= 3)
                {
                    string lastName = nameParts[0];
                    string firstName = nameParts[1];
                    string middleName = nameParts.Length == 3 ? nameParts[2] : string.Empty;

                    // Создание объекта Employee и добавление его в список
                    employees.Add(new Employee(firstName, lastName, middleName));
                }
                else
                {
                    Console.WriteLine($"Ошибка: Некорректное ФИО '{line.Trim()}'.");
                }
            }

            return employees;
        }
}
