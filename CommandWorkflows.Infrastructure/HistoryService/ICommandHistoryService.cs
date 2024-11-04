namespace CommandWorkflows.Infrastructure.HistoryService;

public interface ICommandHistoryService<in TKey>
{
    void AddCommandToHistory(TKey userId, Type type);

    CommandMetadata? GetCommandFromHistory(TKey userId);

    void MoveForward(TKey userId);
    
    void RemoveCommandFromHistory(TKey userId);
}