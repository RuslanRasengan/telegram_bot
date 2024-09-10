using Newtonsoft.Json;
using System.Text;
using TelegramBot.Core.Interfaces.Services;

namespace TelegramBot.Service.OpenAi.Services
{
    public class OpenAiService : IOpenAiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public OpenAiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["OpenAi:ApiKey"]; //API ключ з конфігурації

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);
        }

        public async Task<string> GetResponseAsync(string userInput)
        {
            var requestBody = new
            {
                model = "gpt-3.5-turbo", // Використовуйте правильну модель
                messages = new[]
                {
                    new { role = "user", content = userInput }
                },

                max_tokens = 100 // Максимальна кількість у відповіді
            };

            string jsonContent = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync("http://api.openai.com/v1/chat/completions", content);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                dynamic result = JsonConvert.DeserializeObject(responseBody);

                if (result.choices != null && result.choices.Count > 0)
                {
                    return result.choices[0].message.content;
                }
                else
                {
                    return "Отримано некоректну відповідь від API.";
                }
            }
            catch(Exception ex)
            {
                return $"Помилка під час виконання запиту до OpenAi API: {ex.Message}";
            }
        }
    }
}
