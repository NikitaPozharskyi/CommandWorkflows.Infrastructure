namespace CommandWorkflows.Infrastructure.HistoryService;

public class CommandMetadata(int position, Type commandType)
{
    public int Position { get; set; } = position;
    
    public Type CommandType { get; } = commandType;
}