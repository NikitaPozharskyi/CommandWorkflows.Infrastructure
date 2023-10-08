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
        serviceCollection.AddSingleton<ICommandClrTypeResolver, CommandClrTypeResolver>();
        serviceCollection.AddSingleton<ICommandHistoryService<TUserId>, CommandHistoryService<TUserId>>();
        serviceCollection.AddSingleton<IWorkflowAndCommandDependencyProvider, WorkflowAndCommandDependencyProvider>();
        serviceCollection.AddSingleton<ICommandResolver, CommandResolver>();
        serviceCollection.AddSingleton<ICommandTypeResolver, CommandTypeResolver>();
    }

    public static void RegisterCommand<T>(this IServiceCollection serviceCollection, string commandName)
        where T : ICommand
    {
        serviceCollection.Configure<TelegramBotCommandAndWorkflowSettings>(
            _ => _.CommandDictionary.Add(commandName, typeof(T)));

        serviceCollection.TryAddScoped(typeof(T));
    }

    public static void RegisterExitCommand<T>(this IServiceCollection serviceCollection, string commandName)
        where T : ICommand
    {
        serviceCollection.Configure<TelegramBotCommandAndWorkflowSettings>(
            _ => _.CommandDictionary.Add(commandName, typeof(T)));

        serviceCollection.TryAddScoped(typeof(T));
    }

    
    public static void RegisterCommandWithWorkflows<T>(this IServiceCollection serviceCollection, string commandName,
        List<Type> workflows)
        where T : ICommand
    {
        serviceCollection.Configure<TelegramBotCommandAndWorkflowSettings>(
            _ => _.CommandDictionary.Add(commandName, typeof(T)));

        serviceCollection.TryAddScoped(typeof(T));

        serviceCollection.Configure<TelegramBotCommandAndWorkflowSettings>(_ =>
            _.WorkflowDictionary.Add(typeof(T), workflows));
        foreach (var workflow in workflows)
        {
            serviceCollection.TryAddScoped(workflow);
        }
    }
}