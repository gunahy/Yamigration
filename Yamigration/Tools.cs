using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yamigration
{
    static class Tools
    {

        // Метод для генерации логина
        public static string GenerateLogin(string firstName, string lastName, string formatChoice)
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
        public static string GeneratePassword(int length)
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
            password.Append(specialChars[random.Next(specialChars.Length)]);

            // Заполняем оставшуюся часть пароля случайными символами из всех категорий
            string allChars = upperCase + lowerCase + digits + specialChars;
            for (int i = 4; i < length; i++)
            {
                password.Append(allChars[random.Next(allChars.Length)]);
            }

            // Перемешиваем символы в пароле для большей случайности
            return new string(password.ToString().OrderBy(c => random.Next()).ToArray());
        }

        // Метод для определения пола на основе отчества
        public static string DetermineGender(string middleName)
        {
            if (string.IsNullOrWhiteSpace(middleName))
            {
                return "unknown";
            }

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
        public static string Transliterate(string text)
        {
            Dictionary<char, string> translitMap = new Dictionary<char, string>()
    {
        {'а', "a"}, {'б', "b"}, {'в', "v"}, {'г', "g"}, {'д', "d"},
        {'е', "e"}, {'ё', "e"}, {'ж', "zh"}, {'з', "z"}, {'и', "i"},
        {'й', "y"}, {'к', "k"}, {'л', "l"}, {'м', "m"}, {'н', "n"},
        {'о', "o"}, {'п', "p"}, {'р', "r"}, {'с', "s"}, {'т', "t"},
        {'у', "u"}, {'ф', "f"}, {'х', "kh"}, {'ц', "ts"}, {'ч', "ch"},
        {'ш', "sh"}, {'щ', "shch"}, {'ъ', ""}, {'ы', "y"}, {'ь', ""},
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
}
