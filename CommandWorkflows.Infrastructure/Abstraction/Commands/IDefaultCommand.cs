namespace CommandWorkflows.Infrastructure.Abstraction.Commands;

public interface IDefaultCommand<TRequest, TResponse>: ICommand<TRequest, TResponse>
    where TRequest: IRequest
{
    
}