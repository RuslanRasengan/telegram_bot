using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.Core.Interfaces.Commands
{
    public interface ICommandHandler
    {
        Task HandleCommandAsync(ITelegramBotClient botClient, Message message);
    }
}
