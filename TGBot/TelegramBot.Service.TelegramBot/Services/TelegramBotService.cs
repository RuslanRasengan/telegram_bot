using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Core.Interfaces.Logging;
using TelegramBot.Core.Interfaces.Messaging;
using TelegramBot.Core.Interfaces.Services;

namespace TelegramBot.Service.TelegramBot.Services
{
    public class TelegramBotService : ITelegramBotService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IMessageHandler _messageHandler;
        private readonly ILoggerService _logger;

        public TelegramBotService(
            ITelegramBotClient botClient,
            IMessageHandler messageHandler,
            ILoggerService logger)
        {
            _botClient = botClient;
            _messageHandler = messageHandler;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>() // Отримати всі типи оновлень
            };

            _botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken: cancellationToken);

            _logger.LogInfo("Бот запущений! Натисність Enter для зупинки.");
            await Task.CompletedTask;
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message && update.Message != null)
            {
                await _messageHandler.HandleMessageAsync(botClient, update.Message);
            }
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError("Помилка Telegram API", exception);
            return Task.CompletedTask;
        }
    }
}
