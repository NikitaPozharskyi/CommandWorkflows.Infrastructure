using TelegramBot.CommandWorkflows.Infrastructure.Abstraction.Commands;

namespace TelegramBot.CommandWorkflows.Infrastructure.HistoryService;

public class CommandHistoryService : ICommandHistoryService
{
    private readonly Dictionary<long, ICommand> _commandHistory = new();

    public void AddCommandToHistory(ICommand command, long userId)
    {
        var isExists = _commandHistory.TryGetValue(userId, out var value);

        if (isExists)
        {
            _commandHistory[userId] = command;
            return;
        }

        _commandHistory.Add(userId, command);
    }

    public ICommand? GetCommandFromHistory(long userId)
    {
        var isExists = _commandHistory.TryGetValue(userId, out var command);

        return !isExists ? null : command;
    }

    public void RemoveCommandFromHistory(long userId)
    {
        _commandHistory[userId] = null;
    }

    public bool IsExistsCommand(long userId)
    {
        var isExists = _commandHistory.TryGetValue(userId, out var command);
        
        return command != null;
    }
}