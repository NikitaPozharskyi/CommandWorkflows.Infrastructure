using CommandWorkflows.Infrastructure.Abstraction;
using CommandWorkflows.Infrastructure.Abstraction.Commands;
using CommandWorkflows.Infrastructure.DependencyProvider;
using CommandWorkflows.Infrastructure.Exceptions;
using CommandWorkflows.Infrastructure.HistoryService;
using CommandWorkflows.Infrastructure.Resolver;
using CommandWorkflows.Infrastructure.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CommandWorkflows.Infrastructure;

public class ServiceCollectionHelper(
    IServiceCollection serviceCollection,
    Type commandType,
    ServiceLifetime commandLifetime)
{
    public IServiceCollection ServiceCollection { get; set; } = serviceCollection;

    public Type CommandType { get; set; } = commandType;

    public ServiceLifetime CommandLifetime { get; set; } = commandLifetime;
}

public static class ServiceCollectionExtensions
{
    public static void AddCommandRegistry<TUserId>(this IServiceCollection serviceCollection)
        where TUserId : notnull
    {
        serviceCollection.TryAddSingleton<ICommandHistoryService<TUserId>, CommandHistoryService<TUserId>>();
        serviceCollection.TryAddSingleton<ICommandResolver, CommandResolver>();
        serviceCollection.TryAddSingleton<ICommandClrTypeResolver, CommandClrTypeResolver>();
        serviceCollection.TryAddSingleton<IWorkflowAndCommandDependencyProvider, WorkflowAndCommandDependencyProvider>();
    }

    [Obsolete("Please use RegisterCommand<T>")]
    public static void RegisterCommand<T, TRequest, TResponse>(this IServiceCollection serviceCollection,
        string commandName)
        where TRequest : IRequest
        where T : ICommand<TRequest, TResponse>
    {
        serviceCollection.Configure<CommandAndWorkflowSettings>(
            _ => _.CommandDictionary.Add(commandName, typeof(T)));

        serviceCollection.TryAddSingleton(typeof(T));
    }

    public static ServiceCollectionHelper RegisterCommand<T>(this IServiceCollection serviceCollection, string commandName, ServiceLifetime serviceLifetime)
    {
        var commandType = typeof(T);
        var interfaces = commandType.GetInterfaces();
        var baseType = commandType.BaseType;
        if (!interfaces.Any(VerifyCommandType) || (baseType != typeof(object) && !VerifyCommandType(baseType)))
            throw new InvalidCommandException($"Cannot register command with Type {typeof(T)}");
        
        serviceCollection.Configure<CommandAndWorkflowSettings>(_ => _.CommandDictionary.Add(commandName, typeof(T)));
        
        // initialize workflows
        serviceCollection.Configure<CommandAndWorkflowSettings>(_ => _.WorkflowDictionary.Add(typeof(T), []));
        
        serviceCollection.TryAdd(new ServiceDescriptor(typeof(T), typeof(T), serviceLifetime));
        
        return new ServiceCollectionHelper(serviceCollection, typeof(T), serviceLifetime);
    }

    [Obsolete("Please use RegisterCommand<T>")]
    public static void RegisterExitCommand<T, TRequest, TResponse>(this IServiceCollection serviceCollection,
        string commandName)
        where TRequest : IRequest
        where T : ICommand<TRequest, TResponse>
    {
        serviceCollection.Configure<CommandAndWorkflowSettings>(
            _ => _.CommandDictionary.Add(commandName, typeof(T)));

        serviceCollection.TryAddSingleton(typeof(T));
    }

    [Obsolete("Please use RegisterCommand<ICommand>().RegisterWorkflow<IWorkflow>()")]
    public static void RegisterCommandWithWorkflows<T, TRequest, TResponse>(this IServiceCollection serviceCollection,
        string commandName, List<Type> workflows)
        where TRequest : IRequest
        where T : ICommand<TRequest, TResponse>
    {
        serviceCollection.Configure<CommandAndWorkflowSettings>(
            _ => _.CommandDictionary.Add(commandName, typeof(T)));

        serviceCollection.TryAddSingleton(typeof(T));

        serviceCollection.Configure<CommandAndWorkflowSettings>(_ => _.WorkflowDictionary.Add(typeof(T), workflows));
        foreach (var workflow in workflows)
        {
            serviceCollection.TryAddSingleton(workflow);
        }
    }

    public static ServiceCollectionHelper RegisterWorkflow<TWorkflow>(this ServiceCollectionHelper serviceCollectionHelper)
    {
        serviceCollectionHelper.ServiceCollection.Configure<CommandAndWorkflowSettings>(_ =>
        {
            var workflows = _.WorkflowDictionary[serviceCollectionHelper.CommandType];
            workflows.Add(typeof(TWorkflow));
        });
        serviceCollectionHelper.ServiceCollection.TryAdd(new ServiceDescriptor(typeof(TWorkflow), typeof(TWorkflow), serviceCollectionHelper.CommandLifetime));
        
        return serviceCollectionHelper;
    }
    private static bool VerifyCommandType(Type? type)
    {
        if (type.GetGenericTypeDefinition() == typeof(ICommand<,>))
        {
            return true;
        }
        
        var interfaces = type?.GetInterfaces();
        
        return interfaces != null && interfaces.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommand<,>));
    }
}