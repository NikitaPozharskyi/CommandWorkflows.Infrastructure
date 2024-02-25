namespace CommandWorkflows.Infrastructure.Abstraction.Commands;

public interface ICommand<TRequest, TResponse>
where TRequest: IRequest
{
    Queue<IWorkflow<TRequest, TResponse>> Workflows { get; set; }

    public Task<TResponse> ExecuteAsync(TRequest request);
}