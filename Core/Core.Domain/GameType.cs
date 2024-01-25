using System.ComponentModel.DataAnnotations;

namespace Core.Domain;

/// <summary>
/// GameType represents the type of a game.
/// </summary>
public enum GameType
{
    [Display(Name="Kaartenspel")] CardGame,
    [Display(Name="Bordspel")] BoardGame,
    [Display(Name="Dobbelspel")] DiceGame,
    [Display(Name="Videogame")] VideoGame,
    [Display(Name="Overig")] Other
}