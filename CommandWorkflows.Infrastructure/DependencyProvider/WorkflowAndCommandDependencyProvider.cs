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

    public IWorkflow GetWorkflow(Type workflowType) 
    {
        return _serviceProvider.GetRequiredService(workflowType) as IWorkflow ?? throw new InvalidWorkflowException("Requested type is not a Workflow");
    }

    public ICommand GetCommand(Type commandType)
    {
        return _serviceProvider.GetRequiredService(commandType) as ICommand ?? throw new InvalidCommandException("Requested type is not a Command");
    }
    
    
}