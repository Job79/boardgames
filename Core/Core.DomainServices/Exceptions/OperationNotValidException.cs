namespace Core.DomainServices.Exceptions;

/// <summary>
/// OperationNotValidException is thrown when a user tries to perform
/// an invalid operation.
/// </summary>
public class OperationNotValidException : BusinessLogicException
{
    public OperationNotValidException(string message) : base(message)
    {
    }
}