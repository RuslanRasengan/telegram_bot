using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TelegramBot.Core.Interfaces.Services;

namespace TelegramBot.Service.Weather.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;

        private readonly string _apiKey = "YOUR_APIKEY";

        public WeatherService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["OpenWeather: ApiKey"]; // API ключ з конфігурації
        }

        public async Task<string> GetWeatherAsync(string cityName)
        {
            string apiUrl = $"http://api.openweathermap.org/data/2.5/weather?q={cityName}&appid={_apiKey}&units=metric&lang=uk";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                dynamic weatherData = JsonConvert.DeserializeObject(responseBody);

                string weatherDescription = weatherData.weather[0].description;
                double temperature = weatherData.main.temp;

                return $"Погода в {cityName}: {weatherDescription}, температура: {temperature}°C.";
            }
            catch (Exception ex)
            {
                return $"Не вдалося отримати погоду для {cityName}. Помилка: {ex.Message}";
            }
        }
    }
}
