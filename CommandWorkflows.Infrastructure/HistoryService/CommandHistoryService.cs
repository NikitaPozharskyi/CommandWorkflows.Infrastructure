namespace CommandWorkflows.Infrastructure.HistoryService;

public class CommandHistoryService<TKey> : ICommandHistoryService<TKey>
    where TKey: notnull
{
    private readonly Dictionary<TKey, CommandMetadata> _commandHistory = new();

    public void AddCommandToHistory(string command, TKey userId)
    {
        var isExists = _commandHistory.TryGetValue(userId, out _);

        if (isExists)
        {
            _commandHistory[userId] = new CommandMetadata(command, 0);
            return;
        }

        _commandHistory.Add(userId, new CommandMetadata(command, 0));
    }

    public CommandMetadata? GetCommandFromHistory(TKey userId)
    {
        var isExists = _commandHistory.TryGetValue(userId, out var commandMetadata);

        return isExists ? commandMetadata : null;
    }

    public void IncreaseWorkflowExecutionPosition(TKey userId)
    {
        var isExists = _commandHistory.TryGetValue(userId, out var command);
        if (!isExists) return;
        command!.Position++;
        _commandHistory[userId] = command;
    }

    public void RemoveCommandFromHistory(TKey userId)
    {
        _commandHistory.Remove(userId);
    }
}