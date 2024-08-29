using Telegram.Bot;

namespace Telegram_Messanging_Bot
{
    public class BotCommandHandler
    {
        private readonly TelegramBotClient _bot;
        private List<BotCommand> _commands;
        public BotCommandHandler(TelegramBotClient bot) 
        {
            _bot = bot;
            InitializeCommands();
        }

        private void InitializeCommands() 
        {
            _commands = new List<BotCommand>
            {
                new BotCommand("/start", "Запуск бота", async (id) =>
                {
                    await _bot.SendTextMessageAsync(id, "Привіт! Я твій асистент.");
                }),
                new BotCommand("/help", "Допомога", async (id) =>
                {
                    string helpMessage = string.Join("\n", _commands.Select(c => $"{c.Command} - {c.Description}"));
                    await _bot.SendTextMessageAsync(id, helpMessage);
                })
            };
        }

        public async Task HandleCommand(string commandText, long chatId)
        {
            var command = _commands.FirstOrDefault(c => c.Command == commandText);
            if (command != null)
            {
                await command.Execute(chatId);
            }
            else
            {
                await _bot.SendTextMessageAsync(chatId, "Вибачте, я не розумію цю команду");
            }
        }

        public List<BotCommand> GetCommands()
        {
            return _commands;
        }
    }
}