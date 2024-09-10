namespace TelegramBot.Core.Models
{
    public class ChatResponse
    {
        public long ChatId { get; set; } // Ідентифікатор чату користувача
        public string ResponseMessage { get; set; } // Повідомлення для відправки
        public bool IsSuccess { get; set; } // Флаг успішної обробки запиту

        public ChatResponse(long chatId, string responseMessage, bool isSuccess = true)
        {
            ChatId = chatId;
            ResponseMessage = responseMessage;
            IsSuccess = isSuccess;
        }
    }
}
