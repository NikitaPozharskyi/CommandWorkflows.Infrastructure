using CommandWorkflows.Infrastructure.Abstraction;
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
    
    public virtual CommandType GetCommandType<TRequest, TResponse>(ICommand<TRequest, TResponse> command, Func<ICommand<TRequest, TResponse>, CommandType> typeSelector) where TRequest: IRequest => command switch
    {
        IAdminCommand<TRequest, TResponse> => CommandType.AdminCommand,
        ISuperAdminCommand<TRequest, TResponse> => CommandType.SuperAdminCommand,
        IDefaultCommand<TRequest, TResponse> => CommandType.DefaultCommand,
        _ => typeSelector(command)
    };
    
    public virtual CommandType GetCommandType<TRequest, TResponse>(string commandName, Func<ICommand<TRequest, TResponse>, CommandType> typeSelector) where TRequest: IRequest
    {
        var command = _commandResolver.GetCommand<TRequest, TResponse>(commandName);

        return command switch
        {
            IAdminCommand<TRequest, TResponse> => CommandType.AdminCommand,
            ISuperAdminCommand<TRequest, TResponse> => CommandType.SuperAdminCommand,
            IDefaultCommand<TRequest, TResponse> => CommandType.DefaultCommand,
            _ => typeSelector(command)
        };
    }
}