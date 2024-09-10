namespace TelegramBot.Core.Models
{
    /// <summary>
    /// Модель для представлення даних про погоду.
    /// </summary>
    public class WeatherData
    {
        public string CityName { get; set; } // Назва міста
        public string Description { get; set; } // Опис погоди (наприклад, "clear sky")
        public double Temperature { get; set; } // Температура

        public WeatherData(string cityName, string description, double temperature)
        {
            CityName = cityName;
            Description = description;
            Temperature = temperature;
        }

        public override string ToString()
        {
            return $"Погода в {CityName}: {Description}, температура: {Temperature}°C.";
        }
    }
}
