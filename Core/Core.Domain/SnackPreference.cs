using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Core.Domain;

/// <summary>
/// SnackPreference represents the snack preference of
/// a user. Is also set for each game event, and if the users preference
/// does not match the availability of the game event the user is notified
/// before registering.
/// </summary>
public class SnackPreference
{
    public int Id { get; set; }
    public required string Name { get; set; }
    
    [JsonIgnore]
    public ICollection<User> Users { get; set; } = new List<User>();
    [JsonIgnore]
    public ICollection<GameEvent> GameEvents { get; set; } = new List<GameEvent>();
}