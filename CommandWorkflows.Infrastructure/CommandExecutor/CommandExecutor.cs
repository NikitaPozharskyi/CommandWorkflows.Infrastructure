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

    public CommandExecutor(ICommandHistoryService<TKey> commandHistoryService,
        ICommandResolver commandResolver, ILogger<CommandExecutor<TKey>> logger)
    {
        _commandHistoryService = commandHistoryService;
        _commandResolver = commandResolver;
        _logger = logger;
    }

    public async Task<TResponse> ExecuteCommandAsync<TRequest, TResponse>(TRequest request, TKey userId) where TRequest: IRequest 
    {
        TResponse response;
        var commandFromHistory = _commandHistoryService.GetCommandFromHistory<TRequest, TResponse>(userId);
        try
        {
            var isExitCommand = _commandResolver.GetExitCommand<TRequest, TResponse>(request.Message);
            if (isExitCommand != null)
            {
                _commandHistoryService.RemoveCommandFromHistory(userId);
                commandFromHistory = null;
            }
            
            if (commandFromHistory == null)
            {
                var command = _commandResolver.GetCommand<TRequest, TResponse>(request.Message);

                response = await command.ExecuteAsync(request);

                if (!command.Workflows.Any()) return response;

                _commandHistoryService.AddCommandToHistory(request.Message, userId);
                return response;
            }

            var workflow = commandFromHistory.Workflows.Peek();
            
            _logger.LogInformation("Start executing workflow {Workflow}...", workflow.GetType());
            response = await workflow.ExecuteAsync(request);
            
            commandFromHistory.Workflows.Dequeue();
            _commandHistoryService.IncreaseWorkflowExecutionPosition(userId);

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