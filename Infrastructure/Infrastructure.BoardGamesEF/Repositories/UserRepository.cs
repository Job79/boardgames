using Core.Domain;
using Core.DomainServices.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.BoardGamesEF.Repositories;

public class UserRepository : IUserRepository
{
    private readonly BoardGamesContext context;

    public UserRepository(BoardGamesContext context)
    {
        this.context = context;
    }

    public async Task<User?> ById(string id)
    {
        return await context.Users
            .Include(u => u.Address)
            .Include(u => u.SnackPreferences)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task Create(User gameEvent)
    {
        await context.Users.AddAsync(gameEvent);
        await context.SaveChangesAsync();
    }
}