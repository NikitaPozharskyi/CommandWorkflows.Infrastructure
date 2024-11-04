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
    
    public ICommand<TRequest, TResponse> GetCommand<TRequest, TResponse>(string commandName, int skip = 0) where TRequest : IRequest
    {
        var command = _workflowAndCommandDependencyProvider.GetCommand<TRequest, TResponse>(_commandClrTypeResolver.GetCommandType(commandName));
        command.Workflows = InitializeWorkflows<TRequest, TResponse>(command.GetType(), skip);

        return command;
    }
    public ICommand<TRequest, TResponse> GetCommand<TRequest, TResponse>(Type commandType, int skip = 0) where TRequest : IRequest
    {
        var command = _workflowAndCommandDependencyProvider.GetCommand<TRequest, TResponse>(commandType);
        command.Workflows = InitializeWorkflows<TRequest, TResponse>(command.GetType(), skip);

        return command;
    }

    private Queue<IWorkflow<TRequest, TResponse>> InitializeWorkflows<TRequest, TResponse>(Type commandType, int skip) where TRequest : IRequest
    {
        var workflowTypes = _commandClrTypeResolver.GetRelatedWorkflowsTypeList(commandType);
        
        var workflowQueue = workflowTypes
            .Skip(skip)
            .Select(workflowType => _workflowAndCommandDependencyProvider.GetWorkflow<TRequest, TResponse>(workflowType))
            .ToQueue();

        return workflowQueue;
    }
    
    public IPermanentExitCommand<TRequest, TResponse>? GetExitCommand<TRequest, TResponse>(string commandName) where TRequest : IRequest
    {
        return _commandClrTypeResolver.IsCommandExists(commandName) ? GetCommand<TRequest, TResponse>(commandName) as IPermanentExitCommand<TRequest, TResponse> : null;
    }
}