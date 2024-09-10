using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Core.Interfaces;

namespace TelegramBot.Core.Strategies
{
    public class HelpCommandStrategy : ICommandStrategy
    {
        public async Task ExecuteAsync(ITelegramBotClient botClient, Message message)
        {
            string helpText = "Цей бот може виконувати такі команди:\n" +
                                "/start - Початок роботи з помічником\n" +
                                "/help - Допомога\n" +
                                "/ask - Задати питання OpenAi.";

            await botClient.SendTextMessageAsync(message.Chat.Id, helpText);
        }
    }
}
