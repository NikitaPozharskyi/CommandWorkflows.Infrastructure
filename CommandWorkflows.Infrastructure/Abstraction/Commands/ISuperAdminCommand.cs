namespace CommandWorkflows.Infrastructure.Abstraction.Commands;

public interface ISuperAdminCommand<TRequest, TResponse> : ICommand<TRequest, TResponse>
    where TRequest: IRequest
{
    
}