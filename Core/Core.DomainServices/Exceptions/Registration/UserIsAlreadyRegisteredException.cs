namespace Core.DomainServices.Exceptions.Registration;

public class UserIsAlreadyRegisteredException : BusinessLogicException
{
    public UserIsAlreadyRegisteredException(string message) : base(message)
    {
    }
}