namespace Infrastructure.BoardGamesEF.Tests.Repositories;

public class SnackPreferenceRepositoryTest 
{
    private readonly List<SnackPreference> snackPreferences = new()
    {
        new()
        {
            Id = 1,
            Name = "Snack 1"
        },
        new()
        {
            Id = 2,
            Name = "Snack 2"
        },
        new()
        {
            Id = 3,
            Name = "Snack 3"
        }
    };
    
    [Fact]
    public async Task All_returns_all_snack_preferences()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockSet = snackPreferences.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.SnackPreferences).Returns(mockSet.Object);

        // Act
        var repository = new SnackPreferenceRepository(mockContext.Object);
        var result = await repository.All();

        // Assert
        Assert.Equal(snackPreferences, result);
    }
    
    [Fact]
    public async Task ByIds_returns_snack_preferences_with_ids()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockSet = snackPreferences.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.SnackPreferences).Returns(mockSet.Object);

        // Act
        var repository = new SnackPreferenceRepository(mockContext.Object);
        var result = await repository.ByIds(new List<int> {1, 3});

        // Assert
        Assert.Equal(new List<SnackPreference> {snackPreferences[0], snackPreferences[2]}, result);
    }
    
    [Fact]
    public async Task ByIds_returns_empty_list_when_no_snack_preferences_with_ids_exist()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockSet = snackPreferences.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.SnackPreferences).Returns(mockSet.Object);

        // Act
        var repository = new SnackPreferenceRepository(mockContext.Object);
        var result = await repository.ByIds(new List<int> {13, 14});

        // Assert
        Assert.Empty(result);
    }
    
    [Fact]
    public async Task ByIds_returns_single_snack_preference_when_single_id_matches()
    {
        // Arrange
        var mockContext = new Mock<BoardGamesContext>();
        var mockSet = snackPreferences.AsQueryable().BuildMockDbSet();
        mockContext.Setup(c => c.SnackPreferences).Returns(mockSet.Object);

        // Act
        var repository = new SnackPreferenceRepository(mockContext.Object);
        var result = await repository.ByIds(new List<int> {2, 13});

        // Assert
        Assert.Equal(new List<SnackPreference> {snackPreferences[1]}, result);
    }
}