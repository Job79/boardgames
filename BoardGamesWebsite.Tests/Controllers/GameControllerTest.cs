using Core.Domain;
using Core.DomainServices.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BoardGamesWebsite.Tests.Controllers;

public class GameControllerTest
{
    private readonly Game game = new()
    {
        Id = 1,
        Name = "",
        Description = "",
        Is18Plus = false,
        ImageUri = "",
        Genre = GameGenre.Action,
        Type = GameType.CardGame
    };

    [Fact]
    public async Task Details_returns_game_with_id()
    {
        // Arrange
        var gameRepository = new Mock<IGameRepository>();
        gameRepository.Setup(x => x.ById(1)).ReturnsAsync(game);
        var controller = new GameController(gameRepository.Object);

        // Act
        var result = await controller.Details(1);

        // Assert
        var value = Assert.IsType<ViewResult>(result);
        Assert.Equal(game, value.Model);
        gameRepository.Verify(x => x.ById(1), Times.Once);
    }

    [Fact]
    public async Task Details_returns_not_found_when_game_with_id_does_not_exist()
    {
        // Arrange
        var gameRepository = new Mock<IGameRepository>();
        gameRepository.Setup(x => x.ById(1)).ReturnsAsync((Game?)null);
        var controller = new GameController(gameRepository.Object);

        // Act
        var result = await controller.Details(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        gameRepository.Verify(x => x.ById(1), Times.Once);
    }
}