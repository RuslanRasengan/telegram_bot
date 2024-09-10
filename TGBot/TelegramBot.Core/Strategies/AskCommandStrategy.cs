using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Core.Interfaces;

namespace TelegramBot.Core.Strategies
{
    public class AskCommandStrategy : ICommandStrategy
    {
        private readonly IOpenAiService _openAiService;

        public async Task ExecuteAsync(ITelegramBotClient botClient, Message message)
        {
            var userInput = message.Text.Substring("/ask".Length).Trim();
            if (string.IsNullOrEmpty(userInput))
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Будь ласка, введіть ваш запит після команди /ask.");
            }
            else
            {
                // Отримуємо відповідь від OpenAI API
                string chatGptResponse = await _openAiService.GetResponseAsync(userInput);
                await botClient.SendTextMessageAsync(message.Chat.Id, chatGptResponse);
            }
        }

    }
}
