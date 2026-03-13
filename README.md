# CommandFlowEngine

`CommandFlowEngine` is a .NET library for stateful, command-driven flows.

It lets you:
- map a text command (for example `"start"` or `"help"`) to a command class
- attach ordered workflows to that command
- keep per-user progress in command history
- continue execution step-by-step on later messages

This is useful for chatbots, assistants, and guided multi-step interactions.

## What This Repo Contains

- `CommandFlowEngine`: core library
- `CommandFlowEngine.TestApplication`: sample console app
- `CommandFlowEngine.Tests`: NUnit tests for command execution flow

## How It Works

1. A request (`IRequest`) arrives with a `Message`.
2. The engine checks if that user already has an active command in history.
3. If no active command exists:
   - `Message` is treated as a command keyword.
   - The command is resolved and executed.
   - If workflows are configured for that command, the command is stored in history.
4. If an active command exists:
   - the next workflow in that command's queue is executed.
   - history position is advanced.
   - when all workflows are done, history is cleared for that user.
5. If the incoming message resolves to a command implementing `IPermanentExitCommand<,>`, history is cleared first.

## Core Types

```csharp
public interface IRequest
{
    string Message { get; set; }
}

public interface ICommand<TRequest, TResponse>
    where TRequest : IRequest
{
    Queue<IWorkflow<TRequest, TResponse>> Workflows { get; set; }
    Task<TResponse> ExecuteAsync(TRequest request);
}

public interface IWorkflow<in TRequest, TResponse>
    where TRequest : IRequest
{
    Task<TResponse> ExecuteAsync(TRequest message);
}
```

## Quick Start

### 1. Register engine services

```csharp
services.AddCommandRegistry<long>(ServiceLifetime.Scoped);
```

### 2. Create request/response models

```csharp
public class MyRequest : IRequest
{
    public string Message { get; set; } = string.Empty;
}

public class MyResponse
{
    public string Message { get; set; } = string.Empty;
}
```

### 3. Implement a command

```csharp
public class StartCommand : CommandAbstract<MyRequest, MyResponse>
{
    public override Task<MyResponse> ExecuteAsync(MyRequest request)
    {
        return Task.FromResult(new MyResponse { Message = "Start command executed" });
    }
}
```

### 4. Implement workflows

```csharp
public class CollectNameWorkflow : IWorkflow<MyRequest, MyResponse>
{
    public Task<MyResponse> ExecuteAsync(MyRequest message)
    {
        return Task.FromResult(new MyResponse { Message = $"Name: {message.Message}" });
    }
}
```

### 5. Register commands and workflows

```csharp
services
    .RegisterCommand<StartCommand>("start", ServiceLifetime.Scoped)
    .RegisterWorkflow<CollectNameWorkflow>();
```

### 6. Execute

```csharp
var response = await commandExecutor.ExecuteCommandAsync<MyRequest, MyResponse>(
    new MyRequest { Message = "start" },
    userId: 42);
```

## Exit Command

To define a command that clears active flow state, implement:

```csharp
IPermanentExitCommand<TRequest, TResponse>
```

If the incoming message maps to that command keyword, command history for that user is removed.

## Build And Run

```bash
dotnet build CommandWorkflows.Infrastructure.sln
dotnet test CommandWorkflows.Infrastructure.sln
dotnet run --project CommandFlowEngine.TestApplication
```

## Notes

- Current history store is in-memory: `InMemCommandHistoryService<TKey>`.
- To persist across process restarts, replace `ICommandHistoryService<TKey>` with your own implementation.

## License

See [LICENSE](LICENSE).
