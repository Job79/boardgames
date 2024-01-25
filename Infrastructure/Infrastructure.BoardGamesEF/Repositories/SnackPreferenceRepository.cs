using Core.Domain;
using Core.DomainServices.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.BoardGamesEF.Repositories;

public class SnackPreferenceRepository : ISnackPreferenceRepository
{
    private readonly BoardGamesContext context;

    public SnackPreferenceRepository(BoardGamesContext context)
    {
        this.context = context;
    }

    public async Task<ICollection<SnackPreference>> All()
    {
        return await context.SnackPreferences.ToListAsync();
    }

    public async Task<ICollection<SnackPreference>> ByIds(ICollection<int> ids)
    {
        return await context.SnackPreferences.Where(s => ids.Contains(s.Id)).ToListAsync();
    }
}