using CommandWorkflows.Infrastructure.Abstraction.Commands;

namespace CommandWorkflows.Infrastructure.HistoryService;

public class CommandHistoryService<TKey> : ICommandHistoryService<TKey>
where TKey: notnull
{
    private readonly Dictionary<TKey, ICommand> _commandHistory = new();

    public void AddCommandToHistory(ICommand command, TKey userId)
    {
        var isExists = _commandHistory.TryGetValue(userId, out _);

        if (isExists)
        {
            _commandHistory[userId] = command;
            return;
        }

        _commandHistory.Add(userId, command);
    }

    public ICommand? GetCommandFromHistory(TKey userId)
    {
        var isExists = _commandHistory.TryGetValue(userId, out var command);

        return isExists ? command : null;
    }

    public void RemoveCommandFromHistory(TKey userId)
    {
        _commandHistory.Remove(userId);
    }
}