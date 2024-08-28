namespace PRG.EVA.BlackJack.Models
{
    public class Deck
    {
        
        public int Id { get; set; }  // Primary Key

        public List<Card> Cards { get; set; }
        public int TotalValue
        {
            get
            {
                // Sum the value of all cards in the deck
                return Cards.Sum(card => card.GetValue());
            }
        }

    }
}
