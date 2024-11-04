using CommandWorkflows.Infrastructure.Abstraction.Commands;
using CommandWorkflows.Infrastructure.DependencyProvider;
using CommandWorkflows.Infrastructure.Exceptions;
using CommandWorkflows.Infrastructure.HistoryService;
using CommandWorkflows.Infrastructure.Resolver;
using CommandWorkflows.Infrastructure.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CommandWorkflows.Infrastructure.Extensions;

public record ServiceCollectionHelper(IServiceCollection ServiceCollection, Type CommandType, ServiceLifetime CommandLifetime);

public static class ServiceCollectionExtensions
{
    public static void AddCommandRegistry<TUserId>(this IServiceCollection serviceCollection, ServiceLifetime commandResolverLifetime)
        where TUserId : notnull
    {
        serviceCollection.TryAddSingleton<ICommandHistoryService<TUserId>, CommandHistoryService<TUserId>>();
        serviceCollection.TryAddSingleton<ICommandClrTypeResolver, CommandClrTypeResolver>();
        serviceCollection.TryAdd(new ServiceDescriptor(typeof(ICommandResolver), typeof(CommandResolver), commandResolverLifetime));
        serviceCollection.TryAdd(new ServiceDescriptor(typeof(IWorkflowAndCommandDependencyProvider), typeof(WorkflowAndCommandDependencyProvider), commandResolverLifetime));
    }

    public static ServiceCollectionHelper RegisterCommand<T>(this IServiceCollection serviceCollection, string commandName, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
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
        if (type!.GetGenericTypeDefinition() == typeof(ICommand<,>))
        {
            return true;
        }
        
        var interfaces = type?.GetInterfaces();
        
        return interfaces != null && interfaces.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommand<,>));
    }
}