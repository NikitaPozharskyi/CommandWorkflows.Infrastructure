using CommandWorkflows.Infrastructure.Abstraction;
using CommandWorkflows.Infrastructure.Abstraction.Commands;
using CommandWorkflows.Infrastructure.DependencyProvider;
using CommandWorkflows.Infrastructure.Extensions;

namespace CommandWorkflows.Infrastructure.Resolver;

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
        
        var workflowQueue = workflowTypes.Select(workflowType => _workflowAndCommandDependencyProvider.GetWorkflow(workflowType)).ToQueue();

        return workflowQueue;
    }
    
    public IPermanentExitCommand? GetExitCommand(string commandName)
    {
        return _commandClrTypeResolver.IsCommandExists(commandName)? GetCommand(commandName) as IPermanentExitCommand : null;
    }
}