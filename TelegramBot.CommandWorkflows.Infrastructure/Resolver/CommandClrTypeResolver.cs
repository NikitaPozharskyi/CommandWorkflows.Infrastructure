using Microsoft.Extensions.Options;
using TelegramBot.CommandWorkflows.Infrastructure.Exceptions;
using TelegramBot.CommandWorkflows.Infrastructure.Settings;

namespace TelegramBot.CommandWorkflows.Infrastructure.Resolver;

public class CommandClrTypeResolver : ICommandClrTypeResolver
{
    private readonly TelegramBotCommandAndWorkflowSettings _commandAndWorkflowSettings;
    
    public CommandClrTypeResolver(IOptions<TelegramBotCommandAndWorkflowSettings> commandAndWorkflowSettings)
    {
        _commandAndWorkflowSettings = commandAndWorkflowSettings.Value;
    }

    public Type GetCommandType(string commandName)
    {
        var isExists = _commandAndWorkflowSettings.CommandDictionary.TryGetValue(commandName, out var messageType);

        if (!isExists)
        {
            throw new InvalidCommandTypeException($"There is no registered command with type {commandName}");
        }

        return messageType!;
    }

    public IEnumerable<Type> GetRelatedWorkflowsTypeList(Type commandType)
    {
        var isExists = _commandAndWorkflowSettings.WorkflowDictionary.TryGetValue(commandType, out var workflows);
        
        return !isExists ? new List<Type>() : workflows!;
    }

    public bool IsCommandExists(string commandName) =>
        _commandAndWorkflowSettings.CommandDictionary.TryGetValue(commandName, out _);
}