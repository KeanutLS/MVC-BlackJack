using Microsoft.EntityFrameworkCore;
using PRG.EVA.BlackJack.Models;

public class ApplicationDbContext : DbContext
{
    public DbSet<GameLog> GameLogs { get; set; }
    public DbSet<BlackJackGame> BlackJackGames { get; set; }
    public DbSet<Deck> Decks { get; set; }
    public DbSet<Card> Cards { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}
