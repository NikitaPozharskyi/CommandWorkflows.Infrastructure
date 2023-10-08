using CommandWorkflows.Infrastructure.Abstraction.Commands;
using CommandWorkflows.Infrastructure.Abstraction.Enums;

namespace CommandWorkflows.Infrastructure.Resolver;

public class CommandTypeResolver : ICommandTypeResolver
{
    private readonly ICommandResolver _commandResolver;

    public CommandTypeResolver(ICommandResolver commandResolver)
    {
        _commandResolver = commandResolver;
    }
    
    public virtual CommandType GetCommandType(ICommand command, Func<ICommand, CommandType> typeSelector) => command switch
    {
        IAdminCommand => CommandType.AdminCommand,
        ISuperAdminCommand => CommandType.SuperAdminCommand,
        IDefaultCommand => CommandType.DefaultCommand,
        _ => typeSelector(command)
    };
    
    public virtual CommandType GetCommandType(string commandName, Func<ICommand, CommandType> typeSelector)
    {
        var command = _commandResolver.GetCommand(commandName);

        return command switch
        {
            IAdminCommand => CommandType.AdminCommand,
            ISuperAdminCommand => CommandType.SuperAdminCommand,
            IDefaultCommand => CommandType.DefaultCommand,
            _ => typeSelector(command)
        };
    }
}