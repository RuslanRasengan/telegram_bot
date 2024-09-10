namespace TelegramBot.Core.Models
{
    /// <summary>
    /// Модель для зберігання інформації
    /// </summary>
    public class UserRequest
    {        
        public long ChatId { get; set; } // Ідентифікатор чату користувачі
        public string Command { get; set; } // Команда, введена користувачем
        public string Parameters { get; set; } // Додаткові параметри команди

        public UserRequest(long chatId, string command, string parameters = "")
        {
            ChatId = chatId;
            Command = command;
            Parameters = parameters;
        }
    }
}
