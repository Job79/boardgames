namespace Infrastructure.BoardGamesEF.Tests.Repositories;

public class GameRepositoryTest
{
    private readonly List<Game> games = new()
    {
        new()
        {
            Id = 1,
            Name = "",
            Description = "",
            Is18Plus = false,
            ImageUri = "",
            Genre = GameGenre.Action,
            Type = GameType.CardGame
        },
        new()
        {
            Id = 2,
            Name = "",
            Description = "",
            Is18Plus = false,
            ImageUri = "",
            Genre = GameGenre.Action,
            Type = GameType.CardGame
        }
    };

    [Fact]
    public async Task ById_returns_game_with_id()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockSet = games.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.Games).Returns(mockSet.Object);

        // Act
        var repository = new GameRepository(mockContext.Object);
        var result = await repository.ById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(games[0], result);
    }

    [Fact]
    public async Task ById_returns_null_when_game_with_id_does_not_exist()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockSet = games.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.Games).Returns(mockSet.Object);

        // Act
        var repository = new GameRepository(mockContext.Object);
        var result = await repository.ById(13);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task All_returns_all_games()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockSet = games.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.Games).Returns(mockSet.Object);

        // Act
        var repository = new GameRepository(mockContext.Object);
        var results = await repository.All();

        // Assert
        Assert.Equal(games, results);
    }

    [Fact]
    public async Task ByIds_returns_games_with_ids()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockSet = games.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.Games).Returns(mockSet.Object);

        // Act
        var repository = new GameRepository(mockContext.Object);
        var results = await repository.ByIds(new List<int> { 1, 2 });

        // Assert
        Assert.Equal(games, results);
    }

    [Fact]
    public async Task ByIds_returns_empty_collection_when_no_games_with_ids_exist()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockSet = games.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.Games).Returns(mockSet.Object);

        // Act
        var repository = new GameRepository(mockContext.Object);
        var results = await repository.ByIds(new List<int> { 13, 14 });

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public async Task ByIds_returns_single_game_when_single_id_matches()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockSet = games.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.Games).Returns(mockSet.Object);

        // Act
        var repository = new GameRepository(mockContext.Object);
        var results = await repository.ByIds(new List<int> { 1, 13 });

        // Assert
        Assert.Single(results);
        Assert.Equal(new List<Game> { games[0] }, results);
    }

    [Fact]
    public void Query_returns_games_queryable()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockSetObject = games.AsQueryable().BuildMockDbSet().Object;
        mockContext.Setup(c => c.Games).Returns(mockSetObject);

        // Act
        var repository = new GameRepository(mockContext.Object);
        var results = repository.Query();

        // Assert
        Assert.Same(mockSetObject, results);
    }
}