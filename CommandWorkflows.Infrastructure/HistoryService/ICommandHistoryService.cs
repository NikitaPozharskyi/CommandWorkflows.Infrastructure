using CommandWorkflows.Infrastructure.Abstraction.Commands;

namespace CommandWorkflows.Infrastructure.HistoryService;

public interface ICommandHistoryService<in TKey>
{
    void AddCommandToHistory(ICommand notAdminCommand, TKey userId);

    ICommand? GetCommandFromHistory(TKey userId);
    
    void RemoveCommandFromHistory(TKey userId);
}