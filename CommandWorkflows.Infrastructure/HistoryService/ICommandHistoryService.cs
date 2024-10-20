namespace CommandWorkflows.Infrastructure.HistoryService;

public interface ICommandHistoryService<in TKey>
{
    void AddCommandToHistory(string command, TKey userId, Type type);

    CommandMetadata? GetCommandFromHistory(TKey userId);

    void IncreaseWorkflowExecutionPosition(TKey userId);
    
    void RemoveCommandFromHistory(TKey userId);
}