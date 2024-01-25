using System.Text.Json.Serialization;

namespace Core.Domain;

/// <summary>
/// Registration represents a single registration for
/// a game event.
/// </summary>
public class Registration
{
    public int Id { get; set; }
    public required User User { get; set; }
    public required GameEvent GameEvent { get; set; }
    public required DateTime Timestamp { get; set; }
    
    // Determines whether the user actually attended
    // the game event. Set by the game organizer after
    // a game is finished.
    public required bool? DidAttend { get; set; }
    
    [JsonIgnore]
    public int GameEventId { get; set; }
    [JsonIgnore]
    public string UserId { get; set; }
}