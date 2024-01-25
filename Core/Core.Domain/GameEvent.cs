using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Core.Domain;

/// <summary>
/// GameEvent represents a game event that is organized by
/// a user.
/// </summary>
public class GameEvent
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required int MaxPlayers { get; set; }
    public required bool Is18Plus { get; set; }
    public required DateTime DateTime { get; set; }
    public required int Duration { get; set; }
    public required string ImageUri { get; set; }
    
    public required User Organizer { get; set; }
    public required Address Address { get; set; }
    public ICollection<Game> Games { get; set; } = new List<Game>();
    public ICollection<SnackPreference> AvailableSnacks { get; set; } = new List<SnackPreference>();
    [JsonIgnore]
    public ICollection<Registration> Registrations { get; } = new List<Registration>();
    
    [JsonIgnore]
    public int AddressId { get; set; }
}