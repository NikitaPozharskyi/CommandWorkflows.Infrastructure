using CommandWorkflows.Infrastructure.Abstraction;
using CommandWorkflows.Infrastructure.Abstraction.Commands;

namespace CommandWorkflows.Infrastructure.DependencyProvider;

public interface IWorkflowAndCommandDependencyProvider
{
    public IWorkflow GetWorkflow(Type workflowType);
    
    public ICommand GetCommand(Type commandType);

}