namespace TelegramBot.Core.Interfaces.Services
{
    public interface IWeatherService
    {
        Task<string> GetWeatherAsync(string cityName);
    }
}
