using Core.Domain;
using Core.DomainServices.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.BoardGamesEF.Repositories;

public class GameRepository : IGameRepository
{
    private readonly BoardGamesContext context;

    public GameRepository(BoardGamesContext context)
    {
        this.context = context;
    }

    public async Task<Game?> ById(int id)
    {
        return await context.Games.FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<ICollection<Game>> All()
    {
        return await context.Games.ToListAsync();
    }

    public async Task<ICollection<Game>> ByIds(ICollection<int> ids)
    {
        return await context.Games.Where(g => ids.Contains(g.Id)).ToListAsync();
    }

    public IQueryable<Game> Query()
    {
        return context.Games;
    }
}