using Microsoft.Extensions.Logging;
using TelegramBot.CommandWorkflows.Infrastructure.Abstraction;
using TelegramBot.CommandWorkflows.Infrastructure.Exceptions;
using TelegramBot.CommandWorkflows.Infrastructure.HistoryService;
using TelegramBot.CommandWorkflows.Infrastructure.Resolver;

namespace TelegramBot.CommandWorkflows.Infrastructure.CommandExecutor;

public class CommandExecutor : ICommandExecutor
{
    private readonly ICommandHistoryService _commandHistoryService;
    private readonly ICommandResolver _commandResolver;
    private readonly ILogger<ICommandExecutor> _logger;

    public IWorkflow? LastExecutableWorkflow { get; set; }
    public CommandExecutor(ICommandHistoryService commandHistoryService,
        ICommandResolver commandResolver, ILogger<ICommandExecutor> logger)
    {
        _commandHistoryService = commandHistoryService;
        _commandResolver = commandResolver;
        _logger = logger;
    }

    public async Task<string> ExecuteCommandAsync(string text, long userId)
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
            // try to execute command.
            // If it's failed we don't want to pop the workflow. Just repeat prev steps
            response = await workflow.ExecuteAsync(text);
            // if command was successfully executed we deque it from list.
            LastExecutableWorkflow = commandFromHistory.Workflows.Dequeue();

            if (!commandFromHistory.Workflows.Any())
            {
                _commandHistoryService.RemoveCommandFromHistory(userId);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exception was thrown");
            response = ExceptionHandler(e);
        }

        return response;
    }

    private string ExceptionHandler(Exception e)
    {
        switch (e)
        {
            case InvalidDataException:
            case InvalidCommandException:
                return e.Message;
            default:
                return "Щось пішло не так, спробуйте ще раз";
        }
    }
}