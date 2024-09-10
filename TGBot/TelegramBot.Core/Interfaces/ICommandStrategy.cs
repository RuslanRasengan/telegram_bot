using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.Core.Interfaces
{
    public interface ICommandStrategy
    {
        /// <summary>
        /// Виконує логіку обробки команди.
        /// </summary>
        /// <param name="botClient">Клієнт Telegram Bot</param>
        /// <param name="meassage">Повідомлення від користувача</param>
        /// <returns></returns>
        Task ExecuteAsync(ITelegramBotClient botClient, Message meassage);
    }
}