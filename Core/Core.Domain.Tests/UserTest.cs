namespace Core.Domain.Tests;

public class UserTest
{
    [Fact]
    public void Is18Plus_is_true_when_birth_date_is_18_years_ago()
    {
        // Arrange
        var user = new User
        {
            Id = "",
            Username = "",
            Gender = Gender.Man,
            Address = new Address
            {
                City = "",
                Street = "",
                HouseNumber = "",
            },
            BirthDate = DateTime.Now.AddYears(-18),
        };

        // Act
        var result = user.Is18Plus;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Is18Plus_is_false_when_birthdate_is_17_years_ago()
    {
        // Arrange
        var user = new User
        {
            Id = "",
            Username = "",
            Gender = Gender.Man,
            Address = new Address
            {
                City = "",
                Street = "",
                HouseNumber = "",
            },
            BirthDate = DateTime.Now.AddYears(-17)
        };

        // Act
        var result = user.Is18Plus;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Is18Plus_is_false__when_birthdate_is_19_years_ago()
    {
        // Arrange
        var user = new User
        {
            Id = "",
            Username = "",
            Gender = Gender.Man,
            Address = new Address
            {
                City = "",
                Street = "",
                HouseNumber = "",
            },
            BirthDate = DateTime.Now.AddYears(-19)
        };

        // Act
        var result = user.Is18Plus;

        // Assert
        Assert.True(result);
    }
}