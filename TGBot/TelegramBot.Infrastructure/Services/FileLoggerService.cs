using Microsoft.Extensions.Logging;
using TelegramBot.Core.Interfaces.Logging;

namespace TelegramBot.Infrastructure.Services
{
    public class FileLoggerService : ILoggerService
    {
        private readonly ILogger<FileLoggerService> _logger;

        public FileLoggerService(ILogger<FileLoggerService> logger)
        {
            _logger = logger;
        }

        public void LogInfo(string message)
        {
            _logger.LogInformation(message);
        }

        public void LogError(string message, Exception ex)
        {
            _logger.LogError(message, ex);
        }
    }
}
