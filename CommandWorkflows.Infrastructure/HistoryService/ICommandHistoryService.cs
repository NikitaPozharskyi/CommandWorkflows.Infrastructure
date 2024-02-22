using CommandWorkflows.Infrastructure.Abstraction;
using CommandWorkflows.Infrastructure.Abstraction.Commands;

namespace CommandWorkflows.Infrastructure.HistoryService;

public interface ICommandHistoryService<in TKey>
{
    void AddCommandToHistory(string command, TKey userId);

    ICommand<TRequest, TResponse>? GetCommandFromHistory<TRequest, TResponse>(TKey userId) where TRequest: IRequest;

    void IncreaseWorkflowExecutionPosition(TKey userId);
    
    void RemoveCommandFromHistory(TKey userId);
}