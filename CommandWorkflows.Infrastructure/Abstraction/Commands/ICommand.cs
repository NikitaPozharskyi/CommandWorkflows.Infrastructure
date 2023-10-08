namespace CommandWorkflows.Infrastructure.Abstraction.Commands;

public interface ICommand
{
    Queue<IWorkflow> Workflows { get; set; }

    public Task<string> ExecuteAsync();
}