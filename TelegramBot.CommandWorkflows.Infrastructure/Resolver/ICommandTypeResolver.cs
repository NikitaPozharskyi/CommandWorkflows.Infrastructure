using TelegramBot.CommandWorkflows.Infrastructure.Abstraction.Commands;
using TelegramBot.CommandWorkflows.Infrastructure.Abstraction.Enums;

namespace TelegramBot.CommandWorkflows.Infrastructure.Resolver;

public interface ICommandTypeResolver
{
    CommandType GetCommandType(ICommand command, Func<ICommand, CommandType> typeSelector);

    CommandType GetCommandType(string commandName, Func<ICommand, CommandType> typeSelector);
}