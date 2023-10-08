using CommandWorkflows.Infrastructure.Abstraction;
using CommandWorkflows.Infrastructure.HistoryService;
using CommandWorkflows.Infrastructure.Resolver;
using Microsoft.Extensions.Logging;

namespace CommandWorkflows.Infrastructure.CommandExecutor;

public class CommandExecutor<TKey> : ICommandExecutor<TKey> 
    where TKey : notnull
{
    private readonly ICommandHistoryService<TKey> _commandHistoryService;
    private readonly ICommandResolver _commandResolver;
    private readonly ILogger<ICommandExecutor<TKey>> _logger;

    public IWorkflow? LastExecutableWorkflow { get; set; }
    public CommandExecutor(ICommandHistoryService<TKey> commandHistoryService,
        ICommandResolver commandResolver, ILogger<CommandExecutor<TKey>> logger)
    {
        _commandHistoryService = commandHistoryService;
        _commandResolver = commandResolver;
        _logger = logger;
    }

    public async Task<string> ExecuteCommandAsync(string text, TKey userId)
    {
        string response;
        var commandFromHistory = _commandHistoryService.GetCommandFromHistory(userId);
        try
        {
            var isExitCommand = _commandResolver.GetExitCommand(text);
            if (isExitCommand != null)
            {
                _commandHistoryService.RemoveCommandFromHistory(userId);
                commandFromHistory = null;
            }
            
            if (commandFromHistory == null)
            {
                var command = _commandResolver.GetCommand(text);

                response = await command.ExecuteAsync();

                if (!command.Workflows.Any()) return response;

                _commandHistoryService.AddCommandToHistory(command, userId);
                return response;
            }

            var workflow = commandFromHistory.Workflows.Peek();
            
            _logger.LogInformation("Start executing workflow {Workflow}...", workflow.GetType());
            response = await workflow.ExecuteAsync(text);
            
            LastExecutableWorkflow = commandFromHistory.Workflows.Dequeue();

            if (!commandFromHistory.Workflows.Any())
            {
                _commandHistoryService.RemoveCommandFromHistory(userId);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exception was thrown");
            throw;
        }

        return response;
    }
}