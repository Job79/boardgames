using Core.Domain;

namespace Core.DomainServices.Repositories;

public interface IGameRepository
{
    /// <summary>
    /// ById fetches a single Game by its id.
    /// </summary>
    /// <param name="id">Game id</param>
    /// <returns>Game for provided id</returns>
    public Task<Game?> ById(int id);
    
    /// <summary>
    /// All fetches all Games.
    /// </summary>
    /// <returns>All Games</returns>
    public Task<ICollection<Game>> All();
    
    /// <summary>
    /// ByIds fetches all Games for the given ids.
    /// </summary>
    /// <param name="ids">List of game ids</param>
    /// <returns>Games that match the given ids</returns>
    public Task<ICollection<Game>> ByIds(ICollection<int> ids);
    
    /// <summary>
    /// Query returns a queryable for Games. Should only
    /// be used by GraphQL.
    /// </summary>
    /// <returns>IQueryable of Games</returns>
    public IQueryable<Game> Query();
}