using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Text;
using TelegramBot.Core.Interfaces;

namespace TelegramBot.Infrastructure.Services
{
    public class OpenAiService : IOpenAiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public OpenAiService(HttpClient httpClient, string apiKey)
        {
            _httpClient = httpClient;
            _apiKey = apiKey;

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
    }
}
