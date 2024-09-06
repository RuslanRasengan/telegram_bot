using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

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

            var recieverOptions = new ReceiverOptions
            {
                // Отримати всі типи оновлень
                AllowedUpdates = Array.Empty<UpdateType>()
            };

            // Запускаємо прослуховування оновлень
            botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                recieverOptions,
                cancellationToken: cts.Token);

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

                Console.WriteLine($"Отримано повідомлення: '{messageText}' від чату {chatId}");

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

            else if (update.Type == UpdateType.CallbackQuery)
            {
                var callbackQuery = update.CallbackQuery;

                var callbackAlertMessage = $"Ви вибрали: {callbackQuery.Data}";

                // Обробляємо callback data від інлайн-кнопок
                await botClient.AnswerCallbackQueryAsync(
                    callbackQuery.Id,
                    // Показати спливаюче вікно
                    callbackAlertMessage,
                    showAlert: true);

                //Перевіряємо, чи доступне повідомлення, на яке натиснута кнопка
                if (callbackQuery.Message != null)
                {
                    // Можемо виконати додаткові діїї залежно від callback data
                    await botClient.SendTextMessageAsync(
                        callbackQuery.Message.Chat.Id,
                        $"Ви натиснули на кнопку з даними: {callbackQuery.Data}");
                }
                else
                {
                    // Логічний блок на випадок, якщо повідомлення недоступне
                    Console.WriteLine("Повідомлення недоступне для відправлення відповіді.");
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
                string helpText = "Цей бот може виконувати такі команди:\n" +
                                    "/start - початок роботи з ботом\n" +
                                    "/help - Допомога\n" +
                                    "/menu - Показати меню з кнопками\n" +
                                    "/inline - Показати інлайн-кнопки";

                await botClient.SendTextMessageAsync(message.Chat.Id, helpText);
            }
            else if(commandText == "/menu")
            {
                // Викликаємо метод для відправки з клавіатури
                await SendReplyKeyboardAsync(botClient, message.Chat.Id);
            }
            else if(commandText == "/inline")
            {
                //Викликаємо метод для відправки інлайн-кнопок
                await SendInlineKeyboardAsync(botClient, message.Chat.Id);
            }
            else
            {
                // Невідома команда
                await botClient.SendTextMessageAsync(message.Chat.Id, "Невідома команда. Використайте /help для отримання списку команд.");
            }
        }

        static async Task SendReplyKeyboardAsync(ITelegramBotClient botClient, long chatId)
        {
            //Створюємо кнопки для клавіатури
            var replyKeyboard = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[] {"Кнопка 1", "Кнопка 2"},
                new KeyboardButton[] {"Кнопка 3", "Кнопка 4"}
            })
            {
                ResizeKeyboard = true //Автоматично змінює розмір клавіатури під кілкість кнопок
            };

            //Відправляємо повідомлення з клавіатурою
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Оберіть опцію:",
                replyMarkup: replyKeyboard);
        }
        static async Task SendInlineKeyboardAsync(ITelegramBotClient botClient, long chatId)
        {
            //Створюємо інлайн-кнопки
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
                {
                //Кожна кнопка має текст і дані (Callback Data), які повертаються боту при натискані
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Кнопка А", "callback_a"),
                    InlineKeyboardButton.WithCallbackData("Кнопка В","callback_b")
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Кнопка С", "callback_c"),
                    InlineKeyboardButton.WithCallbackData("Кнопка D", "callback_d")
                }
            });

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Оберіть одну з кнопок нижче:",
                replyMarkup: inlineKeyboard);
        }
    }
}