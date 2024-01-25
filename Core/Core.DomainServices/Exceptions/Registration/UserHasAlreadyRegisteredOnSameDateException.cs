namespace Core.DomainServices.Exceptions.Registration;

public class UserHasAlreadyRegisteredOnSameDateException : BusinessLogicException
{
    public UserHasAlreadyRegisteredOnSameDateException(string message) : base(message)
    {
    }
}