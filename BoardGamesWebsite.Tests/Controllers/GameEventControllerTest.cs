using System.Security.Claims;
using BoardGamesWebsite.Models;
using Core.Domain;
using Core.DomainServices.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BoardGamesWebsite.Tests.Controllers;

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
            Address = users[0].Address,
        }
    };

    [Fact]
    public async Task All_returns_view_result_with_list_of_game_events()
    {
        // Arrange
        var store = new Mock<IUserStore<IdentityUser>>();
        var userManager =
            new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        var gameEventRepository = new Mock<IGameEventRepository>();
        gameEventRepository
            .Setup(repo => repo.All())
            .ReturnsAsync(gameEvents);
        var userRepository = new Mock<IUserRepository>();
        var gameRepository = new Mock<IGameRepository>();
        var snackPreferenceRepository = new Mock<ISnackPreferenceRepository>();
        var registrationRepository = new Mock<IRegistrationRepository>();
        var controller = new GameEventController(
            userManager.Object,
            gameEventRepository.Object,
            userRepository.Object,
            gameRepository.Object,
            snackPreferenceRepository.Object,
            registrationRepository.Object);

        // Act
        var result = await controller.All();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<GameEvent>>(viewResult.ViewData.Model);
        Assert.Equal(gameEvents, model);
    }

    [Fact]
    public async Task Details_returns_not_found_when_game_event_not_found()
    {
        // Arrange
        var store = new Mock<IUserStore<IdentityUser>>();
        var userManager =
            new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        var gameEventRepository = new Mock<IGameEventRepository>();
        gameEventRepository
            .Setup(repo => repo.ById(It.IsAny<int>()))
            .ReturnsAsync((GameEvent?)null);
        var userRepository = new Mock<IUserRepository>();
        var gameRepository = new Mock<IGameRepository>();
        var snackPreferenceRepository = new Mock<ISnackPreferenceRepository>();
        var registrationRepository = new Mock<IRegistrationRepository>();
        var controller = new GameEventController(
            userManager.Object,
            gameEventRepository.Object,
            userRepository.Object,
            gameRepository.Object,
            snackPreferenceRepository.Object,
            registrationRepository.Object);

        // Act
        var result = await controller.Details(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        gameEventRepository.Verify(repo => repo.ById(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task Details_returns_view_result_with_game_event()
    {
        // Arrange
        var store = new Mock<IUserStore<IdentityUser>>();
        var userManager =
            new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        var gameEventRepository = new Mock<IGameEventRepository>();
        gameEventRepository
            .Setup(repo => repo.ById(It.IsAny<int>()))
            .ReturnsAsync(gameEvents[0]);
        var userRepository = new Mock<IUserRepository>();
        var gameRepository = new Mock<IGameRepository>();
        var snackPreferenceRepository = new Mock<ISnackPreferenceRepository>();
        var registrationRepository = new Mock<IRegistrationRepository>();
        var controller = new GameEventController(
            userManager.Object,
            gameEventRepository.Object,
            userRepository.Object,
            gameRepository.Object,
            snackPreferenceRepository.Object,
            registrationRepository.Object);

        // Act
        var result = await controller.Details(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<GameEvent>(viewResult.ViewData.Model);
        Assert.Equal(gameEvents[0], model);
        gameEventRepository.Verify(repo => repo.ById(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task Organised_returns_view_result_with_list_of_game_events()
    {
        // Arrange
        var store = new Mock<IUserStore<IdentityUser>>();
        var userManager =
            new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        var gameEventRepository = new Mock<IGameEventRepository>();
        gameEventRepository
            .Setup(repo => repo.ByOrganiser(It.IsAny<string>()))
            .ReturnsAsync(gameEvents);
        var userRepository = new Mock<IUserRepository>();
        var gameRepository = new Mock<IGameRepository>();
        var snackPreferenceRepository = new Mock<ISnackPreferenceRepository>();
        var registrationRepository = new Mock<IRegistrationRepository>();
        var controller = new GameEventController(
            userManager.Object,
            gameEventRepository.Object,
            userRepository.Object,
            gameRepository.Object,
            snackPreferenceRepository.Object,
            registrationRepository.Object);

        // Act
        var result = await controller.Organised();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<GameEvent>>(viewResult.ViewData.Model);
        Assert.Equal(gameEvents, model);
        gameEventRepository.Verify(repo => repo.ByOrganiser(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Organised_returns_view_result_with_empty_list_of_game_events()
    {
        // Arrange
        var store = new Mock<IUserStore<IdentityUser>>();
        var userManager =
            new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        var gameEventRepository = new Mock<IGameEventRepository>();
        gameEventRepository
            .Setup(repo => repo.ByOrganiser(It.IsAny<string>()))
            .ReturnsAsync(new List<GameEvent>());
        var userRepository = new Mock<IUserRepository>();
        var gameRepository = new Mock<IGameRepository>();
        var snackPreferenceRepository = new Mock<ISnackPreferenceRepository>();
        var registrationRepository = new Mock<IRegistrationRepository>();
        var controller = new GameEventController(
            userManager.Object,
            gameEventRepository.Object,
            userRepository.Object,
            gameRepository.Object,
            snackPreferenceRepository.Object,
            registrationRepository.Object);

        // Act
        var result = await controller.Organised();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<GameEvent>>(viewResult.ViewData.Model);
        Assert.Empty(model);
        gameEventRepository.Verify(repo => repo.ByOrganiser(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Participating_returns_view_result_with_list_of_game_events()
    {
        // Arrange
        var store = new Mock<IUserStore<IdentityUser>>();
        var userManager =
            new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        var gameEventRepository = new Mock<IGameEventRepository>();
        gameEventRepository
            .Setup(repo => repo.ByRegistrations(It.IsAny<string>()))
            .ReturnsAsync(gameEvents);
        var userRepository = new Mock<IUserRepository>();
        var gameRepository = new Mock<IGameRepository>();
        var snackPreferenceRepository = new Mock<ISnackPreferenceRepository>();
        var registrationRepository = new Mock<IRegistrationRepository>();
        var controller = new GameEventController(
            userManager.Object,
            gameEventRepository.Object,
            userRepository.Object,
            gameRepository.Object,
            snackPreferenceRepository.Object,
            registrationRepository.Object);

        // Act
        var result = await controller.Participating();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<GameEvent>>(viewResult.ViewData.Model);
        Assert.Equal(gameEvents, model);
        gameEventRepository.Verify(repo => repo.ByRegistrations(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Participating_returns_view_result_with_empty_list_of_game_events()
    {
        // Arrange
        var store = new Mock<IUserStore<IdentityUser>>();
        var userManager =
            new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        var gameEventRepository = new Mock<IGameEventRepository>();
        gameEventRepository
            .Setup(repo => repo.ByRegistrations(It.IsAny<string>()))
            .ReturnsAsync(new List<GameEvent>());
        var userRepository = new Mock<IUserRepository>();
        var gameRepository = new Mock<IGameRepository>();
        var snackPreferenceRepository = new Mock<ISnackPreferenceRepository>();
        var registrationRepository = new Mock<IRegistrationRepository>();
        var controller = new GameEventController(
            userManager.Object,
            gameEventRepository.Object,
            userRepository.Object,
            gameRepository.Object,
            snackPreferenceRepository.Object,
            registrationRepository.Object);

        // Act
        var result = await controller.Participating();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<GameEvent>>(viewResult.ViewData.Model);
        Assert.Empty(model);
        gameEventRepository.Verify(repo => repo.ByRegistrations(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Create_returns_view_result()
    {
        // Arrange
        var store = new Mock<IUserStore<IdentityUser>>();
        var userManager =
            new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        var gameEventRepository = new Mock<IGameEventRepository>();
        var userRepository = new Mock<IUserRepository>();
        var gameRepository = new Mock<IGameRepository>();
        gameRepository.Setup(repo => repo.All())
            .ReturnsAsync(new List<Game>());
        var snackPreferenceRepository = new Mock<ISnackPreferenceRepository>();
        snackPreferenceRepository.Setup(repo => repo.All())
            .ReturnsAsync(new List<SnackPreference>());
        var registrationRepository = new Mock<IRegistrationRepository>();
        var controller = new GameEventController(
            userManager.Object,
            gameEventRepository.Object,
            userRepository.Object,
            gameRepository.Object,
            snackPreferenceRepository.Object,
            registrationRepository.Object);

        // Act
        var result = await controller.Create();

        // Assert
        Assert.IsType<ViewResult>(result);
        gameRepository.Verify(repo => repo.All(), Times.Once);
        snackPreferenceRepository.Verify(repo => repo.All(), Times.Once);
    }

    [Fact]
    public async Task Create_returns_view_result_with_game_event_when_model_state_is_invalid()
    {
        // Arrange
        var store = new Mock<IUserStore<IdentityUser>>();
        var userManager =
            new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        userManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>()))
            .Returns(users[0].Id);
        var gameEventRepository = new Mock<IGameEventRepository>();
        var userRepository = new Mock<IUserRepository>();
        var gameRepository = new Mock<IGameRepository>();
        gameRepository.Setup(repo => repo.All())
            .ReturnsAsync(new List<Game>());
        var snackPreferenceRepository = new Mock<ISnackPreferenceRepository>();
        snackPreferenceRepository.Setup(repo => repo.All())
            .ReturnsAsync(new List<SnackPreference>());
        var registrationRepository = new Mock<IRegistrationRepository>();
        var controller = new GameEventController(
            userManager.Object,
            gameEventRepository.Object,
            userRepository.Object,
            gameRepository.Object,
            snackPreferenceRepository.Object,
            registrationRepository.Object);
        controller.ModelState.AddModelError("Name", "Name is required");

        // Act
        var result = await controller.Create(new GameEventModel
        {
            Name = "Game 1",
            ImageUri = "https://google.com/image1.png",
            MaxPlayers = 10,
            Is18Plus = false,
            DateTime = DateTime.Now,
            Duration = 10,
            City = "City 1",
            Street = "Street 1",
            HouseNumber = "1a"
        });

        // Assert
        Assert.IsType<ViewResult>(result);
        gameRepository.Verify(repo => repo.All(), Times.Once);
        snackPreferenceRepository.Verify(repo => repo.All(), Times.Once);
    }

    [Fact]
    public async Task Create_returns_redirect_to_action_result_when_model_state_is_valid()
    {
        // Arrange
        var store = new Mock<IUserStore<IdentityUser>>();
        var userManager =
            new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        userManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>()))
            .Returns(users[0].Id);
        var gameEventRepository = new Mock<IGameEventRepository>();
        gameEventRepository.Setup(repo => repo.Create(It.IsAny<GameEvent>()));
        var userRepository = new Mock<IUserRepository>();
        userRepository.Setup(repo => repo.ById(It.IsAny<string>())).ReturnsAsync(users[0]);
        var gameRepository = new Mock<IGameRepository>();
        var snackPreferenceRepository = new Mock<ISnackPreferenceRepository>();
        var registrationRepository = new Mock<IRegistrationRepository>();
        var controller = new GameEventController(
            userManager.Object,
            gameEventRepository.Object,
            userRepository.Object,
            gameRepository.Object,
            snackPreferenceRepository.Object,
            registrationRepository.Object);

        // Act
        var result = await controller.Create(new GameEventModel
        {
            Name = "Game 1",
            ImageUri = "https://google.com/image1.png",
            MaxPlayers = 10,
            Is18Plus = false,
            DateTime = DateTime.Now,
            Duration = 10,
            City = "City 1",
            Street = "Street 1",
            HouseNumber = "1a"
        });

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Organised", redirectResult.ActionName);
        gameEventRepository.Verify(repo => repo.Create(It.IsAny<GameEvent>()), Times.Once);
    }

    [Fact]
    public async Task Edit_returns_not_found_when_game_event_not_found()
    {
        // Arrange
        var store = new Mock<IUserStore<IdentityUser>>();
        var userManager =
            new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        var gameEventRepository = new Mock<IGameEventRepository>();
        gameEventRepository.Setup(repo => repo.ById(It.IsAny<int>())).ReturnsAsync((GameEvent?)null);
        var userRepository = new Mock<IUserRepository>();
        var gameRepository = new Mock<IGameRepository>();
        var snackPreferenceRepository = new Mock<ISnackPreferenceRepository>();
        var registrationRepository = new Mock<IRegistrationRepository>();
        var controller = new GameEventController(
            userManager.Object,
            gameEventRepository.Object,
            userRepository.Object,
            gameRepository.Object,
            snackPreferenceRepository.Object,
            registrationRepository.Object);
        controller.ModelState.AddModelError("Name", "Name is required");

        // Act
        var result = await controller.Edit(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        gameEventRepository.Verify(repo => repo.ById(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task Edit_returns_not_found_when_user_is_not_organizer_of_game_event()
    {
        // Arrange
        var store = new Mock<IUserStore<IdentityUser>>();
        var userManager =
            new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        userManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>()))
            .Returns(users[0].Id);
        var gameEventRepository = new Mock<IGameEventRepository>();
        gameEventRepository.Setup(repo => repo.ById(It.IsAny<int>())).ReturnsAsync(gameEvents[0]);
        var userRepository = new Mock<IUserRepository>();
        var gameRepository = new Mock<IGameRepository>();
        var snackPreferenceRepository = new Mock<ISnackPreferenceRepository>();
        var registrationRepository = new Mock<IRegistrationRepository>();
        var controller = new GameEventController(
            userManager.Object,
            gameEventRepository.Object,
            userRepository.Object,
            gameRepository.Object,
            snackPreferenceRepository.Object,
            registrationRepository.Object);
        controller.ModelState.AddModelError("Name", "Name is required");
        gameEvents[0].Organizer = users[1];

        // Act
        var result = await controller.Edit(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        gameEventRepository.Verify(repo => repo.ById(It.IsAny<int>()), Times.Once);
    }
    
    [Fact]
    public async Task Edit_returns_view_result_with_game_event_when_model_state_is_invalid()
    {
        // Arrange
        var store = new Mock<IUserStore<IdentityUser>>();
        var userManager = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        userManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(users[0].Id);
        var gameEventRepository = new Mock<IGameEventRepository>();
        gameEventRepository.Setup(repo => repo.ById(It.IsAny<int>())).ReturnsAsync(gameEvents[0]);
        var userRepository = new Mock<IUserRepository>();
        var gameRepository = new Mock<IGameRepository>();
        gameRepository.Setup(repo => repo.All()).ReturnsAsync(new List<Game>());
        var snackPreferenceRepository = new Mock<ISnackPreferenceRepository>();
        snackPreferenceRepository.Setup(repo => repo.All()).ReturnsAsync(new List<SnackPreference>());
        var registrationRepository = new Mock<IRegistrationRepository>();
        var controller = new GameEventController(userManager.Object, gameEventRepository.Object, userRepository.Object, gameRepository.Object, snackPreferenceRepository.Object, registrationRepository.Object);
        controller.ModelState.AddModelError("Name", "Name is required");

        // Act
        var result = await controller.Edit(new GameEventModel
        {
            Id = 1,
            Name = "Game 1",
            ImageUri = "https://google.com/image1.png",
            MaxPlayers = 10,
            Is18Plus = false,
            DateTime = DateTime.Now,
            Duration = 10,
            City = "City 1",
            Street = "Street 1",
            HouseNumber = "1a"
        });

        // Assert
        Assert.IsType<ViewResult>(result);
        gameRepository.Verify(repo => repo.All(), Times.Once);
        snackPreferenceRepository.Verify(repo => repo.All(), Times.Once);
    }
    
    [Fact]
    public async Task Edit_returns_redirect_to_action_result_when_model_state_is_valid()
    {
        // Arrange
        var store = new Mock<IUserStore<IdentityUser>>();
        var userManager = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        userManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(users[0].Id);
        var gameEventRepository = new Mock<IGameEventRepository>();
        gameEventRepository.Setup(repo => repo.ById(It.IsAny<int>())).ReturnsAsync(gameEvents[0]);
        gameEventRepository.Setup(repo => repo.Update(It.IsAny<GameEvent>()));
        var userRepository = new Mock<IUserRepository>();
        var gameRepository = new Mock<IGameRepository>();
        var snackPreferenceRepository = new Mock<ISnackPreferenceRepository>();
        var registrationRepository = new Mock<IRegistrationRepository>();
        var controller = new GameEventController(userManager.Object, gameEventRepository.Object, userRepository.Object, gameRepository.Object, snackPreferenceRepository.Object, registrationRepository.Object);

        // Act
        var result = await controller.Edit(new GameEventModel
        {
            Id = 1,
            Name = "Game 1",
            ImageUri = "https://google.com/image1.png",
            MaxPlayers = 10,
            Is18Plus = false,
            DateTime = DateTime.Now,
            Duration = 10,
            City = "City 1",
            Street = "Street 1",
            HouseNumber = "1a"
        });

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Organised", redirectResult.ActionName);
        gameEventRepository.Verify(repo => repo.Update(It.IsAny<GameEvent>()), Times.Once);
    }
    
    [Fact]
    public async Task Delete_returns_not_found_when_game_event_not_found()
    {
        // Arrange
        var store = new Mock<IUserStore<IdentityUser>>();
        var userManager = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        var gameEventRepository = new Mock<IGameEventRepository>();
        gameEventRepository.Setup(repo => repo.ById(It.IsAny<int>())).ReturnsAsync((GameEvent?)null);
        var userRepository = new Mock<IUserRepository>();
        var gameRepository = new Mock<IGameRepository>();
        var snackPreferenceRepository = new Mock<ISnackPreferenceRepository>();
        var registrationRepository = new Mock<IRegistrationRepository>();
        var controller = new GameEventController(userManager.Object, gameEventRepository.Object, userRepository.Object, gameRepository.Object, snackPreferenceRepository.Object, registrationRepository.Object);

        // Act
        var result = await controller.Delete(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        gameEventRepository.Verify(repo => repo.ById(It.IsAny<int>()), Times.Once);
    }
    
    [Fact]
    public async Task Delete_returns_not_found_when_user_is_not_organizer_of_game_event()
    {
        // Arrange
        var store = new Mock<IUserStore<IdentityUser>>();
        var userManager = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        userManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(users[0].Id);
        var gameEventRepository = new Mock<IGameEventRepository>();
        gameEventRepository.Setup(repo => repo.ById(It.IsAny<int>())).ReturnsAsync(gameEvents[0]);
        var userRepository = new Mock<IUserRepository>();
        var gameRepository = new Mock<IGameRepository>();
        var snackPreferenceRepository = new Mock<ISnackPreferenceRepository>();
        var registrationRepository = new Mock<IRegistrationRepository>();
        var controller = new GameEventController(userManager.Object, gameEventRepository.Object, userRepository.Object, gameRepository.Object, snackPreferenceRepository.Object, registrationRepository.Object);
        gameEvents[0].Organizer = users[1];

        // Act
        var result = await controller.Delete(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        gameEventRepository.Verify(repo => repo.ById(It.IsAny<int>()), Times.Once);
    }
    
    [Fact]
    public async Task Delete_returns_redirect_to_organized_when_valid()
    {
        // Arrange
        var store = new Mock<IUserStore<IdentityUser>>();
        var userManager = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        userManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(users[0].Id);
        var gameEventRepository = new Mock<IGameEventRepository>();
        gameEventRepository.Setup(repo => repo.ById(It.IsAny<int>())).ReturnsAsync(gameEvents[0]);
        var userRepository = new Mock<IUserRepository>();
        var gameRepository = new Mock<IGameRepository>();
        var snackPreferenceRepository = new Mock<ISnackPreferenceRepository>();
        var registrationRepository = new Mock<IRegistrationRepository>();
        var controller = new GameEventController(userManager.Object, gameEventRepository.Object, userRepository.Object, gameRepository.Object, snackPreferenceRepository.Object, registrationRepository.Object);

        // Act
        var result = await controller.Delete(1);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Organised", redirectResult.ActionName);
        gameEventRepository.Verify(repo => repo.ById(It.IsAny<int>()), Times.Once);
    }
    
    [Fact]
    public async Task Registrations_returns_not_found_when_game_event_not_found()
    {
        // Arrange
        var store = new Mock<IUserStore<IdentityUser>>();
        var userManager = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        var gameEventRepository = new Mock<IGameEventRepository>();
        gameEventRepository.Setup(repo => repo.ById(It.IsAny<int>())).ReturnsAsync((GameEvent?)null);
        var userRepository = new Mock<IUserRepository>();
        var gameRepository = new Mock<IGameRepository>();
        var snackPreferenceRepository = new Mock<ISnackPreferenceRepository>();
        var registrationRepository = new Mock<IRegistrationRepository>();
        registrationRepository.Setup(repo => repo.ByGameEvent(It.IsAny<int>())).ReturnsAsync(new List<Registration>());
        var controller = new GameEventController(userManager.Object, gameEventRepository.Object, userRepository.Object, gameRepository.Object, snackPreferenceRepository.Object, registrationRepository.Object);

        // Act
        var result = await controller.Registrations(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        gameEventRepository.Verify(repo => repo.ById(It.IsAny<int>()), Times.Once);
        registrationRepository.Verify(repo => repo.ByGameEvent(It.IsAny<int>()), Times.Never);
    }
    
    [Fact]
    public async Task Registrations_returns_view_result_with_list_of_registrations()
    {
        // Arrange
        var store = new Mock<IUserStore<IdentityUser>>();
        var userManager = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        userManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(users[0].Id);
        var gameEventRepository = new Mock<IGameEventRepository>();
        gameEventRepository.Setup(repo => repo.ById(It.IsAny<int>())).ReturnsAsync(gameEvents[0]);
        var userRepository = new Mock<IUserRepository>();
        var gameRepository = new Mock<IGameRepository>();
        var snackPreferenceRepository = new Mock<ISnackPreferenceRepository>();
        var registrations = new List<Registration>
        {
            new()
            {
                Id = 1,
                GameEvent = gameEvents[0],
                User = users[0],
                Timestamp = DateTime.Now,
                DidAttend = null
            }
        };
        var registrationRepository = new Mock<IRegistrationRepository>();
        registrationRepository.Setup(repo => repo.ByGameEvent(It.IsAny<int>())).ReturnsAsync(registrations);
        registrationRepository.Setup(repo => repo.CalculateAttendanceForUsers(It.IsAny<ICollection<string>>())).ReturnsAsync(new Dictionary<string, Tuple<int, int>>());
        var controller = new GameEventController(userManager.Object, gameEventRepository.Object, userRepository.Object, gameRepository.Object, snackPreferenceRepository.Object, registrationRepository.Object);

        // Act
        var result = await controller.Registrations(1);

        // Assert
        Assert.IsType<ViewResult>(result);
        gameEventRepository.Verify(repo => repo.ById(It.IsAny<int>()), Times.Once);
        registrationRepository.Verify(repo => repo.ByGameEvent(It.IsAny<int>()), Times.Once);
    }
    
    [Fact]
    public async Task Registrations_returns_view_result_with_empty_list_of_registrations()
    {
        // Arrange
        var store = new Mock<IUserStore<IdentityUser>>();
        var userManager = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        userManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(users[0].Id);
        var gameEventRepository = new Mock<IGameEventRepository>();
        gameEventRepository.Setup(repo => repo.ById(It.IsAny<int>())).ReturnsAsync(gameEvents[0]);
        var userRepository = new Mock<IUserRepository>();
        var gameRepository = new Mock<IGameRepository>();
        var snackPreferenceRepository = new Mock<ISnackPreferenceRepository>();
        var registrationRepository = new Mock<IRegistrationRepository>();
        registrationRepository.Setup(repo => repo.ByGameEvent(It.IsAny<int>())).ReturnsAsync(new List<Registration>());
        registrationRepository.Setup(repo => repo.CalculateAttendanceForUsers(It.IsAny<ICollection<string>>())).ReturnsAsync(new Dictionary<string, Tuple<int, int>>());
        var controller = new GameEventController(userManager.Object, gameEventRepository.Object, userRepository.Object, gameRepository.Object, snackPreferenceRepository.Object, registrationRepository.Object);

        // Act
        var result = await controller.Registrations(1);

        // Assert
        Assert.IsType<ViewResult>(result);
        gameEventRepository.Verify(repo => repo.ById(It.IsAny<int>()), Times.Once);
        registrationRepository.Verify(repo => repo.ByGameEvent(It.IsAny<int>()), Times.Once);
    }
    
    [Fact]
    public async Task Registrations_returns_view_result_with_list_of_registrations_and_attendance_calculated_for_users()
    {
        // Arrange
        var store = new Mock<IUserStore<IdentityUser>>();
        var userManager = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        userManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(users[0].Id);
        var gameEventRepository = new Mock<IGameEventRepository>();
        gameEventRepository.Setup(repo => repo.ById(It.IsAny<int>())).ReturnsAsync(gameEvents[0]);
        var userRepository = new Mock<IUserRepository>();
        var gameRepository = new Mock<IGameRepository>();
        var registrations = new List<Registration>
        {
            new()
            {
                Id = 1,
                GameEvent = gameEvents[0],
                User = users[0],
                Timestamp = DateTime.Now,
                DidAttend = null
            }
        };
        var registrationRepository = new Mock<IRegistrationRepository>();
        registrationRepository.Setup(repo => repo.ByGameEvent(It.IsAny<int>())).ReturnsAsync(registrations);
        registrationRepository.Setup(repo => repo.CalculateAttendanceForUsers(It.IsAny<ICollection<string>>())).ReturnsAsync(new Dictionary<string, Tuple<int, int>>
        {
            { users[0].Id, new Tuple<int, int>(1, 1) }
        });
        var snackPreferenceRepository = new Mock<ISnackPreferenceRepository>();
        var controller = new GameEventController(userManager.Object, gameEventRepository.Object, userRepository.Object, gameRepository.Object, snackPreferenceRepository.Object, registrationRepository.Object);

        // Act
        var result = await controller.Registrations(1);

        // Assert
        Assert.IsType<ViewResult>(result);
        gameEventRepository.Verify(repo => repo.ById(It.IsAny<int>()), Times.Once);
        registrationRepository.Verify(repo => repo.ByGameEvent(It.IsAny<int>()), Times.Once);
        registrationRepository.Verify(repo => repo.CalculateAttendanceForUsers(It.IsAny<ICollection<string>>()), Times.Once);
    }
}