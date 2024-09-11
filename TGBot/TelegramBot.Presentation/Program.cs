﻿using Telegram.Bot;
using Microsoft.Extensions.DependencyInjection;
using TelegramBot.Core.Interfaces.Commands;
using TelegramBot.Core.Interfaces.Logging;
using TelegramBot.Core.Interfaces.Messaging;
using TelegramBot.Core.Interfaces.Services;
using TelegramBot.Infrastructure.Services;
using TelegramBot.Service.OpenAi.Services;
using TelegramBot.Service.Weather.Services;
using TelegramBot.Service.TelegramBot;
using TelegramBot.Service.TelegramBot.Services;
using TelegramBot.Service.TelegramBot.Strategies;
using Microsoft.Extensions.Configuration;

public static class Program
{
    public static async Task Main(string[] args)
    {
        // Встановлення кодування UTF-8 Для консолі
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // Створення IConfigurator з налаштуваннями з файлу appsettings.json
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // Встановлюємо кореневу директорію для пошуку файлів конфігурації
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Додаємо файл appsettings.json
            .Build();

        var serviceCollection = new ServiceCollection();
        ConfigureService(serviceCollection, configuration);

        var serviceProvider = serviceCollection.BuildServiceProvider();
        var telegramBotService = serviceProvider.GetRequiredService<ITelegramBotService>();

        var cts = new CancellationTokenSource();
        await telegramBotService.StartAsync(cts.Token);
    }

    static void ConfigureService(IServiceCollection services, IConfiguration configuration)
    {
        // Реєстрація IConfiguration у DI-контейнері
        services.AddSingleton<IConfiguration>(configuration);

        // Реєстрація сервісів
        services.AddSingleton<ITelegramBotClient>(provider =>
        {
            var token = configuration["TelegramBotClient:Token"];
            return new TelegramBotClient(token);
        });

        services.AddHttpClient<IOpenAiService, OpenAiService>();
        services.AddHttpClient<IWeatherService, WeatherService>();
        services.AddSingleton<ILoggerService, LoggerService>();
        services.AddSingleton<FileLoggerService>();
        services.AddSingleton<ITelegramBotService, TelegramBotService>();

        // Реєстрація обробників
        services.AddSingleton<IMessageHandler, MessageHandler>();
        services.AddSingleton<ICommandHandler, CommandHandler>();

        // Реєстрація стратегій команд
        services.AddSingleton<CommandContext>();
        services.AddSingleton<CommandFactory>();
        services.AddSingleton<ICommandStrategy, StartCommandStrategy>();
        services.AddSingleton<ICommandStrategy, HelpCommandStrategy>();
        services.AddSingleton<ICommandStrategy, AskCommandStrategy>();
        services.AddSingleton<ICommandStrategy, WeatherCommandStrategy>();
        services.AddSingleton<ICommandStrategy, UnknownCommandStrategy>();
    }
}