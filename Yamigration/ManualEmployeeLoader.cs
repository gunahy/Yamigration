using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yamigration
{
    internal class ManualEmployeeLoader : IEmployeeLoader
    {
        private string _input;

        public ManualEmployeeLoader(string input)
        {
            _input = input;
        }

        public List<Employee> LoadEmployees()
        {
            var employees = new List<Employee>();

            foreach (var item in _input.Split(','))
            {
                string[] nameParts = item.Trim().Split(' ');

                if (nameParts.Length >= 2 && nameParts.Length <= 3)
                {
                    string lastName = nameParts[0];
                    string firstName = nameParts[1];
                    string middleName = nameParts.Length == 3 ? nameParts[2] : string.Empty;

                    employees.Add(new Employee(firstName, lastName, middleName));
                }
                else
                {
                    throw new InvalidInputException($"Некорректное ФИО: {item.Trim()}");
                }
            }

            return employees;
        }
    }
}
