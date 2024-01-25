namespace Infrastructure.BoardGamesEF.Tests.Repositories;

public class UserRepositoryTest
{
    private readonly List<User> users = new()
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
    
    [Fact]
    public async Task ById_returns_user_with_id()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockSet = users.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.Users).Returns(mockSet.Object);

        // Act
        var repository = new UserRepository(mockContext.Object);
        var result = await repository.ById("a9b0c50c-90ff-4a82-a7b8-4d8187bc887a");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(result, users[0]);
    }
    
    [Fact]
    public async Task ById_returns_null_when_user_with_id_does_not_exist()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockSet = users.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.Users).Returns(mockSet.Object);

        // Act
        var repository = new UserRepository(mockContext.Object);
        var result = await repository.ById("a9b0c50c-90ff-4a82-a7b8-4d8187bc887b");

        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task Create_adds_user_to_context()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockSet = new List<User>().AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.Users).Returns(mockSet.Object);
        mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var repository = new UserRepository(mockContext.Object);
        await repository.Create(users[0]);

        // Assert
        mockSet.Verify(m => m.AddAsync(users[0], It.IsAny<CancellationToken>()), Times.Once());
        mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
    }
}