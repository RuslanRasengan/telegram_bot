using System;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram_Messanging_Bot
{
    class Program //створюємо основний нашого додатку,
                  //назва може бути будь якою, але зазвичай це Program 
    {
        static ITelegramBotClient botClient; // оголошення змінної botClient типу ITelegramBotClient
                                             // яка використовується для взаємодії з TelegramApi.
                                             // ITelegramBotClient - це інтерфейс, що надається бібліотекою TelegramBot,
                                             // і який ми будемо використовувати для виконання всіх запитів до TelegramApi?
                                             // static змінна є статичною, тобто належить класу Program
                                             // і доступна з будь якої точки цього класу без створення екземпляру.


        static void Main(string[] args) //головний метод нашого додатку,
                                        //це точка входу з якої починаєтся робота додатку,
                                        //static означає що створювати екземпляр не потрібно,
                                        //void означає що метод нічого не повертає
        {
            botClient = new TelegramBotClient("7002027437:AAE846ngvBKppW1QcHl61wn58OFS_OPRq1A"); //створення нового екземпляру TelegramBotClient з використанням токену
                                                                                                 //отриманого від BotFather це необхідно для того щоб бот міг взаємодіяти з серверами
                                                                                                 //Телеграм і виконувати запити, наприклад відправляти повідомлення.
                                                                                                 //
                                                                                                 //присвоюємо новий екземпляр TelegramBotClient змінній botClient 
                                                                                                 //щоб мати доступ до клієнта в цьому класі Program

            var cts = new CancellationTokenSource();// CancelationTokenSource використовується
                                                    // для контролю скасування асинхронних операцій.
                                                    // Ми використовуємо його, щоб мати змогу зупинити прослуховування
                                                    // оновлень, коли це необхідно.

            botClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, cancellationToken: cts.Token);//StartReceiving метод клієнта бота,
                                                                                                        //який запускає отримання нових оновлень.
                                                                                                        //Ми передаємо в нього методи HandleUpdateAsync
                                                                                                        //і HandleErrorAsync для обробки помилок,
                                                                                                        //а також токен скасування cts.Token

            Console.WriteLine("Бот запущений! Натисніть ентер для зупинки");
            Console.ReadLine();
            cts.Cancel();
        }

        static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken token)//HandleErrorAsync - це метод який буде викликаний
                                                                                                                //одразу при винекнені помилки.
                                                                                                                //Він приймає параметри ITelegramBot, Exception та CancelationToken.
        {
            var errorMessage = exception switch// exception switch - це конструкція swtich,
                                               // що дозволяє обробити різні типи помилок.
                                               // У нашому випадку якщо виняток - це ApiRequestException,
                                               // ми обробляємо його окремо, виводячи код помилки та повідомлення.
                                               // Усі помилки обробляються загальним способом виводячи їх у текстовому форматі.
            {
                ApiRequestException apiRequestException//ApiRequestException - Специфічний тип вийнятку,
                                                       //який виникає якщо сталася помилка під час запиту до TelegramApi.
                                                       //Ми отримуємо код і повідомлення про помилку з цього винятку.

                    => $"Помилка Telegram API: \n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString() //ця строчка використовується для обробки всіх інших типів вийнятків,
                                          //_ вказує на те, що всі інші випадки(які не є ApiRequestException )
                                          //будуть оброблені цим варіантом. exception.ToString() повертає повний опис помилки.
            };

            Console.WriteLine(errorMessage);
            return Task.CompletedTask;
        }

        static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)//HandleUpdateAsync метод який обробляє кожне отримане оновлення.
                                                                                                                             //Тут ми перевіряємо, чи є оновлення повідомленням,
                                                                                                                             //і якщо так то обробляємо його (відправляє відповідь з таким самим текстом)
        {
            if(update.Type == UpdateType.Message && update.Message!.Text != null)
            {
                var chatId = update.Message.Chat.Id;
                var messageText = update.Message.Text;

                Console.WriteLine($"Отримано повідомлення: '{messageText} від чату {chatId}'");

                //відправляємо відповідь на повідомлення
                await botClient.SendTextMessageAsync(chatId, "Ви сказали: "+ messageText, cancellationToken: cancellationToken);
            }
        }
    }
}