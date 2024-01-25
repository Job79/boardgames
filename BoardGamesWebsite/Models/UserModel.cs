using System.ComponentModel.DataAnnotations;
using BoardGamesWebsite.Models.ValidationAttributes;
using Core.Domain;

namespace BoardGamesWebsite.Models;

public class UserModel
{
    [Required(ErrorMessage = "Gebruikersnaam is verplicht")]
    public required string Username { get; set; }

    [Required(ErrorMessage = "Email is verplicht")]
    [EmailAddress(ErrorMessage = "Email is niet geldig")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Wachtwoord is verplicht")]
    public required string Password { get; set; }

    [Required(ErrorMessage = "Gender is verplicht")]
    public Gender Gender { get; set; }

    [Required(ErrorMessage = "Geboortedatum is verplicht")]
    [BirthdateMinimumAge(16)]
    public required DateOnly BirthDate { get; set; }

    public ICollection<int> SnackPreferences { get; set; } = new List<int>();

    [Required(ErrorMessage = "Stad is verplicht")]
    public required string City { get; set; }

    [Required(ErrorMessage = "Straat is verplicht")]
    public required string Street { get; set; }

    [Required(ErrorMessage = "HuisNummer is verplicht")]
    public required string HouseNumber { get; set; }
}