using CommandWorkflows.Infrastructure.Abstraction;
using CommandWorkflows.Infrastructure.Abstraction.Commands;
using CommandWorkflows.TestApplication.Models;

namespace CommandWorkflows.TestApplication;

public class TestCommand : IAdminCommand<MyRequest, MyResponse>
{
    public Queue<IWorkflow<MyRequest, MyResponse>> Workflows { get; set; }
    public Task<MyResponse> ExecuteAsync()
    {
        Console.WriteLine("Executing command 1...");
    
        return Task.FromResult(new MyResponse
        {
            Message = "TestCommand Executed"
        });
    }
}