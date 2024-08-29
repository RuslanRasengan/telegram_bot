namespace Telegram_Messanging_Bot
{
    public class BotCommand
    {
        public string Command { get; set; }

        public string Description { get; set; }

        public Func<long, Task> Execute { get; set; }


        public BotCommand(string command, string description, Func<long, Task> execute)
        {
            Command = command;
            Description = description;
            Execute = execute;

        }
    }
}
