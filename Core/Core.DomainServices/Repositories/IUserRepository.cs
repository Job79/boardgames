using Core.Domain;

namespace Core.DomainServices.Repositories;

public interface IUserRepository
{
    /// <summary>
    /// ById fetches a single User by its id.
    /// </summary>
    /// <param name="id">User id</param>
    /// <returns>User for provided id</returns>
    public Task<User?> ById(string id);
    
    /// <summary>
    /// Create creates a new User.
    /// </summary>
    /// <param name="gameEvent">A new User</param>
    public Task Create(User gameEvent);
}