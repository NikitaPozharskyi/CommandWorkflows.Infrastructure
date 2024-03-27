namespace CommandWorkflows.Infrastructure.HistoryService;

public class CommandMetadata(string commandName, int position)
{
    public string CommandName { get; set; } = commandName;
    public int Position { get; set; } = position;
}