namespace TelegramBot.Core.Interfaces.Services
{
    public interface ITelegramBotService
    {
        Task StartAsync(CancellationToken cancellationToken);
    }
}