using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Core.Interfaces;

namespace TelegramBot.Infrastructure.Handlers
{
    public class MessageHandler : IMessageHandler
    {
        private readonly ICommandHandler _commandHandler;

        public MessageHandler(ICommandHandler commandHandler)
        {
            _commandHandler = commandHandler;
        }

        public async Task HandleMessageAsync(ITelegramBotClient botClient, Message message)
        {
            if (message.Text.StartsWith("/"))
            {
                await _commandHandler.HandleCommandAsync(botClient, message);
            }
            else
            {
                // Обробка звичайного повідомлення
                await botClient.SendTextMessageAsync(message.Chat.Id, "Я не розумію цієї команди.");
            }
        }
    }
}
