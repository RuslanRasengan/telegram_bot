using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Core.Interfaces.Commands;

namespace TelegramBot.Service.TelegramBot.Strategies
{
    public class UnknownCommandStrategy : ICommandStrategy
    {        
        public async Task ExecuteAsync(ITelegramBotClient botClient, Message message)
        {
            // Відправка повідомлення про невідому команду
            await botClient.SendTextMessageAsync(message.Chat.Id, "Невідома команда. Використайте /help для отримання списку доступних команд.");
        }
    }
}
