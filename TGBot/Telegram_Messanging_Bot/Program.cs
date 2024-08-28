using Newtonsoft.Json;
using System;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram_Messanging_Bot
{
    struct BotUpdate
    {
        public string text;
        public long id;
        public string? username;

    }
    class Program
    {
        static TelegramBotClient Bot = new TelegramBotClient
            ("7002027437:AAE846ngvBKppW1QcHl61wn58OFS_OPRq1A");

        static string fileName = "updates.json";
        static List<BotUpdate> botUpdates = new List<BotUpdate> ();

        static void Main(string[] args)
        {
            //read all saved updates 

            try
            {
                var botUpdateString = System.IO.File.ReadAllText(fileName);

                botUpdates = JsonConvert.DeserializeObject<List<BotUpdate>>(botUpdateString) ??
                    botUpdates;
            }
            catch(Exception ex) 
            { 
                Console.WriteLine($"Error reading or deserializing{ex}");  
            }
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new UpdateType[]
                {
                    UpdateType.Message,
                    UpdateType.EditedMessage,

                }
            };
            Bot.StartReceiving(UpdateHandler, ErrorHandler, receiverOptions);

            Console.ReadLine();
        }

        private static async Task ErrorHandler(ITelegramBotClient client, Exception exception, 
            CancellationToken token)
        {
            throw new NotImplementedException();
        }

        private static async Task UpdateHandler(ITelegramBotClient bot, Update update, 
            CancellationToken token)
        {
            if (update.Type == UpdateType.Message && update.Message.Type == MessageType.Text)
            {
                var text = update.Message.Text.ToLower();
                var id = update.Message.Chat.Id;
                string? username = update.Message.Chat.Username;

                switch (text)
                {
                    case "/start":
                        await bot.SendTextMessageAsync(id, "Привіт! Я твій асистен. Чим я можу допомогти?");
                        break;

                    case "/help":
                        await bot.SendTextMessageAsync(id, "Ось команди, які я розумію: /start, /help, /set_reminder");
                        break;

                    default:
                        await bot.SendTextMessageAsync(id, "Вибачте, я не розумію цю командую");
                        break;
                }

                Console.WriteLine($"{username}| {id} | {text}");
            }
        }
    }
}