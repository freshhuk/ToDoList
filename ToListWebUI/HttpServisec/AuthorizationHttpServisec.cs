﻿using System.Net.Http;
using ToDoListWebDomain.Domain.Models;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Http;

namespace ToListWebUI.HttpServisec
{
    public class AuthorizationHttpServisec
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AuthorizationHttpServisec> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthorizationHttpServisec(IHttpContextAccessor httpContextAccessor, HttpClient httpClient, ILogger<AuthorizationHttpServisec> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
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
                var response = await _httpClient.PostAsync("https://localhost:7212/api/APIAccount/Register", content);
                _logger.LogInformation(message: "Регистрация  почти успешна");
                if (response.IsSuccessStatusCode)
                {
                    // Регистрация успешна, вернуть успешное сообщение
                    _logger.LogInformation(message: "Регистрация успешна");
                    var token = await response.Content.ReadAsStringAsync();

                    // Сохраняем JWT токен в cookie с использованием HttpContext
                    _httpContextAccessor.HttpContext?.Response.Cookies.Append("jwtToken", token);
                    return "successful";
                }
                else
                {
                    // Получить текст ошибки из ответа сервера
                    var errorText = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation(message: "Ошибка регестрации");
                    // Регистрация не удалась, вернуть сообщение об ошибке
                    return "nosuccessful";
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
                    _logger.LogInformation(message: "Логин успешен");
                    var token = await response.Content.ReadAsStringAsync();
                    _httpContextAccessor.HttpContext?.Response.Cookies.Append("jwtToken", token);
                    return "successful";
                }
                else
                {
                    _logger.LogInformation(message: "Ошибка логина");
                    return "no successful";
                }
            }
            catch (Exception ex)
            {
                return $"Произошла ошибка при выполнении запроса: {ex.Message}";
            }
        }
        //метод для отправки Post запроса на сервер для logout
        public async Task<string> LogoutAsync()
        {
            try
            {
                var content = new StringContent("");

                // Выполнить HTTP POST-запрос на сервер API
                var response = await _httpClient.PostAsync("https://localhost:7212/api/APIAccount/LogoutAccount", content);

                if (response.IsSuccessStatusCode)
                {
                    // Регистрация успешна, вернуть успешное сообщение
                    _logger.LogInformation(message: "Логаут успешен");
                    return "successful";
                }
                else
                {
                    // Получить текст ошибки из ответа сервера
                    var errorText = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation(message: "Ошибка логаута");
                    // Регистрация не удалась, вернуть сообщение об ошибке
                    return $"Ошибка при регистрации: {errorText}";
                }
            }
            catch (Exception ex)
            {
                return $"Произошла ошибка при выполнении запроса: {ex.Message}";
            }
        }
        public async Task<string> ChangeDataAccountAsync(ChangeDataAccountModel model)
        {
            try
            {
                _logger.LogInformation($"Sending task to APIHttpServices: {JsonSerializer.Serialize(model)}");

                var json = JsonSerializer.Serialize(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Выполнить HTTP POST-запрос на сервер API
                var response = await _httpClient.PostAsync("https://localhost:7212/dataaccount/ChangeDataAccount", content);

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
