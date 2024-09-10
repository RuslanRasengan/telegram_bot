using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Core.Interfaces.Commands;
using TelegramBot.Core.Interfaces.Messaging;

namespace TelegramBot.Service.TelegramBot.Services
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
