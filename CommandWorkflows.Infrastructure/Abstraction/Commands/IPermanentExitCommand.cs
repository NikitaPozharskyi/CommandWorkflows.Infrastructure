namespace CommandWorkflows.Infrastructure.Abstraction.Commands;

public interface IPermanentExitCommand<TRequest, TResponse> : ICommand<TRequest, TResponse>
where TRequest: IRequest;