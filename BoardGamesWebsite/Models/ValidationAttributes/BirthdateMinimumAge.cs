using System.ComponentModel.DataAnnotations;

namespace BoardGamesWebsite.Models.ValidationAttributes;

/// <summary>
/// BirthdateMinimumAge is a validation attribute that checks
/// if the given birthdate is at least the given minimum age.
/// </summary>
public class BirthdateMinimumAge : ValidationAttribute
{
    private readonly int minimumAge;
    public BirthdateMinimumAge(int minimumAge)
    {
        this.minimumAge = minimumAge;
    }
    
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("Geboortedatum is verplicht");
        }
        
        var currentValue = (DateOnly)value;
        var now = DateOnly.FromDateTime(DateTime.Now);
        if (currentValue > now)
        {
            return new ValidationResult("Geboortedatum mag niet in de toekomst liggen");
        }

        if (currentValue.AddYears(minimumAge) > now)
        {
            return new ValidationResult($"Je moet minstens {minimumAge} jaar oud zijn voordat je een account mag aanmaken");
        }

        return ValidationResult.Success;
    }
}