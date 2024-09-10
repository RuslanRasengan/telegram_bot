namespace TelegramBot.Core.Interfaces.Services
{
    public interface IOpenAiService
    {
        Task<string> GetResponseAsync(string userInput);
    }
}
