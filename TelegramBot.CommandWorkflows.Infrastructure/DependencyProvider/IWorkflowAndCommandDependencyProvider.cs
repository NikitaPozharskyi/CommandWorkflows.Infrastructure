using TelegramBot.CommandWorkflows.Infrastructure.Abstraction;
using TelegramBot.CommandWorkflows.Infrastructure.Abstraction.Commands;

namespace TelegramBot.CommandWorkflows.Infrastructure.DependencyProvider;

public interface IWorkflowAndCommandDependencyProvider
{
    public IWorkflow GetWorkflow(Type workflowType);
    
    public ICommand GetCommand(Type commandType);

}