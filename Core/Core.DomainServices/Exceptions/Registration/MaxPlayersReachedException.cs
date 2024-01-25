namespace Core.DomainServices.Exceptions.Registration;

public class MaxPlayersReachedException : BusinessLogicException
{
    public MaxPlayersReachedException(string message) : base(message)
    {
    }
}