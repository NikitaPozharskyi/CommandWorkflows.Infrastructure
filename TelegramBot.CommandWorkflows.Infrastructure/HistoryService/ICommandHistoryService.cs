using TelegramBot.CommandWorkflows.Infrastructure.Abstraction.Commands;

namespace TelegramBot.CommandWorkflows.Infrastructure.HistoryService;

public interface ICommandHistoryService
{
    void AddCommandToHistory(ICommand notAdminCommand, long userId);

    ICommand? GetCommandFromHistory(long userId);
    
    void RemoveCommandFromHistory(long userId);
}