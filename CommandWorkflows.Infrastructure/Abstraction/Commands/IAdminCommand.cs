namespace CommandWorkflows.Infrastructure.Abstraction.Commands;

public interface IAdminCommand<TRequest, TResponse> : ICommand <TRequest, TResponse>
where TRequest: IRequest
{
    
}