using Core.Domain;

namespace Core.DomainServices.Repositories;

public interface ISnackPreferenceRepository
{
    /// <summary>
    /// All fetches all SnackPreferences.
    /// </summary>
    /// <returns>All SnackPreferences</returns>
    public Task<ICollection<SnackPreference>> All();
    
    /// <summary>
    /// ByIds fetches all SnackPreferences for the given ids. 
    /// </summary>
    /// <param name="ids">List of SnackPreference ids</param>
    /// <returns>SnackPreferences that match the given ids</returns>
    public Task<ICollection<SnackPreference>> ByIds(ICollection<int> ids);
}