using Microsoft.Extensions.Logging;
using CommandWorkflows.Infrastructure.CommandExecutor;
using CommandWorkflows.Infrastructure.HistoryService;
using CommandWorkflows.Infrastructure.Resolver;
using CommandWorkflows.TestApplication.Models;

namespace CommandWorkflows.TestApplication;

public class CustomCommandExecutor : CommandExecutor<long>, ICustomCommandExecutor
{
    public CustomCommandExecutor(ICommandHistoryService<long> commandHistoryService,
        ICommandResolver commandResolver, ILogger<CustomCommandExecutor> logger) : base(
        commandHistoryService, commandResolver, logger)
    {
    }

    public async Task<int> Process(string message)
    {
        var myRequest = new MyRequest
        {
            Message = message
        };

        var response = await ExecuteCommandAsync<MyRequest, int>(myRequest, 100);

        return response;
    }
}

public interface ICustomCommandExecutor
{
    Task<int> Process(string message);
}