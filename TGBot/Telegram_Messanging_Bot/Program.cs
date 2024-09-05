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
            if(update.Type == UpdateType.Message && update.Message!.Text != null)
            {
                var chatId = update.Message.Chat.Id;
                var messageText = update.Message.Text;

                Console.WriteLine($"Отримано повідомлення: '{messageText} від чату {chatId}'");

                // Відправляємо відповідь на повідомлення
                await botClient.SendTextMessageAsync(chatId, "Ви сказали: "+ messageText, cancellationToken: cancellationToken);
            }
        }
    }
}