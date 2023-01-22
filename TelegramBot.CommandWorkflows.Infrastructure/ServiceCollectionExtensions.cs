using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TelegramBot.CommandWorkflows.Infrastructure.DependencyProvider;
using TelegramBot.CommandWorkflows.Infrastructure.HistoryService;
using TelegramBot.CommandWorkflows.Infrastructure.Resolver;
using TelegramBot.CommandWorkflows.Infrastructure.Settings;

namespace TelegramBot.CommandWorkflows.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static void AddCommandRegistry(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ICommandClrTypeResolver, CommandClrTypeResolver>();
        serviceCollection.AddSingleton<IWorkflowAndCommandDependencyProvider, WorkflowAndCommandDependencyProvider>();
        serviceCollection.AddSingleton<ICommandHistoryService, CommandHistoryService>();
        serviceCollection.AddSingleton<ICommandResolver, CommandResolver>();
        serviceCollection.AddSingleton<ICommandTypeResolver, CommandTypeResolver>();
    }

    public static void RegisterCommand<T>(this IServiceCollection serviceCollection, string commandName)
    {
        serviceCollection.Configure<TelegramBotCommandAndWorkflowSettings>(
            _ => _.CommandDictionary.Add(commandName, typeof(T)));
        
        serviceCollection.TryAddScoped(typeof(T));
    }
    
    public static void RegisterCommandWithWorkflows<T>(this IServiceCollection serviceCollection, string commandName, List<Type>? workflows)
    {
        serviceCollection.Configure<TelegramBotCommandAndWorkflowSettings>(
            _ => _.CommandDictionary.Add(commandName, typeof(T)));
        
        serviceCollection.TryAddScoped(typeof(T));

        if (workflows != null)
        {
            serviceCollection.Configure<TelegramBotCommandAndWorkflowSettings>(_ =>
                _.WorkflowDictionary.Add(typeof(T), workflows));
            foreach (var workflow in workflows)
            {
                serviceCollection.TryAddScoped(workflow);
            }
        }
    }
}