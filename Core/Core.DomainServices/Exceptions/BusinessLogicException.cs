namespace Core.DomainServices.Exceptions;

/// <summary>
/// BusinessLogicException is the base class for all exceptions
/// that are thrown from the domain layer. This allows the application
/// layer to catch all exceptions and handle them in the same way.
/// </summary>
public abstract class BusinessLogicException : Exception
{
    protected BusinessLogicException(string message) : base(message)
    {
    }
}