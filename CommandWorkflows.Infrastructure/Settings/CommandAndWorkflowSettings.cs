namespace CommandWorkflows.Infrastructure.Settings;

public class CommandAndWorkflowSettings
{
    public Dictionary<string, Type> CommandDictionary { get; } = new();

    public Dictionary<Type, List<Type>> WorkflowDictionary { get; } = new();
}

[Obsolete("Deprecated, please use CommandAndWorkflowSettings")]
public class TelegramBotCommandAndWorkflowSettings
{
    public Dictionary<string, Type> CommandDictionary { get; } = new();

    public Dictionary<Type, List<Type>> WorkflowDictionary { get; } = new();
}