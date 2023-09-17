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

        public AuthorizationHttpServisec(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> RegisterUserAsync(UserRegistration userData)
        {
            var json = JsonSerializer.Serialize(userData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:53807/api/APIAccountController/RegisterAccount", content); //отправляем пост запрос на авторизацию

            return response.IsSuccessStatusCode;
        }
    }
}
