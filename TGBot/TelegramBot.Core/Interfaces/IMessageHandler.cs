using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.Core.Interfaces
{
    public interface IMessageHandler
    {
        Task HandleMessageAsync(ITelegramBotClient botClient, Message message);
    }
}
