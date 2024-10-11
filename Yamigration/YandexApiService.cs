using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Yamigration
{
    public class YandexApiService
    {
        private readonly string _baseUrl = "https://api360.yandex.net/directory/v1/org/{org_id}/users";
        private readonly string _token;
        private readonly HttpClient _httpClient;

        public YandexApiService(string token)
        {
            _token = token ?? throw new ArgumentNullException(nameof(token));
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"OAuth {_token}");
        }

        public async Task<bool> CreateUserAsync(Employee employee)
        {
            var url = _baseUrl.Replace("{org_id}", "7975862"); // Замените на ID вашей организации
            var user = new
            {
                login = employee.Login,
                password = employee.Password,
                first_name = employee.FirstName,
                last_name = employee.LastName,
                middle_name = employee.MiddleName,
                gender = employee.Gender,
                birthday = "2000-01-01",  // Пример даты рождения
                language = "ru"
            };

            var jsonContent = JsonConvert.SerializeObject(user);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Пользователь {employee.Login}@ms11.ru успешно создан.\nПароль {employee.Password}");
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Ошибка при создании пользователя: {errorContent}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
                return false;
            }
        }
    }
}
