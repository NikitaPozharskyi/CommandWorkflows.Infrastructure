using CommandWorkflows.Infrastructure.Abstraction;
using CommandWorkflows.Infrastructure.Abstraction.Commands;

namespace CommandWorkflows.Infrastructure.Resolver;

public interface ICommandResolver
{
    ICommand<TRequest, TResponse>GetCommand<TRequest, TResponse>(string commandName, int skip = 0)
        where TRequest: IRequest;
    
    IPermanentExitCommand<TRequest, TResponse>? GetExitCommand<TRequest, TResponse>(string commandName)
        where TRequest: IRequest;
}