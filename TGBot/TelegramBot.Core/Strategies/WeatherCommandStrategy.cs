using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Core.Interfaces;

namespace TelegramBot.Core.Strategies
{
    public class WeatherCommandStrategy : ICommandStrategy
    {
        private readonly IWeatherService _weatherService;
        public WeatherCommandStrategy(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        public async Task ExecuteAsync(ITelegramBotClient botClient, Message message)
        {
            // Витягуємо назву міста з тексту повідомлення після команди /weather
            var cityName = message.Text.Substring("/weather".Length).Trim();

            if (string.IsNullOrEmpty(cityName))
            {
                // Якщо користувач не вказав назву міста, відправляємо повідомлення про помилку
                await botClient.SendTextMessageAsync(message.Chat.Id, "Будь ласка, введіть назву вашого міста після команди /weather.");
                return;                
            }
            // Отримуємо дані про погоду для введеного міста
            string weatherInfo = await _weatherService.GetWeatherAsync(cityName);

            // Відправляємо інформацію про погоду користувачу
            await botClient.SendTextMessageAsync(message.Chat.Id, weatherInfo);
        }
    }
}
