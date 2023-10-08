using CommandWorkflows.Infrastructure.Abstraction;
using CommandWorkflows.Infrastructure.Abstraction.Commands;

public class TestCommand2 : IDefaultCommand
{
    public Queue<IWorkflow> Workflows { get; set; }
    
    public IEnumerable<Type> ExecutableWorkflows { get; set; }

    public Task<string> ExecuteAsync()
    {
        return Task.FromResult("Command 2 Executing");
    }
}