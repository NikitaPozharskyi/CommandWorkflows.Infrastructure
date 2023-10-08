using CommandWorkflows.TestApplication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CommandWorkflows.Infrastructure;

const string testCommand = "Test Command";
const string testCommand2 = "Test Command2";
const string exitCommand = "exit";

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddCommandRegistry<long>();
        services.RegisterExitCommand<ExitCommand>(exitCommand);
        services.RegisterCommand<TestCommand2>(testCommand2);
        services.RegisterCommandWithWorkflows<TestCommand>(testCommand, new List<Type>
        {
            typeof(TestWorkflow2),
            typeof(TestWorkflow)
        });
    
        services.AddScoped<CustomCommandExecutor>();
    })
    .Build();

var commandExecutor = host.Services.GetRequiredService<CustomCommandExecutor>();
var result2 = await commandExecutor.ExecuteCommandAsync(testCommand2, 1234);
Console.WriteLine(result2);
var result = await commandExecutor.ExecuteCommandAsync(testCommand, 1234);
Console.WriteLine(result);

// test in case if we have a workflows to execute.
await commandExecutor.ExecuteCommandAsync("Workflow message", 1234);
var exitResult = await commandExecutor.ExecuteCommandAsync(exitCommand, 1235);
Console.WriteLine(exitResult);
await commandExecutor.ExecuteCommandAsync("Workflow message 2", 1234);

await host.RunAsync();
await host.StopAsync();