using CommandWorkflows.Infrastructure.Abstraction;
using CommandWorkflows.Infrastructure.Abstraction.Commands;

namespace CommandWorkflows.TestApplication;

public class TestCommand : IAdminCommand
{
    public Queue<IWorkflow> Workflows { get; set; }
    public Task<string> ExecuteAsync()
    {
        Console.WriteLine("Executing command 1...");
    
        return Task.FromResult("ssd");
    }
}