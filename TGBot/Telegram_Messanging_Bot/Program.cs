using Newtonsoft.Json;
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
            if (update.Type == UpdateType.Message && update.Message != null)
            {
                // Отримуємо ідентифікатор чату
                var chatId = update.Message.Chat.Id;

                if (update.Message.Photo != null && update.Message.Photo.Length > 0)
                {
                    // Отримуємо файл з фотографією (останній у массиві - найвища якість)
                    var fileId = update.Message.Photo.Last().FileId;
                    var fileInfo = await botClient.GetFileAsync(fileId);
                    var filePath = fileInfo.FilePath;

                    Console.WriteLine("Поточна робоча директорія: " + Directory.GetCurrentDirectory());


                    // Завантажуємо файл локально
                    using (var fileStream = new FileStream($"photo_{chatId}.jpg", FileMode.Create))
                    {
                        await botClient.DownloadFileAsync(filePath, fileStream);
                    }

                    // Відправляємо підтвердження користувачу
                    await botClient.SendTextMessageAsync(chatId, "Фотографію отримано та збережено!.");
                }
                else if (update.Message.Text != null)
                {
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
            }

            else if (update.Type == UpdateType.CallbackQuery)
            {
                var callbackQuery = update.CallbackQuery;

                // Обробляємо callback data від інлайн-кнопок
                await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);

                //Перевіряємо, чи доступне повідомлення, на яке натиснута кнопка
                if (callbackQuery.Message != null)
                {
                    // Можемо виконати додаткові діїї залежно від callback data
                    await botClient.SendTextMessageAsync(
                        callbackQuery.Message.Chat.Id,
                        $"Ви натиснули на кнопку з даними: {callbackQuery.Data}");
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
                                    "/start - початок роботи з помічником\n" +
                                    "/help - Допомога\n" +
                                    "/menu - Показати меню з кнопками\n" +
                                    "/inline - Показати інлайн-кнопки\n" +
                                    "/weather - показу погоду\n" +
                                    "/ask - Задати питання помічнику";

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
            else if(commandText.StartsWith("/weather"))
            {
                var cityName = commandText.Substring("/weather".Length).Trim();

                if(string.IsNullOrEmpty(cityName))
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Будь ласка, введіть назву вашого міста після команди /weather.");
                }
                else
                {
                    string weatherInfo = await GetWeatherAsync(cityName);
                    await botClient.SendTextMessageAsync(message.Chat.Id, weatherInfo);
                }
            }
            else if(commandText.StartsWith("/ask"))
            {
                // Витягуємо запит користувача до ChatGpt
                var userInput = commandText.Substring("/ask".Length).Trim();

                if(string.IsNullOrEmpty (userInput))
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Будьласка, введіть ваш запит після команди /ask.");
                }
                else
                {
                    // Отримуємо відповідь від ChatGpt
                    string chatGptResponse = await GetChatGptResponseAsync(userInput);
                    await botClient.SendTextMessageAsync(message.Chat.Id, chatGptResponse);
                }
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

        static async Task<string> GetWeatherAsync(string cityName)
        {
            string apiKey = "d9aa0ab6c107b93bc93181be376cb3f4";
            string apiUrl = $"http://api.openweathermap.org/data/2.5/weather?q={cityName}&appid={apiKey}&units=metric&lang=uk";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Виконуємо HTTP GET запит до API
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();

                    //Читаємо відповідь як JSON
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Розбираємо JSON та отримуємо інформацію про погоду
                    dynamic weatherData = JsonConvert.DeserializeObject(responseBody);
                    string weatherDescription = weatherData.weather[0].description;
                    double temperature = weatherData.main.temp;

                    return $"Погода в {cityName}: {weatherDescription}, температура: {temperature}°C.";
                }
                catch (Exception ex)
                {
                    return $"Не вдалося отримати погоду для {cityName}. Помилка: {ex.Message}";
                }
            }
        }

        static async Task<string> GetChatGptResponseAsync(string userInput)
        {
            int maxRetries = 3; // Максимальна кількість спроб
            int delayMilliseconds = 2000; // Затримка між спробами (2 секунди)

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

                var requestBody = new
                {
                    model = "gpt-4o-mini-2024-07-18", // Або інша модель
                    messages = new[]
                    {
                        new { role = "user", content = userInput }
                    },
                    max_tokens = 1000 // Максимальна кількість токенів у відповіді
                };

                var jsonContent = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                for (int i = 0; i < maxRetries; i++)
                {
                    try
                    {
                        HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
                        response.EnsureSuccessStatusCode();

                        string responseBody = await response.Content.ReadAsStringAsync();
                        dynamic result = JsonConvert.DeserializeObject(responseBody);

                        string chatGptResponse = result.choices[0].message.content;

                        return chatGptResponse;
                    }
                    catch (HttpRequestException ex) when ((int)ex.StatusCode == 429)
                    {
                        Console.WriteLine("Отримано помилку 429 (Too Many Requests). Очікуємо перед повтором...");

                        // Зачекайте перед наступною спробою
                        await Task.Delay(delayMilliseconds);
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine("Помилка під час запиту до OpenAi API: " + ex.Message);
                        return "Вибачте, сталася помилка при обробці вашого запиту.";
                    }
                }
                return "Перевищено кількість спроб. Будь ласка, спробуйте пізніше.";
            }
        }
    }
}