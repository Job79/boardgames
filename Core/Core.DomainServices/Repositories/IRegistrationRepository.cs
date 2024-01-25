using Core.Domain;

namespace Core.DomainServices.Repositories;

public interface IRegistrationRepository
{
    /// <summary>
    /// ById fetches a single registration by its id.
    /// </summary>
    /// <param name="id">Registration Id</param>
    /// <returns>Registration for provided id</returns>
    public Task<Registration?> ById(int id);
    
    /// <summary>
    /// ByGameEvent fetches all registrations for a given GameEvent.
    /// </summary>
    /// <param name="gameEventId">GameEvent id</param>
    /// <returns>All registrations for given GameEvent</returns>
    public Task<ICollection<Registration>> ByGameEvent(int gameEventId);
    
    /// <summary>
    /// CalculateAttendanceForUsers calculates the attendance
    /// for all given users. It returns a dictionary with the
    /// User id as key and a Tuple{AttendanceCount: int, NonAttendanceCount: int}
    /// as value.
    /// </summary>
    /// <param name="userIds">User ids for which to calculate the attendance</param>
    /// <returns>Dictionary with attendance of users</returns>
    public Task<Dictionary<string, Tuple<int, int>>> CalculateAttendanceForUsers(ICollection<string> userIds);
    
    /// <summary>
    /// Create creates a new registration.
    /// </summary>
    /// <param name="registration">A new registration</param>
    public Task Create(Registration registration);
    
    /// <summary>
    /// Update updates an existing registration.
    /// </summary>
    /// <param name="registration">An existing registration</param>
    public Task Update(Registration registration);
}