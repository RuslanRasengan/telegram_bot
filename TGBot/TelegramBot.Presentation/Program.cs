

using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using TelegramBot.Core.Interfaces;
using TelegramBot.Core.Strategies;
using TelegramBot.Infrastructure.Handlers;
using TelegramBot.Infrastructure.Services;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();
        ConfigureService(serviceCollection);

        var serviceProvider = serviceCollection.BuildServiceProvider();
        var telegramBotService = serviceProvider.GetRequiredService<ITelegramBotService>();

        var cts = new CancellationTokenSource();

        await telegramBotService.StartAsync(cts.Token);
    }

    static void ConfigureService(IServiceCollection services)
    {
        // Реєстрація клієнта Telegram Bot
        services.AddSingleton<ITelegramBotClient>(new TelegramBotClient("YOUR_TELEGRAM_BOT_CLIENT"));

        // Реєстрація сервісів
        services.AddSingleton<IOpenAiService, OpenAiService>(provider =>
        {
            var httpClient = provider.GetRequiredService<HttpClient>();
            return new OpenAiService(httpClient, "YOUR_OPENAI_API_KEY");
        });

        services.AddSingleton<IWeatherService, IWeatherService>();
        services.AddSingleton<ICommandHandler, CommandHandler>();
        services.AddSingleton<IMessageHandler, MessageHandler>();
        services.AddSingleton<ITelegramBotService, TelegramBotService>();
        services.AddSingleton<ILoggerService, LoggerService>();
        services.AddSingleton<HttpClient>();

        // Реєстрація стратегій команд
        services.AddSingleton<CommandContext>();
        services.AddSingleton<ICommandStrategy, StartCommandStrategy>();
        services.AddSingleton<ICommandStrategy, HelpCommandStrategy>();
        services.AddSingleton<ICommandStrategy, AskCommandStrategy>();
        services.AddSingleton<ICommandStrategy, WeatherCommandStrategy>();
    }
}