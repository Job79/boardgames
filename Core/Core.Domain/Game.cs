using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Core.Domain;

/// <summary>
/// Game represents one of the games that can be played
/// at a game event.
/// </summary>
public class Game
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required bool Is18Plus { get; set; }
    public required string ImageUri { get; set; }
    public required GameGenre Genre { get; set; }
    public required GameType Type { get; set; }
    
    [JsonIgnore]
    public ICollection<GameEvent> GameEvents { get; set; } = new List<GameEvent>();
}