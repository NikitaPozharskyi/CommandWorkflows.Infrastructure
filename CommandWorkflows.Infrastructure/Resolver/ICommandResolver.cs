using CommandWorkflows.Infrastructure.Abstraction.Commands;

namespace CommandWorkflows.Infrastructure.Resolver;

public interface ICommandResolver
{
    ICommand GetCommand(string commandName);
    
    IPermanentExitCommand? GetExitCommand(string commandName);
}