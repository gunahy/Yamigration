
namespace Yamigration
{
    class Employee
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Gender { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        // Конструктор для создания сотрудника
        public Employee(string firstName, string lastName, string middleName = "")
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            Gender = Tools.DetermineGender(middleName);
            Password = Tools.GeneratePassword(12);
        }

        // Метод для генерации логина
        public void GenerateLogin(string formatChoice)
        {
            Login = Tools.GenerateLogin(FirstName, LastName, formatChoice);
        }
    }
}