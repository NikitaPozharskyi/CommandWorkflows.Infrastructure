namespace CommandWorkflows.Infrastructure.Abstraction.Commands;

[Obsolete("Library do not support user types of commands anymore")]
public interface ISuperAdminCommand<TRequest, TResponse> : ICommand<TRequest, TResponse>
    where TRequest: IRequest
{
    
}