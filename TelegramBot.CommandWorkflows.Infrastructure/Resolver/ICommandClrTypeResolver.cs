namespace TelegramBot.CommandWorkflows.Infrastructure.Resolver;

public interface ICommandClrTypeResolver
{
    public Type GetCommandType(string commandName);

    public List<Type> GetRelatedWorkflowsTypeList(Type commandType);

    public bool IsCommandExists(string commandName);
}