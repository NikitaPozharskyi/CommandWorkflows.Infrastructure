using CommandWorkflows.Infrastructure.Abstraction;

namespace CommandWorkflows.Infrastructure.CommandExecutor;

public interface ICommandExecutor<in TKey>
where TKey: notnull
{
    Task<string> ExecuteCommandAsync(string text, TKey userId);

    IWorkflow? LastExecutableWorkflow { get; set; }
}