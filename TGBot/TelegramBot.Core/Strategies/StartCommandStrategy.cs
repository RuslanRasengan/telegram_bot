using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Core.Interfaces;

namespace TelegramBot.Core.Strategies
{
    public class StartCommandStrategy : ICommandStrategy
    {
        public async Task ExecuteAsync(ITelegramBotClient botClient, Message message)
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, "Вітаю! Я ваш бот помічник. Я готовий вам допомогти.");
        }
    }
}
