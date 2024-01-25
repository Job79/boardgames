using Core.Domain;
using Core.DomainServices.Exceptions;
using Core.DomainServices.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.BoardGamesEF.Repositories;

public class GameEventRepository : IGameEventRepository
{
    private readonly BoardGamesContext context;

    public GameEventRepository(BoardGamesContext context)
    {
        this.context = context;
    }

    public async Task<GameEvent?> ById(int id)
    {
        return await context.GameEvents
            .Include(g => g.Organizer)
            .Include(g => g.Address)
            .Include(g => g.Games)
            .Include(g => g.Registrations)
            .Include(g => g.AvailableSnacks)
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<ICollection<GameEvent>> All()
    {
        return await context.GameEvents
            .Include(g => g.Address)
            .Include(g => g.Organizer)
            .OrderBy(g => g.DateTime)
            .ToListAsync();
    }

    public async Task<ICollection<GameEvent>> ByOrganiser(string userId)
    {
        return await context.GameEvents
            .Where(g => g.Organizer.Id == userId)
            .Include(g => g.Address)
            .Include(g => g.Organizer)
            .OrderBy(g => g.DateTime)
            .ToListAsync();
    }

    public async Task<ICollection<GameEvent>> ByRegistrations(string userId)
    {
        return await context.GameEvents
            .Where(g => g.Registrations.Any(r => r.UserId == userId))
            .Include(g => g.Address)
            .Include(g => g.Organizer)
            .OrderBy(g => g.DateTime)
            .ToListAsync();
    }
    
    public IQueryable<GameEvent> Query()
    {
        return context.GameEvents
            .Include(g => g.Organizer)
            .Include(g => g.Address)
            .Include(g => g.Games)
            .Include(g => g.AvailableSnacks);
    }

    public async Task Create(GameEvent gameEvent)
    {
        if (!gameEvent.Organizer.Is18Plus)
        {
            throw new UnauthorizedOperationException("Spelavond kan niet aangemaakt worden door een minderjarige");
        }
        
        gameEvent.Is18Plus = gameEvent.Is18Plus || gameEvent.Games.Any(g => g.Is18Plus);
        await context.GameEvents.AddAsync(gameEvent);
        await context.SaveChangesAsync();
    }

    public async Task Update(GameEvent gameEvent)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        if (await checkGameEventAlreadyHasRegistrations(gameEvent))
        {
            throw new UnauthorizedOperationException(
                "Spelavond kan niet meer gewijzigd worden omdat er al aanmeldingen zijn");
        }
        
        gameEvent.Is18Plus = gameEvent.Is18Plus || gameEvent.Games.Any(g => g.Is18Plus);
        context.Update(gameEvent);
        await context.SaveChangesAsync();
        await transaction.CommitAsync();
    }

    public async Task Delete(GameEvent gameEvent)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        if (await checkGameEventAlreadyHasRegistrations(gameEvent))
        {
            throw new UnauthorizedOperationException(
                "Spelavond kan niet meer verwijderd worden omdat er al aanmeldingen zijn");
        }

        context.Remove(gameEvent);
        await context.SaveChangesAsync();
        await transaction.CommitAsync();
    }

    private Task<bool> checkGameEventAlreadyHasRegistrations(GameEvent gameEvent)
        => context.Registrations.AnyAsync(r => r.GameEvent == gameEvent);
}