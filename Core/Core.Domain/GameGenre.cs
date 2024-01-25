using System.ComponentModel.DataAnnotations;

namespace Core.Domain;

/// <summary>
/// GameGenre represents the genre of a game.
/// </summary>
public enum GameGenre
{
    [Display(Name="Actie")] Action,
    [Display(Name="Avontuur")] Adventure,
    [Display(Name="Rol spel")] RolePlaying,
    [Display(Name="Simulatie")] Simulation,
    [Display(Name="Strategie")] Strategy,
}