using CommandWorkflows.Infrastructure.Abstraction.Commands;
using CommandWorkflows.TestApplication.Models;

namespace CommandWorkflows.TestApplication;

public class TestCommand : CommandAbstract<MyRequest, MyResponse>
{
    public override Task<MyResponse> ExecuteAsync(MyRequest request)
    {
        Console.WriteLine("Executing command 1...");
    
        return Task.FromResult(new MyResponse
        {
            Message = "TestCommand Executed"
        });
    }
}