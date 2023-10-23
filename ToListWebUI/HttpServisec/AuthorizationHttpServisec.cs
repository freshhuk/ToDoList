using System.Net.Http;
using ToDoListWebDomain.Domain.Models;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ToListWebUI.HttpServisec
{
    public class AuthorizationHttpServisec
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AuthorizationHttpServisec> _logger;

        public AuthorizationHttpServisec(HttpClient httpClient, ILogger<AuthorizationHttpServisec> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        //метод для отправки Post запроса на сервер для  регестрации
        public async Task<string> RegisterUserAsync(UserRegistration model)
        {
            try
            {
                // Преобразовать модель пользователя в JSON-строку
                var json = JsonSerializer.Serialize(model);

                // Создать строковое содержимое запроса
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Выполнить HTTP POST-запрос на сервер API
                var response = await _httpClient.PostAsync("https://localhost:7212/api/APIAccount/RegisterAccount", content);
                _logger.LogInformation(message: "Регистрация  почти успешна");
                if (response.IsSuccessStatusCode)
                {
                    // Регистрация успешна, вернуть успешное сообщение
                    _logger.LogInformation(message: "Регистрация успешна");
                    return "Регистрация успешна.";
                }
                else
                {
                    // Получить текст ошибки из ответа сервера
                    var errorText = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation(message: "Ошибка регестрации");
                    // Регистрация не удалась, вернуть сообщение об ошибке
                    return $"Ошибка при регистрации: {errorText}";
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(message: "Регистрация не успешна");
                // Обработка исключения, если что-то пошло не так
                _logger.LogError(message: $"{ex.Message}");
                return $"Произошла ошибка при выполнении запроса: {ex.Message}";
            }
        }


        //метод для отправки Post запроса на сервер для логина
        public async Task<string> LoginUserAsync(UserLogin model)
        {
            try
            {
                _logger.LogInformation($"Sending task to APIHttpServices: {JsonSerializer.Serialize(model)}");
                
                var json = JsonSerializer.Serialize(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Выполнить HTTP POST-запрос на сервер API
                var response = await _httpClient.PostAsync("https://localhost:7212/api/APIAccount/LoginAccount", content);

                if (response.IsSuccessStatusCode)
                {
                    // Регистрация успешна, вернуть успешное сообщение
                    _logger.LogInformation(message: "Регистрация успешна");
                    return "successful";
                }
                else
                {
                    // Получить текст ошибки из ответа сервера
                    var errorText = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation(message: "Ошибка регестрации");
                    // Регистрация не удалась, вернуть сообщение об ошибке
                    return $"Ошибка при регистрации: {errorText}";
                }
            }
            catch (Exception ex)
            {
                return $"Произошла ошибка при выполнении запроса: {ex.Message}";
            }
        }
        //метод для отправки Post запроса на сервер для логаута
        public async Task<string> LogoutAsync()
        {
            try
            {
                
               

                // Выполнить HTTP POST-запрос на сервер API
                var response = await _httpClient.PostAsync("https://localhost:7212/api/APIAccount/LoginAccount");

                if (response.IsSuccessStatusCode)
                {
                    // Регистрация успешна, вернуть успешное сообщение
                    _logger.LogInformation(message: "Регистрация успешна");
                    return "successful";
                }
                else
                {
                    // Получить текст ошибки из ответа сервера
                    var errorText = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation(message: "Ошибка регестрации");
                    // Регистрация не удалась, вернуть сообщение об ошибке
                    return $"Ошибка при регистрации: {errorText}";
                }
            }
            catch (Exception ex)
            {
                return $"Произошла ошибка при выполнении запроса: {ex.Message}";
            }
        }
    }
}
