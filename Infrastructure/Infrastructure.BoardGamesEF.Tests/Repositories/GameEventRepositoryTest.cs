using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.BoardGamesEF.Tests.Repositories;

public class GameEventRepositoryTest
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

    private static readonly List<Game> games = new()
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
            Is18Plus = true,
            ImageUri = "",
            Genre = GameGenre.Action,
            Type = GameType.CardGame
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
            Games = new[] { games[0] },
            Registrations =
            {
                new()
                {
                    UserId = users[0].Id,
                    User = users[0],
                    GameEvent = null,
                    Timestamp = DateTime.Now,
                    DidAttend = true
                }
            },
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
            Organizer = users[1],
            Games = new[] { games[0], games[1] },
        }
    };

    public GameEventRepositoryTest()
    {
        foreach (var gameEvent in gameEvents)
            foreach (var registration in gameEvent.Registrations)
                registration.GameEvent = gameEvent;
    }

    [Fact]
    public async Task ById_returns_game_event_with_id()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockSet = gameEvents.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.GameEvents).Returns(mockSet.Object);

        // Act
        var repository = new GameEventRepository(mockContext.Object);
        var result = await repository.ById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(gameEvents[0], result);
    }

    [Fact]
    public async Task ById_returns_null_when_game_event_with_id_does_not_exist()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockSet = gameEvents.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.GameEvents).Returns(mockSet.Object);

        // Act
        var repository = new GameEventRepository(mockContext.Object);
        var result = await repository.ById(13);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task All_returns_all_game_events()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockSet = gameEvents.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.GameEvents).Returns(mockSet.Object);

        // Act
        var repository = new GameEventRepository(mockContext.Object);
        var result = await repository.All();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(gameEvents.OrderBy(x => x.DateTime), result);
    }

    [Fact]
    public async Task ByOrganiser_returns_all_game_events_by_organiser()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockSet = gameEvents.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.GameEvents).Returns(mockSet.Object);

        // Act
        var repository = new GameEventRepository(mockContext.Object);
        var result = await repository.ByOrganiser(users[0].Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(new[] { gameEvents[0] }, result);
    }

    [Fact]
    public async Task ByOrganiser_returns_empty_collection_when_no_game_events_by_organiser_exist()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockSet = gameEvents.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.GameEvents).Returns(mockSet.Object);

        // Act
        var repository = new GameEventRepository(mockContext.Object);
        var result = await repository.ByOrganiser("a");

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task ByRegistrations_returns_all_game_events_by_registrations()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockSet = gameEvents.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.GameEvents).Returns(mockSet.Object);

        // Act
        var repository = new GameEventRepository(mockContext.Object);
        var result = await repository.ByRegistrations(users[0].Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(new[] { gameEvents[0] }, result);
    }

    [Fact]
    public async Task ByRegistrations_returns_empty_collection_when_no_game_events_by_registrations_exist()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockSet = gameEvents.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.GameEvents).Returns(mockSet.Object);

        // Act
        var repository = new GameEventRepository(mockContext.Object);
        var result = await repository.ByRegistrations(users[1].Id);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void Query_returns_game_events_queryable()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockSet = gameEvents.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.GameEvents).Returns(mockSet.Object);

        // Act
        var repository = new GameEventRepository(mockContext.Object);
        var results = repository.Query();

        // Assert
        Assert.Equal(gameEvents, results.ToList());
    }

    [Fact]
    public async Task Create_throws_exception_when_organiser_is_not_18_plus()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockSet = new[] { gameEvents[1] }.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.GameEvents).Returns(mockSet.Object);

        // Act
        var repository = new GameEventRepository(mockContext.Object);
        var gameEvent = gameEvents[0];
        gameEvent.Organizer.BirthDate = DateTime.Now.AddYears(-17);

        // Assert
        await Assert.ThrowsAsync<UnauthorizedOperationException>(() => repository.Create(gameEvent));
    }

    [Fact]
    public async Task Create_creates_game_event_when_valid()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockSet = new[] { gameEvents[1] }.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.GameEvents).Returns(mockSet.Object);

        // Act
        var repository = new GameEventRepository(mockContext.Object);
        var gameEvent = gameEvents[0];
        gameEvent.Id = 3;
        await repository.Create(gameEvent);

        // Assert
        mockSet.Verify(m => m.AddAsync(gameEvent, It.IsAny<CancellationToken>()), Times.Once);
        mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task Create_sets_game_event_18_plus_to_true_when_it_contains_18_plus_games()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockSet = new[] { gameEvents[0] }.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.GameEvents).Returns(mockSet.Object);

        // Act
        var repository = new GameEventRepository(mockContext.Object);
        var gameEvent = gameEvents[1];
        gameEvent.Is18Plus = false;
        gameEvent.Id = 3;
        await repository.Create(gameEvent);
        
        // Assert
        Assert.True(gameEvent.Is18Plus);
        mockSet.Verify(m => m.AddAsync(gameEvent, It.IsAny<CancellationToken>()), Times.Once);
        mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task Update_updates_game_event_when_valid()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockDatabase = new Mock<DatabaseFacade>(mockContext.Object);
        var transactionMock = new Mock<IDbContextTransaction>();
        mockDatabase.Setup(d => d.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(transactionMock.Object);
        var mockSet = gameEvents.AsQueryable().BuildMockDbSet();
        var mockSetRegistrations = gameEvents.SelectMany(x=>x.Registrations).AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.GameEvents).Returns(mockSet.Object);
        mockContext.Setup(c => c.Registrations).Returns(mockSetRegistrations.Object);
        mockContext.Setup(c => c.Database).Returns(mockDatabase.Object);

        // Act
        var repository = new GameEventRepository(mockContext.Object);
        var gameEvent = gameEvents[1];
        gameEvent.Name = "Updated name";
        await repository.Update(gameEvent);
        
        // Assert
        mockContext.Verify(m => m.Update(gameEvent), Times.Once);
        mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        mockDatabase.Verify(m => m.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once());
        transactionMock.Verify(m => m.CommitAsync(It.IsAny<CancellationToken>()), Times.Once());
    }
    
    [Fact]
    public async Task Update_throws_exception_when_game_event_already_has_registrations()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockDatabase = new Mock<DatabaseFacade>(mockContext.Object);
        var transactionMock = new Mock<IDbContextTransaction>();
        mockDatabase.Setup(d => d.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(transactionMock.Object);
        var mockSet = gameEvents.AsQueryable().BuildMockDbSet();
        var mockSetRegistrations = gameEvents.SelectMany(x=>x.Registrations).AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.GameEvents).Returns(mockSet.Object);
        mockContext.Setup(c => c.Registrations).Returns(mockSetRegistrations.Object);
        mockContext.Setup(c => c.Database).Returns(mockDatabase.Object);

        // Act
        var repository = new GameEventRepository(mockContext.Object);
        var gameEvent = gameEvents[0];
        gameEvent.Name = "Updated name";
        await Assert.ThrowsAsync<UnauthorizedOperationException>(() => repository.Update(gameEvent));
        
        // Assert
        mockContext.Verify(m => m.Update(gameEvent), Times.Never);
        mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        mockDatabase.Verify(m => m.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        transactionMock.Verify(m => m.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Fact]
    public async Task Update_sets_game_event_18_plus_to_true_when_it_contains_18_plus_games()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockDatabase = new Mock<DatabaseFacade>(mockContext.Object);
        var transactionMock = new Mock<IDbContextTransaction>();
        mockDatabase.Setup(d => d.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(transactionMock.Object);
        var mockSet = gameEvents.AsQueryable().BuildMockDbSet();
        var mockSetRegistrations = gameEvents.SelectMany(x=>x.Registrations).AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.GameEvents).Returns(mockSet.Object);
        mockContext.Setup(c => c.Registrations).Returns(mockSetRegistrations.Object);
        mockContext.Setup(c => c.Database).Returns(mockDatabase.Object);

        // Act
        var repository = new GameEventRepository(mockContext.Object);
        var gameEvent = gameEvents[1];
        gameEvent.Is18Plus = false;
        gameEvent.Name = "Updated name";
        await repository.Update(gameEvent);
        
        // Assert
        Assert.True(gameEvent.Is18Plus);
        mockContext.Verify(m => m.Update(gameEvent), Times.Once);
        mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        mockDatabase.Verify(m => m.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        transactionMock.Verify(m => m.CommitAsync(It.IsAny<CancellationToken>()), Times.Once());
    }
    
    [Fact]
    public async Task Delete_deletes_game_event_when_valid()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockDatabase = new Mock<DatabaseFacade>(mockContext.Object);
        var transactionMock = new Mock<IDbContextTransaction>();
        mockDatabase.Setup(d => d.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(transactionMock.Object);
        var mockSet = gameEvents.AsQueryable().BuildMockDbSet();
        var mockSetRegistrations = gameEvents.SelectMany(x=>x.Registrations).AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.GameEvents).Returns(mockSet.Object);
        mockContext.Setup(c => c.Registrations).Returns(mockSetRegistrations.Object);
        mockContext.Setup(c => c.Database).Returns(mockDatabase.Object);

        // Act
        var repository = new GameEventRepository(mockContext.Object);
        var gameEvent = gameEvents[1];
        await repository.Delete(gameEvent);
        
        // Assert
        mockContext.Verify(m => m.Remove(gameEvent), Times.Once);
        mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        mockDatabase.Verify(m => m.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        transactionMock.Verify(m => m.CommitAsync(It.IsAny<CancellationToken>()), Times.Once());
    }
    
    [Fact]
    public async Task Delete_throws_exception_when_game_event_already_has_registrations()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockDatabase = new Mock<DatabaseFacade>(mockContext.Object);
        var transactionMock = new Mock<IDbContextTransaction>();
        mockDatabase.Setup(d => d.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(transactionMock.Object);
        var mockSet = gameEvents.AsQueryable().BuildMockDbSet();
        var mockSetRegistrations = gameEvents.SelectMany(x=>x.Registrations).AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.GameEvents).Returns(mockSet.Object);
        mockContext.Setup(c => c.Registrations).Returns(mockSetRegistrations.Object);
        mockContext.Setup(c => c.Database).Returns(mockDatabase.Object);

        // Act
        var repository = new GameEventRepository(mockContext.Object);
        var gameEvent = gameEvents[0];
        await Assert.ThrowsAsync<UnauthorizedOperationException>(() => repository.Delete(gameEvent));
        
        // Assert
        mockContext.Verify(m => m.Remove(gameEvent), Times.Never);
        mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        mockDatabase.Verify(m => m.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        transactionMock.Verify(m => m.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}