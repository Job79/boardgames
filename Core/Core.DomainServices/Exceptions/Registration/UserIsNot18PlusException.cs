namespace Core.DomainServices.Exceptions.Registration;

public class UserIsNot18PlusException : BusinessLogicException
{
    public UserIsNot18PlusException(string message) : base(message)
    {
    }
}