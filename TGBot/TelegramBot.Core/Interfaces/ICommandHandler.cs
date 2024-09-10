using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.Core.Interfaces
{
    public interface ICommandHandler
    {
        Task HandleCommandAsync(ITelegramBotClient botClient, Message message);
    }
}
