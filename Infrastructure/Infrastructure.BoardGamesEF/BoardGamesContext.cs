using Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.BoardGamesEF;

public class BoardGamesContext : DbContext
{
    public virtual DbSet<User> Users { get; set; } = default!;
    public virtual DbSet<Game> Games { get; set; } = default!;
    public virtual DbSet<GameEvent> GameEvents { get; set; } = default!;
    public virtual DbSet<Registration> Registrations { get; set; } = default!;
    public virtual DbSet<SnackPreference> SnackPreferences { get; set; } = default!;

    public BoardGamesContext()
    {
    }
    
    public BoardGamesContext(DbContextOptions<BoardGamesContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Registration>()
            .HasIndex(p => new { p.UserId, p.GameEventId }).IsUnique();

        modelBuilder.Entity<GameEvent>()
            .HasOne(p => p.Address)
            .WithOne()
            .OnDelete(DeleteBehavior.Restrict);
        
        // Add snack preferences.
        modelBuilder.Entity<SnackPreference>().HasData(
            new SnackPreference { Id = 1, Name = "Vegetarisch" },
            new SnackPreference { Id = 2, Name = "Alcohol vrij" },
            new SnackPreference { Id = 3, Name = "Lactose vrij" },
            new SnackPreference { Id = 4, Name = "Noten vrij" }
        );

        // Add some games.
        modelBuilder.Entity<Game>().HasData(
            new Game
            {
                Id = 1,
                Name = "Catan",
                Description =
                    "Het populaire handelsspel Catan in een nieuw jasje! Nu met nog meer spelbeleving! Lukt het jou om op Catan de belangrijkste macht te worden?",
                Genre = GameGenre.Action,
                Type = GameType.CardGame,
                ImageUri = "https://image.intertoys.nl/wcsstore/IntertoysCAS/images/catalog/full/1006506-e69938b2.jpg",
                Is18Plus = false,
            }, new Game
            {
                Id = 2,
                Name = "Monopoly",
                Description =
                    "Speel Monopoly Classic en maak kennis met de badeend, de Tyrannosaurus Rex en de pinguïn. Ga kopen en onderhandelen om de ultieme rijkdom te behalen.",
                Genre = GameGenre.RolePlaying,
                Type = GameType.BoardGame,
                ImageUri = "https://image.intertoys.nl/wcsstore/IntertoysCAS/images/catalog/full/1557023-b23d0603.jpg",
                Is18Plus = false
            },
            new Game
            {
                Id = 3,
                Name = "Risk",
                Description =
                    "Verover het territorium van je vijanden in het strategische spel Risk! De worp van de dobbelstenen is jouw weg naar de overwinning!",
                Genre = GameGenre.Action,
                Type = GameType.BoardGame,
                ImageUri = "https://image.intertoys.nl/wcsstore/IntertoysCAS/images/catalog/full/1387810-9348cdd6.jpg",
                Is18Plus = false
            },
            new Game
            {
                Id = 4,
                Name = "Rummikub",
                Description =
                    "Een speciale editie van Rummikub in een luxe, zwart blik! Ter ere van het 70-jarig bestaan van Rummikub heeft deze versie zware, zwarte stenen!",
                Genre = GameGenre.Strategy,
                Type = GameType.BoardGame,
                ImageUri = "https://image.intertoys.nl/wcsstore/IntertoysCAS/images/catalog/thumb/1984456-04e5b2e1.jpg",
                Is18Plus = true
            }
        );
    }
}