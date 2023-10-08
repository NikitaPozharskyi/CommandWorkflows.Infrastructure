using CommandWorkflows.Infrastructure.Abstraction.Commands;
using CommandWorkflows.Infrastructure.Abstraction.Enums;

namespace CommandWorkflows.Infrastructure.Resolver;

public interface ICommandTypeResolver
{
    CommandType GetCommandType(ICommand command, Func<ICommand, CommandType> typeSelector);

    CommandType GetCommandType(string commandName, Func<ICommand, CommandType> typeSelector);
}