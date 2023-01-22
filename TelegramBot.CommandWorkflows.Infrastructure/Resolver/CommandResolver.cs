using TelegramBot.CommandWorkflows.Infrastructure.Abstraction;
using TelegramBot.CommandWorkflows.Infrastructure.Abstraction.Commands;
using TelegramBot.CommandWorkflows.Infrastructure.DependencyProvider;

namespace TelegramBot.CommandWorkflows.Infrastructure.Resolver;

public class CommandResolver : ICommandResolver
{
    private readonly ICommandClrTypeResolver _commandClrTypeResolver;
    private readonly IWorkflowAndCommandDependencyProvider _workflowAndCommandDependencyProvider;

    public CommandResolver(ICommandClrTypeResolver commandClrTypeResolver, IWorkflowAndCommandDependencyProvider workflowAndCommandDependencyProvider)
    {
        _commandClrTypeResolver = commandClrTypeResolver;
        _workflowAndCommandDependencyProvider = workflowAndCommandDependencyProvider;
    }
    public ICommand GetCommand(string commandName)
    {
        var command = _workflowAndCommandDependencyProvider.GetCommand(_commandClrTypeResolver.GetCommandType(commandName));
        command.Workflows = InitializeRelatedWorkflows(command.GetType());

        return command;
    }

    private Queue<IWorkflow> InitializeRelatedWorkflows(Type commandType)
    {
        var workflowTypes = _commandClrTypeResolver.GetRelatedWorkflowsTypeList(commandType);
        var workflowQueue = new Queue<IWorkflow>();
        
        foreach (var workflow in workflowTypes.Select(workflowType => _workflowAndCommandDependencyProvider.GetWorkflow(workflowType)))
        {
            workflowQueue.Enqueue(workflow);
        }

        return workflowQueue;
    }
    
    // todo: make it as Exit Command
    public bool IsExitCommand(string command) => throw new NotImplementedException(nameof(IsExitCommand));
}