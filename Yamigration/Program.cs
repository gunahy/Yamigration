using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {

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
            Console.WriteLine("Ошибка: Некорректный выбор формата.");
            return;
        }

        string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
        string filePath = Path.Combine(downloadsPath, "migration.csv");

        // Ввод списка ФИО через запятую
        Console.WriteLine("Введите список ФИО через запятую:");
        string input = Console.ReadLine();

        // Проверка на пустое значение или null
        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Ошибка: Введено пустое значение.");
            return;
        }

        // Разделяем ввод на отдельные ФИО
        string[] fullNames = input.Split(',');

        // Список для хранения созданных логинов
        List<string> createdLogins = new List<string>();

        // Открываем файл для записи
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            // Записываем заголовок
            writer.WriteLine("\"login\";\"password\";\"first_name\";\"last_name\";\"middle_name\";\"gender\";\"birthday\";\"language\"");

            // Обработка каждого ФИО
            foreach (string fullName in fullNames)
            {
                // Убираем лишние пробелы
                string trimmedName = fullName.Trim();

                // Разбиваем ФИО на части
                string[] nameParts = trimmedName.Split(' ');

                if (nameParts.Length >= 2 && nameParts.Length <= 3)
                {
                    string lastName = nameParts[0];
                    string firstName = nameParts[1];
                    string middleName = nameParts.Length == 3 ? nameParts[2] : string.Empty;


                    // Генерация логина на основе фамилии и имени
                    //string login = $"{Transliterate(firstName.ToLower())}.{Transliterate(lastName.ToLower())}";

                    string fname1 = Transliterate(firstName.ToLower());
                    string login = GenerateLogin(firstName, lastName, formatChoice); 
                    // Генерация пароля
                    string password = GeneratePassword(12); //"Starlite27$"; //

                    // Определение пола
                    string gender = DetermineGender(middleName);

                    // Запись строки в файл
                    writer.WriteLine($"\"{login}\";\"{password}\";\"{firstName}\";\"{lastName}\";\"{middleName}\";\"{gender}\";\"\";\"ru\"");

                    // Добавляем логин в список созданных логинов
                    createdLogins.Add($"{login}@ms11.ru\n{password}");
                }
                else
                {
                    Console.WriteLine($"Ошибка: Некорректное ФИО '{trimmedName}'.");
                }
            }
        }

        // Вывод сообщения о созданных аккаунтах
        if (createdLogins.Count > 0)
        {
            Console.WriteLine("Созданы аккаунты:");
            foreach (var login in createdLogins)
            {
                Console.WriteLine(login);
            }
            Console.WriteLine($"Сохранено в файле {filePath}");
        }
    }

    static string GenerateLogin(string firstName, string lastName, string formatChoice)
    {
        string login;
        if (formatChoice == "1") // name.surname
        {
            login = $"{Transliterate(firstName.ToLower())}.{Transliterate(lastName.ToLower())}";
        }
        else // n.surname
        {
            login = $"{Transliterate(firstName[0].ToString().ToLower())}.{Transliterate(lastName.ToLower())}";
        }
        return login;
    }

    // Метод для генерации пароля
    static string GeneratePassword(int length)
    {
        const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
        const string digits = "0123456789";
        const string specialChars = "!@#$%^&*()-_=+";

        Random random = new Random();

        StringBuilder password = new StringBuilder();

        // Гарантируем наличие хотя бы одного символа каждого типа
        password.Append(upperCase[random.Next(upperCase.Length)]);
        password.Append(lowerCase[random.Next(lowerCase.Length)]);
        password.Append(digits[random.Next(digits.Length)]);
        //password.Append(specialChars[random.Next(specialChars.Length)]);

        // Заполняем оставшуюся часть пароля случайными символами из всех категорий
        string allChars = upperCase + lowerCase + digits;
        for (int i = 4; i < length; i++)
        {
            password.Append(allChars[random.Next(allChars.Length)]);
        }

        // Перемешиваем символы в пароле для большей случайности
        return new string(password.ToString().OrderBy(c => random.Next()).ToArray());
    }

    // Метод для определения пола на основе отчества
    static string DetermineGender(string middleName)
    {
        if (middleName.EndsWith("ич"))
        {
            return "male";
        }
        else if (middleName.EndsWith("на"))
        {
            return "female";
        }
        else
        {
            return "unknown";
        }
    }

    // Метод для транслитерации кириллических символов в латиницу
    static string Transliterate(string text)
    {
        Dictionary<char, string> translitMap = new Dictionary<char, string>()
        {
            {'а', "a"}, {'б', "b"}, {'в', "v"}, {'г', "g"}, {'д', "d"},
            {'е', "e"}, {'ё', "e"}, {'ж', "zh"}, {'з', "z"}, {'и', "i"},
            {'й', "y"}, {'к', "k"}, {'л', "l"}, {'м', "m"}, {'н', "n"},
            {'о', "o"}, {'п', "p"}, {'р', "r"}, {'с', "s"}, {'т', "t"},
            {'у', "u"}, {'ф', "f"}, {'х', "kh"}, {'ц', "ts"}, {'ч', "ch"},
            {'ш', "sh"}, {'щ', "sch"}, {'ъ', ""}, {'ы', "y"}, {'ь', ""},
            {'э', "e"}, {'ю', "yu"}, {'я', "ya"}
        };

        StringBuilder result = new StringBuilder();

        foreach (char c in text)
        {
            if (translitMap.ContainsKey(c))
            {
                result.Append(translitMap[c]);
            }
            else
            {
                result.Append(c); // Оставляем символы, не подлежащие транслитерации, как есть
            }
        }

        return result.ToString();
    }
}
