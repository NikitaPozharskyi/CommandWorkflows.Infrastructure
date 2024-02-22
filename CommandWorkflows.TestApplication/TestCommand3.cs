using CommandWorkflows.Infrastructure.Abstraction;
using CommandWorkflows.Infrastructure.Abstraction.Commands;

namespace CommandWorkflows.TestApplication;

public class TestCommand3 : IDefaultCommand<MyCustomRequest, MyCustomResponse>
{
    public Queue<IWorkflow<MyCustomRequest, MyCustomResponse>> Workflows { get; set; }
    public Task<MyCustomResponse> ExecuteAsync()
    {
        return Task.FromResult<MyCustomResponse>(new MyCustomResponse
        {
            Message = "TestCommand 3 Custom request/response"
        });
    }
}

public class MyCustomRequest : IRequest
{
    public string Message { get; set; }
}

public class MyCustomResponse
{
    public string Message { get; set; }
}