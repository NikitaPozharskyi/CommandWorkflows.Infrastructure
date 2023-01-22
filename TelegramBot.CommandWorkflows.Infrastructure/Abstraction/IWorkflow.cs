namespace TelegramBot.CommandWorkflows.Infrastructure.Abstraction;

public interface IWorkflow
{
    Task<string> ExecuteAsync(string message);
}