using CommandWorkflows.Infrastructure.Abstraction;
using CommandWorkflows.Infrastructure.Abstraction.Commands;
using CommandWorkflows.TestApplication.Models;

public class ExitCommand : IPermanentExitCommand<MyRequest, MyResponse>
{
    public Queue<IWorkflow<MyRequest, MyResponse>> Workflows { get; set; }
    
    public Task<MyResponse> ExecuteAsync(MyRequest request)
    {
        return Task.FromResult(new MyResponse
        {
            Message = "Exit command executed"
        });
    }
}