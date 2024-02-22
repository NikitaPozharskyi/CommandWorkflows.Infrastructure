using CommandWorkflows.Infrastructure.Abstraction;
using CommandWorkflows.Infrastructure.Abstraction.Commands;
using CommandWorkflows.Infrastructure.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace CommandWorkflows.Infrastructure.DependencyProvider;

public class WorkflowAndCommandDependencyProvider : IWorkflowAndCommandDependencyProvider
{
    private readonly IServiceProvider _serviceProvider;

    public WorkflowAndCommandDependencyProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IWorkflow<TRequest, TResponse> GetWorkflow<TRequest, TResponse>(Type workflowType) where TRequest: IRequest
    {
        return _serviceProvider.GetRequiredService(workflowType) as IWorkflow<TRequest, TResponse> ?? throw new InvalidWorkflowException("Requested type is not a Workflow");
    }

    public ICommand<TRequest, TResponse> GetCommand<TRequest, TResponse>(Type commandType) where TRequest: IRequest
    {
        var requiredService = _serviceProvider.GetRequiredService(commandType);
        var service = requiredService as ICommand<TRequest, TResponse>;
        return service ?? throw new InvalidCommandException("Requested type is not a Command");
    }
    
    
}