using Core.DomainServices.Exceptions.Registration;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.BoardGamesEF.Tests.Repositories;

public class RegistrationRepositoryTest
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

    private readonly List<Registration> registrations = new()
    {
        new()
        {
            Id = 1,
            User = users[0],
            GameEvent = new()
            {
                Id = 1,
                Name = "Game 1",
                MaxPlayers = 10,
                Is18Plus = false,
                DateTime = DateTime.Now,
                Duration = 60,
                ImageUri = "https://www.google.com/image1.png",
                Organizer = users[1],
                Address = users[1].Address
            },
            Timestamp = DateTime.Now,
            DidAttend = false
        },
        new()
        {
            Id = 2,
            User = users[1],
            GameEvent = new()
            {
                Id = 2,
                Name = "Game 2",
                MaxPlayers = 8,
                Is18Plus = true,
                DateTime = DateTime.Now.AddHours(-2),
                Duration = 80,
                ImageUri = "https://www.google.com/image2.png",
                Organizer = users[0],
                Address = users[0].Address
            },
            Timestamp = DateTime.Now.AddHours(-2),
            DidAttend = true
        }
    };

    [Fact]
    public async Task ById_returns_registration_with_id()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockSet = registrations.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.Registrations).Returns(mockSet.Object);

        // Act
        var repository = new RegistrationRepository(mockContext.Object);
        var result = await repository.ById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(registrations[0], result);
    }

    [Fact]
    public async Task ById_returns_null_when_registration_with_id_does_not_exist()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockSet = registrations.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.Registrations).Returns(mockSet.Object);

        // Act
        var repository = new RegistrationRepository(mockContext.Object);
        var result = await repository.ById(13);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task ByGameEvent_returns_all_registrations_for_game_event()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockSet = registrations.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.Registrations).Returns(mockSet.Object);

        // Act
        var repository = new RegistrationRepository(mockContext.Object);
        var result = await repository.ByGameEvent(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Count);
        Assert.Contains(result, r => r == registrations[0]);
    }

    [Fact]
    public async Task ByGameEvent_returns_empty_list_when_no_registrations_for_game_event()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockSet = registrations.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.Registrations).Returns(mockSet.Object);

        // Act
        var repository = new RegistrationRepository(mockContext.Object);
        var result = await repository.ByGameEvent(13);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task CalculateAttendanceForUsers_returns_attendance_for_users()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockSet = registrations.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.Registrations).Returns(mockSet.Object);

        // Act
        var repository = new RegistrationRepository(mockContext.Object);
        var result = await repository.CalculateAttendanceForUsers(new List<string> { users[0].Id, users[1].Id });

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(0, result[users[0].Id].Item1);
        Assert.Equal(1, result[users[0].Id].Item2);
        Assert.Equal(1, result[users[1].Id].Item1);
        Assert.Equal(0, result[users[1].Id].Item2);
    }

    [Fact]
    public async Task CalculateAttendanceForUsers_returns_empty_dictionary_when_no_users()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockSet = registrations.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.Registrations).Returns(mockSet.Object);

        // Act
        var repository = new RegistrationRepository(mockContext.Object);
        var result = await repository.CalculateAttendanceForUsers(new List<string>());

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task CalculateAttendanceForUsers_returns_empty_dictionary_when_no_registrations_for_users()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockSet = registrations.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.Registrations).Returns(mockSet.Object);

        // Act
        var repository = new RegistrationRepository(mockContext.Object);
        var result = await repository.CalculateAttendanceForUsers(new List<string> { "a", "b" });

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task Create_throws_exception_when_user_is_organizer()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockDatabase = new Mock<DatabaseFacade>(mockContext.Object);
        var transactionMock = new Mock<IDbContextTransaction>();
        mockDatabase.Setup(d => d.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(transactionMock.Object);
        var mockSet = new List<Registration>().AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.Registrations).Returns(mockSet.Object);
        mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        mockContext.Setup(c => c.Database).Returns(mockDatabase.Object);

        // Act
        var repository = new RegistrationRepository(mockContext.Object);
        var registration = registrations[0];
        registration.GameEvent.Organizer = users[0];
        await Assert.ThrowsAsync<UserCantRegisterForOwnEventException>(() => repository.Create(registration));
        
        // Assert
        mockSet.Verify(m => m.AddAsync(registration, It.IsAny<CancellationToken>()), Times.Never);
        mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never());
        mockDatabase.Verify(m => m.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Never());
        transactionMock.Verify(m => m.CommitAsync(It.IsAny<CancellationToken>()), Times.Never());
    }

    [Fact]
    public async Task Create_throws_exception_when_user_is_not_18_and_event_is_18_plus()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockDatabase = new Mock<DatabaseFacade>(mockContext.Object);
        var transactionMock = new Mock<IDbContextTransaction>();
        mockDatabase.Setup(d => d.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(transactionMock.Object);
        var mockSet = new List<Registration>().AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.Registrations).Returns(mockSet.Object);
        mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        mockContext.Setup(c => c.Database).Returns(mockDatabase.Object);

        // Act
        var repository = new RegistrationRepository(mockContext.Object);
        var registration = registrations[0];
        registration.GameEvent.Is18Plus = true;
        registration.User.BirthDate = DateTime.Now.AddYears(-17);
        await Assert.ThrowsAsync<UserIsNot18PlusException>(() => repository.Create(registration));
        
        // Assert
        mockSet.Verify(m => m.AddAsync(registration, It.IsAny<CancellationToken>()), Times.Never);
        mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never());
        mockDatabase.Verify(m => m.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Never());
        transactionMock.Verify(m => m.CommitAsync(It.IsAny<CancellationToken>()), Times.Never());
    }

    [Fact]
    public async Task Create_throws_exception_when_user_is_already_registered_for_event()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockDatabase = new Mock<DatabaseFacade>(mockContext.Object);
        var transactionMock = new Mock<IDbContextTransaction>();
        mockDatabase.Setup(d => d.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(transactionMock.Object);
        var mockSet = registrations.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.Registrations).Returns(mockSet.Object);
        mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        mockContext.Setup(c => c.Database).Returns(mockDatabase.Object);

        // Act
        var repository = new RegistrationRepository(mockContext.Object);
        var registration = registrations[0];
        await Assert.ThrowsAsync<UserIsAlreadyRegisteredException>(() => repository.Create(registration));
        
        // Assert
        mockSet.Verify(m => m.AddAsync(registration, It.IsAny<CancellationToken>()), Times.Never);
        mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never());
        mockDatabase.Verify(m => m.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once());
        transactionMock.Verify(m => m.CommitAsync(It.IsAny<CancellationToken>()), Times.Never());
    }

    [Fact]
    public async Task Create_throws_exception_when_user_has_registered_on_same_date()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockDatabase = new Mock<DatabaseFacade>(mockContext.Object);
        var transactionMock = new Mock<IDbContextTransaction>();
        mockDatabase.Setup(d => d.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(transactionMock.Object);
        var mockSet = registrations.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.Registrations).Returns(mockSet.Object);
        mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        mockContext.Setup(c => c.Database).Returns(mockDatabase.Object);

        // Act
        var repository = new RegistrationRepository(mockContext.Object);
        var registration = new Registration
        {
            User = users[0],
            GameEvent = new()
            {
                Id = 3,
                Name = "Game 3",
                MaxPlayers = 10,
                Is18Plus = false,
                DateTime = DateTime.Now,
                Duration = 60,
                ImageUri = "https://www.google.com/image3.png",
                Organizer = users[1],
                Address = users[1].Address
            },
            Timestamp = DateTime.Now,
            DidAttend = false
        };
        await Assert.ThrowsAsync<UserHasAlreadyRegisteredOnSameDateException>(() => repository.Create(registration));
        
        // Assert
        mockSet.Verify(m => m.AddAsync(registration, It.IsAny<CancellationToken>()), Times.Never);
        mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never());
        mockDatabase.Verify(m => m.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once());
        transactionMock.Verify(m => m.CommitAsync(It.IsAny<CancellationToken>()), Times.Never());
    }

    [Fact]
    public async Task Create_throws_when_max_players_is_reached()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockDatabase = new Mock<DatabaseFacade>(mockContext.Object);
        var transactionMock = new Mock<IDbContextTransaction>();
        mockDatabase.Setup(d => d.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(transactionMock.Object);
        var mockSet = registrations.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.Registrations).Returns(mockSet.Object);
        mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        mockContext.Setup(c => c.Database).Returns(mockDatabase.Object);

        // Act
        var repository = new RegistrationRepository(mockContext.Object);
        registrations[0].GameEvent.MaxPlayers = 1;
        var registration = new Registration
        {
            User =
                new()
                {
                    Id = "1e3e6313-cb0a-4a6a-b110-a6ed6f964d63",
                    Username = "John2",
                    Gender = Gender.Man,
                    BirthDate = DateTime.Now.AddYears(-20),
                    Address = new()
                    {
                        City = "City 1",
                        Street = "Street 1",
                        HouseNumber = "1a"
                    }
                },
            GameEvent = registrations[0].GameEvent,
            Timestamp = DateTime.Now,
            DidAttend = false
        };
        await Assert.ThrowsAsync<MaxPlayersReachedException>(() => repository.Create(registration));

        // Assert
        mockSet.Verify(m => m.AddAsync(registration, It.IsAny<CancellationToken>()), Times.Never);
        mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never());
        mockDatabase.Verify(m => m.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once());
        transactionMock.Verify(m => m.CommitAsync(It.IsAny<CancellationToken>()), Times.Never());
    }

    [Fact]
    public async Task Create_creates_registration_when_registration_is_valid_1()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockDatabase = new Mock<DatabaseFacade>(mockContext.Object);
        var transactionMock = new Mock<IDbContextTransaction>();
        mockDatabase.Setup(d => d.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(transactionMock.Object);
        var mockSet = new List<Registration>().AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.Registrations).Returns(mockSet.Object);
        mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        mockContext.Setup(c => c.Database).Returns(mockDatabase.Object);

        // Act
        var repository = new RegistrationRepository(mockContext.Object);
        var registration = registrations[0];
        await repository.Create(registration);
        
        // Assert
        mockSet.Verify(m => m.AddAsync(registration, It.IsAny<CancellationToken>()), Times.Once);
        mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        mockDatabase.Verify(m => m.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once());
        transactionMock.Verify(m => m.CommitAsync(It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task Create_creates_registration_when_registration_is_valid_2()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockDatabase = new Mock<DatabaseFacade>(mockContext.Object);
        var transactionMock = new Mock<IDbContextTransaction>();
        mockDatabase.Setup(d => d.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(transactionMock.Object);
        var mockSet = new List<Registration>().AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.Registrations).Returns(mockSet.Object);
        mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        mockContext.Setup(c => c.Database).Returns(mockDatabase.Object);

        // Act
        var repository = new RegistrationRepository(mockContext.Object);
        var registration = registrations[1];
        await repository.Create(registration);
        
        // Assert
        mockSet.Verify(m => m.AddAsync(registration, It.IsAny<CancellationToken>()), Times.Once);
        mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        mockDatabase.Verify(m => m.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once());
        transactionMock.Verify(m => m.CommitAsync(It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task Create_creates_registration_when_registration_is_valid_3()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockDatabase = new Mock<DatabaseFacade>(mockContext.Object);
        var transactionMock = new Mock<IDbContextTransaction>();
        mockDatabase.Setup(d => d.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(transactionMock.Object);
        var mockSet = registrations.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.Registrations).Returns(mockSet.Object);
        mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        mockContext.Setup(c => c.Database).Returns(mockDatabase.Object);

        // Act
        var repository = new RegistrationRepository(mockContext.Object);
        var registration = new Registration
        {
            User = users[0],
            GameEvent = new()
            {
                Id = 3,
                Name = "Game 3",
                MaxPlayers = 10,
                Is18Plus = false,
                DateTime = DateTime.Now.AddDays(-5),
                Duration = 60,
                ImageUri = "https://www.google.com/image3.png",
                Organizer = users[1],
                Address = users[1].Address
            },
            Timestamp = DateTime.Now,
            DidAttend = false
        };
        await repository.Create(registration);
        
        // Assert
        mockSet.Verify(m => m.AddAsync(registration, It.IsAny<CancellationToken>()), Times.Once);
        mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        mockDatabase.Verify(m => m.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once());
        transactionMock.Verify(m => m.CommitAsync(It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task Update_updates_registration()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockSet = registrations.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.Registrations).Returns(mockSet.Object);
        mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var repository = new RegistrationRepository(mockContext.Object);
        var registration = registrations[0];
        registration.DidAttend = true;
        await repository.Update(registration);
        
        // Assert
        mockSet.Verify(m => m.Update(registration), Times.Once);
        mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }
}