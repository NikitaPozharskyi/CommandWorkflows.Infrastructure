namespace TelegramBot.CommandWorkflows.Infrastructure.Settings;

public class TelegramBotCommandAndWorkflowSettings
{
    public Dictionary<string, Type> CommandDictionary { get; } = new();

    public Dictionary<Type, List<Type>> WorkflowDictionary { get; } = new();
}