using CommandWorkflows.Infrastructure.Abstraction;
using CommandWorkflows.Infrastructure.Abstraction.Commands;
using CommandWorkflows.Infrastructure.Resolver;

namespace CommandWorkflows.Infrastructure.HistoryService;

public class CommandHistoryService<TKey> : ICommandHistoryService<TKey>
    where TKey: notnull
{
    private readonly ICommandResolver _commandResolver;
    private readonly Dictionary<TKey, (string command, int position)> _commandHistory = new();

    public CommandHistoryService(ICommandResolver commandResolver)
    {
        _commandResolver = commandResolver;
    }

    public void AddCommandToHistory(string command, TKey userId)
    {
        var isExists = _commandHistory.TryGetValue(userId, out _);

        if (isExists)
        {
            _commandHistory[userId] = new ValueTuple<string, int>(command, 0);
            return;
        }

        _commandHistory.Add(userId, new ValueTuple<string, int>(command, 0));
    }

    public ICommand<TRequest, TResponse>? GetCommandFromHistory<TRequest, TResponse>(TKey userId) where TRequest: IRequest
    {
        var isExists = _commandHistory.TryGetValue(userId, out var command);

        return isExists ? _commandResolver.GetCommand<TRequest, TResponse>(command!.command, command.position) : null;
    }

    public void IncreaseWorkflowExecutionPosition(TKey userId)
    {
        var isExists = _commandHistory.TryGetValue(userId, out var command);
        if (!isExists) return;
        command!.position++;
        _commandHistory[userId] = command;
    }

    public void RemoveCommandFromHistory(TKey userId)
    {
        _commandHistory.Remove(userId);
    }
}