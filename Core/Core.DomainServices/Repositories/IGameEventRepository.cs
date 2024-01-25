using Core.Domain;

namespace Core.DomainServices.Repositories;

public interface IGameEventRepository
{
    /// <summary>
    /// ById fetches a single GameEvent by its id.
    /// </summary>
    /// <param name="id">GameEvent id</param>
    /// <returns>GameEvent for provided id</returns>
    public Task<GameEvent?> ById(int id);

    /// <summary>
    /// All fetches all GameEvents ordered by DateTime.
    /// </summary>
    /// <returns>All GameEvents</returns>
    public Task<ICollection<GameEvent>> All();

    /// <summary>
    /// ByOrganiser fetches all GameEvents for a given
    /// organiser ordered by DateTime.
    /// </summary>
    /// <param name="userId">Id of organiser</param>
    /// <returns>GameEvents for given organiser</returns>
    public Task<ICollection<GameEvent>> ByOrganiser(string userId);

    /// <summary>
    /// ByRegistrations fetches all GameEvents for which a
    /// given user has registered ordered by DateTime.
    /// </summary>
    /// <param name="userId">Id of user</param>
    /// <returns>GameEvents for which the given user has registered</returns>
    public Task<ICollection<GameEvent>> ByRegistrations(string userId);

    /// <summary>
    /// Query returns a queryable for GameEvents. Should only
    /// be used by GraphQL.
    /// </summary>
    /// <returns>IQueryable of GameEvents</returns>
    public IQueryable<GameEvent> Query();

    /// <summary>
    /// Create creates a new GameEvent.
    /// </summary>
    /// <param name="gameEvent">A new GameEvent</param>
    public Task Create(GameEvent gameEvent);

    /// <summary>
    /// Update updates an existing GameEvent.
    /// </summary>
    /// <param name="gameEvent">An existing GameEvent</param>
    public Task Update(GameEvent gameEvent);

    /// <summary>
    /// Delete deletes an existing GameEvent.
    /// </summary>
    /// <param name="gameEvent">An existing GameEvent</param>
    public Task Delete(GameEvent gameEvent);
}