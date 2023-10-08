using CommandWorkflows.Infrastructure.Abstraction;
using CommandWorkflows.Infrastructure.Abstraction.Commands;

public class ExitCommand : IPermanentExitCommand
{
    public Queue<IWorkflow> Workflows { get; set; }
    
    public Task<string> ExecuteAsync()
    {
        return Task.FromResult("Exited");
    }
}