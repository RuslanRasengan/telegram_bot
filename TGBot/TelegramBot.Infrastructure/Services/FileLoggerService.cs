using TelegramBot.Core.Interfaces.Logging;

namespace TelegramBot.Infrastructure.Services
{
    public class FileLoggerService : ILoggerService
    {
        private readonly string _logFilePath;

        public FileLoggerService(string logFilePath)
        {
            _logFilePath = logFilePath;
        }

        public void LogInfo(string message)
        {
            LogToFile($"INFO: {message}");
        }

        public void LogError(string message, Exception ex)
        {
            LogToFile($"ERROR: {message}, Exception: {ex.Message}");
        }

        public void LogToFile(string logMessage)
        {
            using(StreamWriter writer = new StreamWriter(_logFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now}: {logMessage}");
            }
        }
    }
}
