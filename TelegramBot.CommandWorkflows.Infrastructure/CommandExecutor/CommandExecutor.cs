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

    public async Task<string> ExecuteCommand(string text, long userId)
    {
        // todo: Implement Admin, SuperAdmin commands execution with check.
        string response;
        var commandModel = _commandHistoryService.GetCommandFromHistory(userId);
        try
        {
            // todo: Create ExitCommand.
            // var isExitCommand = _commandConverter.IsExitCommand(text);
            // if (isExitCommand)
            // {
            //     if (commandModel == null)
            //     {
            //         return "Нема з чого виходити!";
            //     }
            //
            //     _commandHistoryService.RemoveCommandFromHistory(telegramUserId);
            //     return "Вихід успішний.";
            // }

            if (commandModel == null)
            {
                var command = _commandResolver.GetCommand(text);

                response = await command.ExecuteAsync();

                if (!command.Workflows.Any()) return response;

                _commandHistoryService.AddCommandToHistory(command, userId);
                return response;
            }

            var workflow = commandModel.Workflows.Peek();
            _logger.LogInformation("Start executing workflow {Workflow}...", workflow.GetType());
            // try to execute command.
            // If it's failed we don't want to pop the workflow. Just repeat prev steps
            response = await workflow.ExecuteAsync(text);
            // if command was successfully executed we deque it from list.
            LastExecutableWorkflow = commandModel.Workflows.Dequeue();

            if (!commandModel.Workflows.Any())
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