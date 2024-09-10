namespace TelegramBot.Core.Interfaces
{
    public interface IOpenAiService
    {
        Task<string> GetResponseAsync(string userInput);
    }
}
