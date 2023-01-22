namespace TelegramBot.CommandWorkflows.Infrastructure.Exceptions;

public class InvalidCommandTypeException : Exception
{
    public InvalidCommandTypeException(string message) 
        : base(message)
    {
    }
}