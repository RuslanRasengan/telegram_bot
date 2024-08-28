using System;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram_Messanging_Bot
{
    class Program
    {
        static TelegramBotClient Bot = new TelegramBotClient
            ("7002027437:AAE846ngvBKppW1QcHl61wn58OFS_OPRq1A");

        static void Main(string[] args)
        {
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
            if (update.Type == UpdateType.Message)
            {
                if (update.Message.Type == MessageType.Text)
                {
                    var text = update.Message.Text;
                    var id = update.Message.Chat.Id;
                    string? username = update.Message.Chat.Username;

                    Console.WriteLine($"{username}| {id} | {text}");

                }
            }
        }
    }
}