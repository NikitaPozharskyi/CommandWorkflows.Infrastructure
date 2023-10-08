using Microsoft.Extensions.Logging;
using CommandWorkflows.Infrastructure.CommandExecutor;
using CommandWorkflows.Infrastructure.HistoryService;
using CommandWorkflows.Infrastructure.Resolver;

namespace CommandWorkflows.TestApplication;

public class CustomCommandExecutor : CommandExecutor<long>
{
    public CustomCommandExecutor(ICommandHistoryService<long> commandHistoryService, ICommandResolver commandResolver, ILogger<CustomCommandExecutor> logger) : base(commandHistoryService, commandResolver, logger)
    {
    }
}