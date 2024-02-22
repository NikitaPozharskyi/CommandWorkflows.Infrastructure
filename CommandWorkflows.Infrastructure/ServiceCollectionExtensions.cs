using CommandWorkflows.Infrastructure.Abstraction;
using CommandWorkflows.Infrastructure.Abstraction.Commands;
using CommandWorkflows.Infrastructure.DependencyProvider;
using CommandWorkflows.Infrastructure.HistoryService;
using CommandWorkflows.Infrastructure.Resolver;
using CommandWorkflows.Infrastructure.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CommandWorkflows.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static void AddCommandRegistry<TUserId>(this IServiceCollection serviceCollection) 
        where TUserId : notnull
    {
        serviceCollection.TryAddSingleton<ICommandHistoryService<TUserId>, CommandHistoryService<TUserId>>();
        serviceCollection.TryAddSingleton<ICommandResolver, CommandResolver>();
        serviceCollection.TryAddSingleton<ICommandTypeResolver, CommandTypeResolver>();
        serviceCollection.TryAddSingleton<ICommandClrTypeResolver, CommandClrTypeResolver>();
        serviceCollection.TryAddSingleton<IWorkflowAndCommandDependencyProvider, WorkflowAndCommandDependencyProvider>();
    }

    public static void RegisterCommand<T, TRequest, TResponse>(this IServiceCollection serviceCollection, string commandName)
        where TRequest: IRequest
        where T : ICommand<TRequest, TResponse>
    {
        serviceCollection.Configure<CommandAndWorkflowSettings>(
            _ => _.CommandDictionary.Add(commandName, typeof(T)));

        serviceCollection.TryAddScoped(typeof(T));
    }

    public static void RegisterExitCommand<T, TRequest, TResponse>(this IServiceCollection serviceCollection, string commandName)
        where TRequest: IRequest
        where T : ICommand<TRequest, TResponse>
    {
        serviceCollection.Configure<CommandAndWorkflowSettings>(
            _ => _.CommandDictionary.Add(commandName, typeof(T)));

        serviceCollection.TryAddScoped(typeof(T));
    }

    
    public static void RegisterCommandWithWorkflows<T, TRequest, TResponse>(this IServiceCollection serviceCollection, string commandName,
        List<Type> workflows)
        where TRequest: IRequest
        where T : ICommand<TRequest, TResponse>
    {
        serviceCollection.Configure<CommandAndWorkflowSettings>(
            _ => _.CommandDictionary.Add(commandName, typeof(T)));

        serviceCollection.TryAddScoped(typeof(T));

        serviceCollection.Configure<CommandAndWorkflowSettings>(_ =>
            _.WorkflowDictionary.Add(typeof(T), workflows));
        foreach (var workflow in workflows)
        {
            serviceCollection.TryAddScoped(workflow);
        }
    }
}