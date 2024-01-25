namespace Core.Domain;

/// <summary>
/// Address represents the address of a user or game event.
/// </summary>
public class Address
{
    public int Id { get; set; }
    public required string City { get; set; }
    public required string Street { get; set; }
    public required string HouseNumber { get; set; }
}