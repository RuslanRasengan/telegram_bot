using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Core.Interfaces.Commands;

namespace TelegramBot.Service.TelegramBot.Services
{
    public class CommandHandler : ICommandHandler
    {
        private readonly CommandContext _commandContext;

        public CommandHandler(CommandContext commandContext)
        {
            _commandContext = commandContext;
        }

        public async Task HandleCommandAsync(ITelegramBotClient botClient, Message message)
        {
            _commandContext.SetCommand(message.Text);
            await _commandContext.ExecuteCommandAsync(botClient, message);
        }
    }
}
