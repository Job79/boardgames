using Core.Domain;
using Core.DomainServices.Exceptions;
using Core.DomainServices.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BoardGamesWebsite.Tests.Controllers;

public class RegistrationControllerTest
{
    private static readonly List<User> users = new()
    {
        new()
        {
            Id = "a9b0c50c-90ff-4a82-a7b8-4d8187bc887a",
            Username = "John",
            Gender = Gender.Man,
            BirthDate = DateTime.Now.AddYears(-20),
            Address = new()
            {
                City = "City 1",
                Street = "Street 1",
                HouseNumber = "1a"
            }
        }
    };

    private readonly List<GameEvent> gameEvents = new()
    {
        new()
        {
            Id = 1,
            Name = "Test event 1",
            MaxPlayers = 10,
            Is18Plus = false,
            DateTime = DateTime.Now,
            Duration = 60,
            ImageUri = "https://google.com/image1.png",
            Organizer = users[0],
            Address = users[0].Address
        }
    };

    [Fact]
    public async Task Register_returns_redirect_to_participating_when_registration_succeeds()
    {
        // Arrange
        var store = new Mock<IUserStore<IdentityUser>>();
        var userManager = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        var registrationRepository = new Mock<IRegistrationRepository>();
        registrationRepository.Setup(x => x.Create(It.IsAny<Registration>()));
        var gameEventRepository = new Mock<IGameEventRepository>();
        gameEventRepository.Setup(x => x.ById(It.IsAny<int>())).ReturnsAsync(gameEvents[0]);
        var userRepository = new Mock<IUserRepository>();
        userRepository.Setup(x => x.ById(It.IsAny<string>())).ReturnsAsync(users[0]);
        var controller = new RegistrationController(userManager.Object, registrationRepository.Object,
            gameEventRepository.Object, userRepository.Object);

        // Act
        var result = await controller.Register(1);

        // Assert
        var value = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Participating", value.ActionName);
        Assert.Equal("GameEvent", value.ControllerName);
        registrationRepository.Verify(x => x.Create(It.IsAny<Registration>()), Times.Once);
        gameEventRepository.Verify(x => x.ById(It.IsAny<int>()), Times.Once);
        userRepository.Verify(x => x.ById(It.IsAny<string>()), Times.Once);
    }
    
    [Fact]
    public async Task Register_returns_error_view_when_registration_fails()
    {
        // Arrange
        var store = new Mock<IUserStore<IdentityUser>>();
        var userManager = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        var registrationRepository = new Mock<IRegistrationRepository>();
        registrationRepository.Setup(x => x.Create(It.IsAny<Registration>())).Throws(new UnauthorizedOperationException("Test error"));
        var gameEventRepository = new Mock<IGameEventRepository>();
        gameEventRepository.Setup(x => x.ById(It.IsAny<int>())).ReturnsAsync(gameEvents[0]);
        var userRepository = new Mock<IUserRepository>();
        userRepository.Setup(x => x.ById(It.IsAny<string>())).ReturnsAsync(users[0]);
        var controller = new RegistrationController(userManager.Object, registrationRepository.Object,
            gameEventRepository.Object, userRepository.Object);

        // Act
        var result = await controller.Register(1);

        // Assert
        var value = Assert.IsType<ViewResult>(result);
        Assert.Equal("Error", value.ViewName);
        Assert.Equal("Test error", value.Model);
        registrationRepository.Verify(x => x.Create(It.IsAny<Registration>()), Times.Once);
        gameEventRepository.Verify(x => x.ById(It.IsAny<int>()), Times.Once);
        userRepository.Verify(x => x.ById(It.IsAny<string>()), Times.Once);
    }
    
    [Fact]
    public async Task Register_returns_error_when_game_event_is_not_found()
    {
        // Arrange
        var store = new Mock<IUserStore<IdentityUser>>();
        var userManager = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        var registrationRepository = new Mock<IRegistrationRepository>();
        var gameEventRepository = new Mock<IGameEventRepository>();
        gameEventRepository.Setup(x => x.ById(It.IsAny<int>())).ReturnsAsync((GameEvent?)null);
        var userRepository = new Mock<IUserRepository>();
        userRepository.Setup(x => x.ById(It.IsAny<string>())).ReturnsAsync(users[0]);
        var controller = new RegistrationController(userManager.Object, registrationRepository.Object,
            gameEventRepository.Object, userRepository.Object);

        // Act
        var result = await controller.Register(1);

        // Assert
        var value = Assert.IsType<ViewResult>(result);
        Assert.Equal("Error", value.ViewName);
        Assert.Equal("Game event not found", value.Model);
        registrationRepository.Verify(x => x.Create(It.IsAny<Registration>()), Times.Never);
        gameEventRepository.Verify(x => x.ById(It.IsAny<int>()), Times.Once);
        userRepository.Verify(x => x.ById(It.IsAny<string>()), Times.Once);
    }
}