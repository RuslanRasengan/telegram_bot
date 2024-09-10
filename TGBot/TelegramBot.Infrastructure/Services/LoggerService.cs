using TelegramBot.Core.Interfaces.Logging;

namespace TelegramBot.Infrastructure.Services
{
    public class LoggerService : ILoggerService
    {
        public void LogInfo(string message)
        {
            Console.WriteLine($"INFO: {message}");
        }
        public void LogError(string message, Exception ex)
        {
            Console.WriteLine($"ERROR: {message}, Exception: {ex.Message}");
        }
    }
}
