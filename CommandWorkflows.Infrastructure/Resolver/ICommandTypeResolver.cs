using CommandWorkflows.Infrastructure.Abstraction;
using CommandWorkflows.Infrastructure.Abstraction.Commands;
using CommandWorkflows.Infrastructure.Abstraction.Enums;

namespace CommandWorkflows.Infrastructure.Resolver;

public interface ICommandTypeResolver
{
    CommandType GetCommandType<TRequest, TResponse>(ICommand<TRequest, TResponse> command, Func<ICommand<TRequest, TResponse>, CommandType> typeSelector) where TRequest: IRequest;

    CommandType GetCommandType<TRequest, TResponse>(string commandName, Func<ICommand<TRequest, TResponse>, CommandType> typeSelector) where TRequest: IRequest;
}