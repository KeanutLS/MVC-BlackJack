using System.Collections.Generic;

namespace PRG.EVA.BlackJack.Models
{
    public class BlackJackGame
    {
        public int Id { get; set; }
        public int DealerDeckId { get; set; }
        public Deck DealerDeck { get; set; }

        public int PlayerDeckId { get; set; }
        public Deck PlayerDeck { get; set; }
        public GameStatus Status { get; set; }
        public decimal Bet { get; set; }
    }
}
