using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Core.Interfaces.Commands;

namespace TelegramBot.Service.TelegramBot
{
    public class CommandContext
    {
        private readonly CommandFactory _commandFactory;
        private ICommandStrategy _currentStrategy;

        public CommandContext(CommandFactory commandFactory)
        {
           _commandFactory = commandFactory;
        }

        public void SetCommand(string command)
        {
            _currentStrategy = _commandFactory.Create(command);
        }

        public async Task ExecuteCommandAsync(ITelegramBotClient botClient, Message message)
        {
            if (_currentStrategy != null)
            {
                await _currentStrategy.ExecuteAsync(botClient, message);
            }
            else
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Невідома команда.");
            }
        }
    }
}
