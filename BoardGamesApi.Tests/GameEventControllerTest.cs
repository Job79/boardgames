using System.Security.Claims;
using Core.Domain;
using Core.DomainServices.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BoardGamesApi.Tests;

public class GameEventControllerTest
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
        },
        new()
        {
            Id = "d5371050-c5ff-46cc-9226-ddc05bfb8191",
            Username = "Johny",
            Gender = Gender.Woman,
            BirthDate = DateTime.Now.AddYears(-22),
            Address = new()
            {
                City = "City 2",
                Street = "Street 2",
                HouseNumber = "2a"
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
        },
        new()
        {
            Id = 2,
            Name = "Test event 2",
            MaxPlayers = 10,
            Is18Plus = false,
            DateTime = DateTime.Now.AddDays(-2),
            Duration = 80,
            ImageUri = "https://google.com/image2.png",
            Address = users[0].Address,
            Organizer = users[1]
        }
    };

    [Fact]
    public async Task All_returns_all_game_events()
    {
        // Arrange
        var store = new Mock<IUserStore<IdentityUser>>();
        var mgr = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        var gameEventRepository = new Mock<IGameEventRepository>();
        gameEventRepository.Setup(x => x.All()).ReturnsAsync(gameEvents);
        var controller = new GameEventController(
            mgr.Object,
            gameEventRepository.Object,
            Mock.Of<IUserRepository>(),
            Mock.Of<IRegistrationRepository>());

        // Act
        var result = await controller.All();

        // Assert
        Assert.Equal(gameEvents, result);
        gameEventRepository.Verify(x => x.All(), Times.Once);
    }
    
    [Fact]
    public async Task All_returns_empty_list_when_no_game_events_exist()
    {
        // Arrange
        var store = new Mock<IUserStore<IdentityUser>>();
        var mgr = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        var gameEventRepository = new Mock<IGameEventRepository>();
        gameEventRepository.Setup(x => x.All()).ReturnsAsync(new List<GameEvent>());
        var controller = new GameEventController(
            mgr.Object,
            gameEventRepository.Object,
            Mock.Of<IUserRepository>(),
            Mock.Of<IRegistrationRepository>());

        // Act
        var result = await controller.All();

        // Assert
        Assert.Empty(result);
        gameEventRepository.Verify(x => x.All(), Times.Once);
    }
    
    [Fact]
    public async Task ById_returns_game_event_with_id()
    {
        // Arrange
        var store = new Mock<IUserStore<IdentityUser>>();
        var mgr = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        var gameEventRepository = new Mock<IGameEventRepository>();
        gameEventRepository.Setup(x => x.ById(1)).ReturnsAsync(gameEvents[0]);
        var controller = new GameEventController(
            mgr.Object,
            gameEventRepository.Object,
            Mock.Of<IUserRepository>(),
            Mock.Of<IRegistrationRepository>());

        // Act
        var result = await controller.ById(1);

        // Assert
        Assert.Equal(gameEvents[0], result.Value);
        Assert.Null(result.Result);
        gameEventRepository.Verify(x => x.ById(1), Times.Once);
    }
    
    [Fact]
    public async Task ById_returns_null_when_game_event_with_id_does_not_exist()
    {
        // Arrange
        var store = new Mock<IUserStore<IdentityUser>>();
        var mgr = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        var gameEventRepository = new Mock<IGameEventRepository>();
        gameEventRepository.Setup(x => x.ById(1)).ReturnsAsync((GameEvent)null);
        var controller = new GameEventController(
            mgr.Object,
            gameEventRepository.Object,
            Mock.Of<IUserRepository>(),
            Mock.Of<IRegistrationRepository>());

        // Act
        var result = await controller.ById(1);

        // Assert
        Assert.Null(result.Value);
        Assert.IsType<NotFoundObjectResult>(result.Result);
        gameEventRepository.Verify(x => x.ById(1), Times.Once);
    }
    
    [Fact]
    public async Task Register_returns_registration_when_game_event_exists_and_user_is_logged_in()
    {
        // Arrange
        var store = new Mock<IUserStore<IdentityUser>>();
        var mgr = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        mgr.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(users[0].Id);
        var userRepository = new Mock<IUserRepository>();
        userRepository.Setup(x => x.ById(users[0].Id)).ReturnsAsync(users[0]);
        var gameEventRepository = new Mock<IGameEventRepository>();
        gameEventRepository.Setup(x => x.ById(1)).ReturnsAsync(gameEvents[0]);
        var registrationRepository = new Mock<IRegistrationRepository>();
        registrationRepository.Setup(x => x.Create(It.IsAny<Registration>()));
        var controller = new GameEventController(
            mgr.Object,
            gameEventRepository.Object,
            userRepository.Object,
            registrationRepository.Object);

        // Act
        var result = await controller.Register(1);

        // Assert
        Assert.IsType<Registration>(result.Value);
        Assert.Null(result.Result);
        mgr.Verify(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()), Times.Once);
        userRepository.Verify(x => x.ById(users[0].Id), Times.Once);
        gameEventRepository.Verify(x => x.ById(1), Times.Once);
        registrationRepository.Verify(x => x.Create(It.IsAny<Registration>()), Times.Once);
    }
    
    [Fact]
    public async Task Register_returns_404_when_game_event_with_id_does_not_exist()
    {
        // Arrange
        var store = new Mock<IUserStore<IdentityUser>>();
        var mgr = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        mgr.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(users[0].Id);
        var userRepository = new Mock<IUserRepository>();
        userRepository.Setup(x => x.ById(users[0].Id)).ReturnsAsync(users[0]);
        var gameEventRepository = new Mock<IGameEventRepository>();
        gameEventRepository.Setup(x => x.ById(1)).ReturnsAsync((GameEvent)null);
        var registrationRepository = new Mock<IRegistrationRepository>();
        registrationRepository.Setup(x => x.Create(It.IsAny<Registration>()));
        var controller = new GameEventController(
            mgr.Object,
            gameEventRepository.Object,
            userRepository.Object,
            registrationRepository.Object);

        // Act
        var result = await controller.Register(1);

        // Assert
        Assert.Null(result.Value);
        Assert.IsType<NotFoundObjectResult>(result.Result);
        mgr.Verify(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()), Times.Once);
        userRepository.Verify(x => x.ById(users[0].Id), Times.Once);
        gameEventRepository.Verify(x => x.ById(1), Times.Once);
        registrationRepository.Verify(x => x.Create(It.IsAny<Registration>()), Times.Never);
    }
}