using Core.Domain;
using Core.DomainServices.Exceptions.Registration;
using Core.DomainServices.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.BoardGamesEF.Repositories;

public class RegistrationRepository : IRegistrationRepository
{
    private readonly BoardGamesContext context;

    public RegistrationRepository(BoardGamesContext context)
    {
        this.context = context;
    }

    public Task<Registration?> ById(int id)
    {
        return context.Registrations
            .Include(r => r.User)
            .Include(r => r.GameEvent)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<ICollection<Registration>> ByGameEvent(int gameEventId)
    {
        return await context.Registrations
            .Include(r => r.User)
            .Where(r => r.GameEvent.Id == gameEventId)
            .ToListAsync();
    }

    public async Task<Dictionary<string, Tuple<int, int>>> CalculateAttendanceForUsers(ICollection<string> userIds)
    {
        return await context.Registrations
            .Where(r => userIds.Contains(r.User.Id))
            .GroupBy(r => r.User.Id)
            .ToDictionaryAsync(g => g.Key, g => new Tuple<int, int>(
                g.Count(r => r.DidAttend == true),
                g.Count(r => r.DidAttend == false)));
    }

    public async Task Create(Registration registration)
    {
        if (registration.GameEvent.Organizer.Id == registration.User.Id)
        {
            throw new UserCantRegisterForOwnEventException("Je kunt je niet inschrijven voor je eigen evenement");
        }

        if (registration.GameEvent.Is18Plus && !registration.User.Is18Plus)
        {
            throw new UserIsNot18PlusException(
                "Je kunt je niet inschrijven voor een 18+ evenement als je nog geen 18 bent");
        }

        await using var transaction = await context.Database.BeginTransactionAsync();
        if (await checkUserAlreadyRegisteredForEvent(registration))
        {
            throw new UserIsAlreadyRegisteredException("Je bent al ingeschreven voor dit evenement");
        }

        if (await checkUserHasRegisteredOnSameDate(registration))
        {
            throw new UserHasAlreadyRegisteredOnSameDateException(
                "Je bent al ingeschreven voor een ander evenement op dezelfde dag");
        }

        if (await checkRegistrationHasReachedMaxPlayers(registration))
        {
            throw new MaxPlayersReachedException("Het maximale aantal inschrijvingen voor dit evenement is al bereikt");
        }

        await context.Registrations.AddAsync(registration);
        await context.SaveChangesAsync();
        await transaction.CommitAsync();
    }

    public Task Update(Registration registration)
    {
        context.Registrations.Update(registration);
        return context.SaveChangesAsync();
    }

    private Task<bool> checkUserAlreadyRegisteredForEvent(Registration registration)
        => context.Registrations.AnyAsync(r =>
            r.User.Id == registration.User.Id && r.GameEvent.Id == registration.GameEvent.Id);

    private Task<bool> checkUserHasRegisteredOnSameDate(Registration registration)
        => context.Registrations.AnyAsync(r =>
            r.User.Id == registration.User.Id && r.GameEvent.DateTime.Date == registration.GameEvent.DateTime.Date);

    private async Task<bool> checkRegistrationHasReachedMaxPlayers(Registration registration)
        => await context.Registrations.CountAsync(r => r.GameEvent.Id == registration.GameEvent.Id) >=
           registration.GameEvent.MaxPlayers;
}