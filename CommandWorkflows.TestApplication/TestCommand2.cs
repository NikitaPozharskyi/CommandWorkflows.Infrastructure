using CommandWorkflows.Infrastructure.Abstraction;
using CommandWorkflows.Infrastructure.Abstraction.Commands;
using CommandWorkflows.TestApplication.Models;

public class TestCommand2 : IDefaultCommand <MyRequest, MyResponse>
{
    public Queue<IWorkflow<MyRequest, MyResponse>> Workflows { get; set; }
    
    public IEnumerable<Type> ExecutableWorkflows { get; set; }

    public Task<MyResponse> ExecuteAsync(MyRequest request)
    {
        return Task.FromResult(new MyResponse
        {
            Message = "TestCommand2 executed"
        });
    }
}