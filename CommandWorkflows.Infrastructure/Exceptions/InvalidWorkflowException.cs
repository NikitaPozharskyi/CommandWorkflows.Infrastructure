namespace CommandWorkflows.Infrastructure.Exceptions;

public class InvalidWorkflowException : Exception
{
    public InvalidWorkflowException(string message) 
        : base(message)
    {
    }
}