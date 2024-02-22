using CommandWorkflows.TestApplication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CommandWorkflows.Infrastructure;
using CommandWorkflows.Infrastructure.Extensions;
using CommandWorkflows.TestApplication.Models;

const string testCommand = "Test Command";
const string testCommand2 = "Test Command2";
const string testCommand3 = "Test Command3";
const string exitCommand = "exit";

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddCommandRegistry<long>();
        services.RegisterCommand<TestCommand3, MyCustomRequest, MyCustomResponse>(testCommand3);
        services.RegisterExitCommand<ExitCommand, MyRequest, MyResponse>(exitCommand);
        services.RegisterCommand<TestCommand2, MyRequest, MyResponse>(testCommand2);
        services.RegisterCommandWithWorkflows<TestCommand, MyRequest, MyResponse>(testCommand, new List<Type>
        {
            typeof(TestWorkflow),
            typeof(TestWorkflow2)
        });
    
        services.AddScoped<CustomCommandExecutor>();
    })
    .Build();

var commandExecutor = host.Services.GetRequiredService<CustomCommandExecutor>();

var result2 = await commandExecutor.ExecuteCommandAsync<MyRequest, MyResponse>(new MyRequest
{
    Message = testCommand2
}, 1234);
Console.WriteLine(result2.Message);

var result3 = await commandExecutor.ExecuteCommandAsync<MyCustomRequest, MyCustomResponse>(new MyCustomRequest
{
    Message = testCommand3
}, 1234);

Console.WriteLine(result3.Message);

var result = await commandExecutor.ExecuteCommandAsync<MyRequest, MyResponse>(new MyRequest
{
    Message = testCommand
}, 1234);
Console.WriteLine(result.Message);



// test in case if we have a workflows to execute.
await commandExecutor.ExecuteCommandAsync<MyRequest, MyResponse>(new MyRequest
{
    Message = "Workflow message"
}, 1234);

// var exitResult = await commandExecutor.ExecuteCommandAsync(new MyRequest
// {
//     Message = exitCommand
// }, 1234);
//
// Console.WriteLine(exitResult.Message);

await commandExecutor.ExecuteCommandAsync<MyRequest, MyResponse>(new MyRequest
{
    Message = "Workflow message 2"
}, 1234);

await host.StartAsync();
await host.StopAsync();

var list = new List<int>
{
    1,
    2,
    3,
    4
};

var queue = list.ToQueue();
Console.WriteLine(queue.Peek());