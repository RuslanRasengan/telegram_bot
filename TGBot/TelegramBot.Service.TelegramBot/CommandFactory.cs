using Microsoft.Extensions.DependencyInjection;
using TelegramBot.Core.Interfaces.Commands;
using TelegramBot.Service.TelegramBot.Strategies;

namespace TelegramBot.Service.TelegramBot
{
    public class CommandFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ICommandStrategy Create(string command)
        {
            return command.ToLower() switch
            {
                "/start" => _serviceProvider.GetRequiredService<StartCommandStrategy>(),
                "/help" => _serviceProvider.GetRequiredService<HelpCommandStrategy>(),
                "/ask" => _serviceProvider.GetRequiredService<AskCommandStrategy>(),
                "/weather" => _serviceProvider.GetRequiredService<WeatherCommandStrategy>(),
                _ => _serviceProvider.GetRequiredService<UnknownCommandStrategy>()
            };
        }
    }
}
