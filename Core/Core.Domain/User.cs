using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Core.Domain;

/// <summary>
/// User represents a single user of the application. User does
/// not contain the email and password, these are stored in the
/// Security database using microsoft Identity Framework.
/// </summary>
public class User
{
    public required string Id { get; set; }
    public required string Username { get; set; }
    public required Gender Gender { get; set; }
    public required DateTime BirthDate { get; set; }
    public required Address Address { get; set; }

    public ICollection<SnackPreference> SnackPreferences { get; set; } = new List<SnackPreference>();
    [JsonIgnore]
    public ICollection<GameEvent> OrganisedGameEvents { get; set; } = new List<GameEvent>();
    [JsonIgnore]
    public ICollection<Registration> GameEventRegistrations { get; set; } = new List<Registration>();
    
    [JsonIgnore]
    public bool Is18Plus => BirthDate.AddYears(18) < DateTime.Now;
    [JsonIgnore]
    public int AddressId { get; set; }
}