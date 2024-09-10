using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Core.Interfaces;

namespace TelegramBot.Core.Strategies
{
    public class CommandContext
    {
        private readonly Dictionary<string, ICommandStrategy> _commands;
        private ICommandStrategy _currentStrategy;

        public CommandContext(IEnumerable<ICommandStrategy> strategies)
        {
            _commands = new Dictionary<string, ICommandStrategy>();
            foreach (var strategy in strategies)
            {
                var commandType = strategy.GetType().Name.Replace("CommandStrategy", "").ToLower();
                _commands[$"/{commandType}"] = strategy;
            }
        }

        public void SetCommand(string command)
        {
            _commands.TryGetValue(command, out _currentStrategy);
        }

        public async Task ExecuteCommandAsync(ITelegramBotClient botClient, Message message)
        {
            if(_currentStrategy != null)
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
