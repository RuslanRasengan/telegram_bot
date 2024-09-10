namespace TelegramBot.Core.Interfaces
{
    public interface IWeatherService
    {
        Task<string> GetWeatherAsync(string cityName);
    }
}
