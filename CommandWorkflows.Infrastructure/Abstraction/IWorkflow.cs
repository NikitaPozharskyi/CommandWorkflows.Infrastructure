namespace CommandWorkflows.Infrastructure.Abstraction;

public interface IWorkflow <in TRequest, TResponse>
where TRequest: IRequest
{
    Task<TResponse> ExecuteAsync(TRequest message);
}