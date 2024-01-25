namespace Core.DomainServices.Exceptions.Registration;

public class UserCantRegisterForOwnEventException : BusinessLogicException
{
    public UserCantRegisterForOwnEventException(string message) : base(message)
    {
    }
}