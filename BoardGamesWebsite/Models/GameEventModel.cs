using System.ComponentModel.DataAnnotations;
using Core.Domain;

namespace BoardGamesWebsite.Models;

public class GameEventModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Naam is verplicht")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "ImageUrl is verplicht")]
    [Url(ErrorMessage = "ImageUrl moet een url zijn")]
    public required string ImageUri { get; set; }

    [Required(ErrorMessage = "Maximaal aantal spelers is verplicht")]
    public required int MaxPlayers { get; set; }

    public required bool Is18Plus { get; set; }

    [Required(ErrorMessage = "Datum en tijd is verplicht")]
    public required DateTime DateTime { get; set; }

    [Required(ErrorMessage = "Tijdsduur is verplicht")]
    public required int Duration { get; set; }

    [Required(ErrorMessage = "Stad is verplicht")]
    public required string City { get; set; }

    [Required(ErrorMessage = "Straat is verplicht")]
    public required string Street { get; set; }

    [Required(ErrorMessage = "HuisNummer is verplicht")]
    public required string HouseNumber { get; set; }

    [Required(ErrorMessage = "Spellen zijn verplicht")]
    public ICollection<int> Games { get; set; } = new List<int>();

    public ICollection<int> AvailableSnacks { get; set; }  = new List<int>();
}