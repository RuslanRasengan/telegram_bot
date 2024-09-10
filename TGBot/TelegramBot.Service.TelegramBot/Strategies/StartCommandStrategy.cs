using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Core.Interfaces.Commands;

namespace TelegramBot.Service.TelegramBot.Strategies
{
    public class StartCommandStrategy : ICommandStrategy
    {
        public async Task ExecuteAsync(ITelegramBotClient botClient, Message message)
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, "Вітаю! Я ваш бот помічник. Я готовий вам допомогти.");
        }
    }
}
