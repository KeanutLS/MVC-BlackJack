using PRG.EVA.BlackJack.Models;

public class BlackJackGame
{
    public int Id { get; set; }

    // Foreign Key for Decks
    public int DealerDeckId { get; set; }
    public Deck DealerDeck { get; set; }

    public int PlayerDeckId { get; set; } // Foreign Key for Decks
    public Deck PlayerDeck { get; set; } // Navigation Property

    public GameStatus Status { get; set; }
    public decimal Bet { get; set; }
}
