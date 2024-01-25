namespace Core.DomainServices.Exceptions;

/// <summary>
/// UnauthorizedOperationException is thrown when a user tries to perform
/// an operation that they are not authorized to perform.
/// </summary>
public class UnauthorizedOperationException : BusinessLogicException
{
    public UnauthorizedOperationException(string message) : base(message)
    {
    }
}