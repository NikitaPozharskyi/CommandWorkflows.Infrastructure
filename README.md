# CommandWorkflows.Infrastructure
Command workflow pattern for external providers that implement exchanging messages 'one by one'

You can use it with any provider that uses 'one-by-one' messaging to
recognize and make the system remember previous calls to save commands execution chain

In other words, it is the library that makes the application stateful and can save intermediate states between calls.
The provider can be a TelegramBot. It is known as a non-scalable application.

## Example of using the library
Here, I create a simple console application and simulate 'one-by-one' messaging to test this library.
First of all, it should use dependency injection. It won't work without it.
We need to create a command that implements ICommand or is related to ICommand interfaces.

IAdminCommand, ISuperAdminCommand and IUserCommand

The following command looks like:

    public class TestCommand : IAdminCommand
    {
        public TestCommand()
        {
            // you can put any registered service from DI here.
            // Example: _customService = customService (from constructor)
        }
        public Queue<IWorkflow> Workflows { get; set; }
        
        public Task<string> ExecuteAsync()
        {
            Console.WriteLine("Executing command...");
        
            return Task.FromResult("ssd");
        }
    }
As you can see, we just implement the interface function and create workflows.
variable that is needed for related workflows if they are existing.

Then we can create workflows if this command has any.
The key relation is command->step-by step workflows to execute.

Workflows look like this:

    public class TestWorkflow : IWorkflow
    {
        public Task<string> ExecuteAsync(string message)
        {
            Console.WriteLine($"Executing workflow... {message}");
            
            return Task.FromResult(message);
        }
    }
    
    public class TestWorkflow2 : IWorkflow
    {
        public readonly ICustomHttpClient _client;

        public TestWorkflow2(ICustomHttpClient client)
        {
            // you can put any registered service here.
            _client = client  
        }
        public Task<string> ExecuteAsync(string message)
        {
            Console.WriteLine($"Executing workflow... {message}");
        
            return Task.FromResult("name");
        }
    }

We're just implementing another interface, and that's it. We can add some services inside of
Command or Workflow because they are created via dependency injection and all of the services
inside the constructors will be resolved.

The Program.cs file looks like:

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
    }).Build();

    var commandExecutor = host.Services.GetRequiredService<CustomCommandExecutor>();
    var result = await commandExecutor.ExecuteCommandAsync(testCommand, 1234);
    Console.WriteLine(result);
    var result2 = await commandExecutor.ExecuteCommandAsync(testCommand2, 1234);
    Console.WriteLine(result2);
    
    // test in case if we have a workflows to execute.
    await commandExecutor.ExecuteCommandAsync("Workflow message", 1234);
    var exitResult = await commandExecutor.ExecuteCommandAsync(exitCommand, 1235);
    Console.WriteLine(exitResult);
    await commandExecutor.ExecuteCommandAsync("Workflow message 2", 1234);
    
    await host.StartAsync();
    

As you can see, to link Command and Workflow, we just need to integrate them in the ServiceCollection Extension =>
RegisterCommandWithWorkflows<TestCommand>(...). This was implemented for flexibility, so you can reuse your workflows.
if some commands contain repeatable workflows.

The rest of the Commands using in the Program.cs:

    public class TestCommand2 : ICommand
    {
        public Queue<IWorkflow> Workflows { get; set; }

        public Task<string> ExecuteAsync()
        {
            return Task.FromResult("Command 2 Executing");
        }
    }
    
    public class ExitCommand : IPermanentExitCommand
    {
        public Queue<IWorkflow> Workflows { get; set; }
    
        public Task<string> ExecuteAsync()
        {
            return Task.FromResult("Exited");
        }
    }
