using TelegramBot.CommandWorkflows.Infrastructure.Abstraction.Commands;

namespace TelegramBot.CommandWorkflows.Infrastructure.Resolver;

public interface ICommandResolver
{
    ICommand GetCommand(string commandName);
    
    IPermanentExitCommand? GetExitCommand(string commandName);
}