using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram_Messanging_Bot
{
    class Program
    {
        static ITelegramBotClient botClient;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Ініціалізація клієнта бота з використанням токену
            botClient = new TelegramBotClient("7002027437:AAE846ngvBKppW1QcHl61wn58OFS_OPRq1A");

            // Налаштування скасування прослуховування оновлень
            var cts = new CancellationTokenSource();

            // Запускаємо прослуховування оновлень
            botClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, cancellationToken: cts.Token);

            Console.WriteLine("Бот запущений! Натисніть ентер для зупинки");
            Console.ReadLine();


            // Зупиняємо прослуховування оновлень
            cts.Cancel();
        }

        static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken token)
        {
            // Обробляємо помилки,що виникли під час роботи API Telegram
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException

                    => $"Помилка Telegram API: \n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString() 
            };

            Console.WriteLine(errorMessage);
            return Task.CompletedTask;
        }

        static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Перевіряємо тип оновлення (чи повідомлення) та наявність тексту в ньому
            if (update.Type == UpdateType.Message && update.Message!.Text != null)
            {
                // Отримуємо ідентифікатор чату
                var chatId = update.Message.Chat.Id;

                // Отримуємо текст повідомлення
                var messageText = update.Message.Text;

                Console.WriteLine($"Отримано повідомлення: '{messageText} від чату {chatId}'");

                // Перевіряємо, чи є повідомлення командою
                if (messageText.StartsWith("/"))
                {
                    await HandleCommandAsync(botClient, update.Message);
                }
                else
                {
                    // Відправляємо відповідь на звичайне повідомлення
                    await botClient.SendTextMessageAsync(chatId, "Ви сказали: " + messageText, cancellationToken: cancellationToken);
                }
            }
        }

        static async Task HandleCommandAsync(ITelegramBotClient botClient, Message message)
        {
            var commandText = message.Text;

            if (commandText == "/start")
            {
                // Обробляємо команду /start
                await botClient.SendTextMessageAsync(message.Chat.Id, "Вітаю! Я ваш бот. Я готовий допомогти вам.");
            }
            else if (commandText == "/help")
            {
                // Обробляємо команду /help
                await botClient.SendTextMessageAsync(message.Chat.Id, "Цей бот може виконувати такі команди: \n/start - початок роботи з ботом\n/help - Допомога");
            }
            else
            {
                // Невідома команда
                await botClient.SendTextMessageAsync(message.Chat.Id, "Невідома команда. Використайте /help для отримання списку команд.");
            }
        }
    }
}