using Microsoft.Extensions.Logging;
using TelegramBot.CommandWorkflows.Infrastructure.CommandExecutor;
using TelegramBot.CommandWorkflows.Infrastructure.HistoryService;
using TelegramBot.CommandWorkflows.Infrastructure.Resolver;

namespace CommandWorkflows.TestApplication;

public class CustomCommandExecutor : CommandExecutor
{
    public CustomCommandExecutor(ICommandHistoryService commandHistoryService, ICommandResolver commandResolver, ILogger<CustomCommandExecutor> logger) : base(commandHistoryService, commandResolver, logger)
    {
    }
}