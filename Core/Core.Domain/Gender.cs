using System.ComponentModel.DataAnnotations;

namespace Core.Domain;

/// <summary>
/// Gender represents the gender of a user.
/// </summary>
public enum Gender
{
    [Display(Name="Man")] Man,
    [Display(Name="Vrouw")] Woman,
    [Display(Name="Overig")] Other
}