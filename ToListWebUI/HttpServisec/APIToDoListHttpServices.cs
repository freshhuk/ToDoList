using System.Net.Http;
using ToDoListWebDomain.Domain.Models;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ToDoListWebDomain.Domain.Entity;

namespace ToListWebUI.HttpServisec
{
    public class APIToDoListHttpServices
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<APIToDoListHttpServices> _logger;

        public APIToDoListHttpServices(HttpClient httpClient, ILogger<APIToDoListHttpServices> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        //метод для отправки пост запроса апи на добовление задачи
        public async Task<string> AddTaskDbAsync(ToDoTask model)
        {
            _logger.LogInformation($"Sending task to API: {JsonSerializer.Serialize(model)}");
            try
            {
                // Преобразовать модель пользователя в JSON-строку
                var json = JsonSerializer.Serialize(model);

                // Создать строковое содержимое запроса
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Выполнить HTTP POST-запрос на сервер API
                var response = await _httpClient.PostAsync("http://localhost:5133/api/Task/AddTaskDb", content);
                _logger.LogInformation(message: "Данные отправились");
                if (response.IsSuccessStatusCode)
                {
                    // Регистрация успешна, вернуть успешное сообщение
                    _logger.LogInformation(message: "Успешно");
                    return "successful";
                }
                else
                {
                    // Получить текст ошибки из ответа сервера
                    _logger.LogError($"HTTP POST запрос завершился с ошибкой: {response.StatusCode}");
                    var errorText = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Текст ошибки: {errorText}");

                    return "nosuccessful";
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(message: "Регистрация не успешна");
                // Обработка исключения, если что-то пошло не так
                return $"Произошла ошибка при выполнении запроса: {ex.Message}";
            }
        }
        //метод который отправляет пост запрос на сервер апишки для удаление задачи
        public async Task<string> DeleteTaskDbAsync(int Id)
        {
            try
            {
                var json = JsonSerializer.Serialize(Id);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                await Console.Out.WriteLineAsync($"Id: {Id}");

                Console.WriteLine($"JSON: {json}");
                // Выполнить HTTP POST-запрос на сервер API
                var response = await _httpClient.PostAsync("http://localhost:5133/api/Task/DeleteTaskDb", content);
                _logger.LogInformation(message: "Данные отправились");
                if(response.IsSuccessStatusCode)
                {
                    _logger.LogInformation(message: "Успешно");
                    return "successful";
                }
                else
                {
                    // Получить текст ошибки из ответа сервера
                    _logger.LogError($"HTTP POST запрос завершился с ошибкой: {response.StatusCode}");
                    var errorText = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Текст ошибки: {errorText}");

                    return "nosuccessful";
                }

            }
            catch (Exception ex)
            {

                _logger.LogInformation(message: "Удаление не успешно");
                // Обработка исключения, если что-то пошло не так
                return $"Произошла ошибка при выполнении запроса: {ex.Message}";
            }

        }
        //метод который отправляет пост запрос на изминение нашей задачи на сервер апишки
        public async Task<string> ChangeTaskDbAsync(ChangeTaskModel chamgemodel)
        {
            try
            {
                var json = JsonSerializer.Serialize(chamgemodel);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Выполнить HTTP POST-запрос на сервер API
                var response = await _httpClient.PostAsync("http://localhost:5133/api/Task/ChangeTaskDb", content);
                _logger.LogInformation(message: "Данные отправились");
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation(message: "Успешно");
                    return "successful";
                }
                else
                {
                    // Получить текст ошибки из ответа сервера
                    _logger.LogError($"HTTP POST запрос завершился с ошибкой: {response.StatusCode}");
                    var errorText = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Текст ошибки: {errorText}");

                    return "nosuccessful";
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(message: "Удаление не успешно");
                // Обработка исключения, если что-то пошло не так
                return $"Произошла ошибка при выполнении запроса: {ex.Message}";
            }
        }
    }
}
