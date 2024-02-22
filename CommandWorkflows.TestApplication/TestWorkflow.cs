using CommandWorkflows.Infrastructure.Abstraction;
using CommandWorkflows.TestApplication.Models;

namespace CommandWorkflows.TestApplication;

public class TestWorkflow : IWorkflow<MyRequest, MyResponse>
{
    public Task<MyResponse> ExecuteAsync(MyRequest message)
    {
        Console.WriteLine($"Executing workflow... {message.Message}");
        
        return Task.FromResult(new MyResponse
        {
            Message = message.Message
        });
    }
}