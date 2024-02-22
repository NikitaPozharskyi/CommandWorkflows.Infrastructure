using CommandWorkflows.Infrastructure.Abstraction;
using CommandWorkflows.Infrastructure.Abstraction.Commands;

namespace CommandWorkflows.Infrastructure.DependencyProvider;

public interface IWorkflowAndCommandDependencyProvider
{
    public IWorkflow<TRequest, TResponse> GetWorkflow<TRequest, TResponse>(Type workflowType)
        where TRequest: IRequest;
    
    public ICommand<TRequest, TResponse> GetCommand<TRequest, TResponse>(Type commandType)
        where TRequest: IRequest;

}