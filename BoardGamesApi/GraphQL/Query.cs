using Core.Domain;
using Core.DomainServices.Repositories;

namespace BoardGamesApi.GraphQL;

public class Query
{
    /// <summary>
    /// GetGames returns all games.
    /// </summary>
    public IQueryable<Game> GetGames([Service] IGameRepository gameRepository) =>
        gameRepository.Query();

    /// <summary>
    /// GetGameEvents returns all game events.
    /// </summary>
    public IQueryable<GameEvent> GetGameEvents([Service] IGameEventRepository gameEventRepository) =>
        gameEventRepository.Query();
}