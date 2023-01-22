using TelegramBot.CommandWorkflows.Infrastructure.Abstraction;

namespace TelegramBot.CommandWorkflows.Infrastructure.CommandExecutor;

public interface ICommandExecutor
{
    Task<string> ExecuteCommand(string text, long userId);

    IWorkflow? LastExecutableWorkflow { get; set; }
}