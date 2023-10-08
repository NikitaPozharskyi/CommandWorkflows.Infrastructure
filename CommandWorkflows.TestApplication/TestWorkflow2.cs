using CommandWorkflows.Infrastructure.Abstraction;

namespace CommandWorkflows.TestApplication;

public class TestWorkflow2 : IWorkflow
{
    public Task<string> ExecuteAsync(string message)
    {
        Console.WriteLine($"Executing workflow... {message}");
    
        return Task.FromResult("name");
    }
}