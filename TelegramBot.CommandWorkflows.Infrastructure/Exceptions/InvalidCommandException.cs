namespace TelegramBot.CommandWorkflows.Infrastructure.Exceptions;

public class InvalidCommandException : Exception
{
    public InvalidCommandException(string message) 
        : base(message)
    {
    }
}