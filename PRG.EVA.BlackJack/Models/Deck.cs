using System.Collections.Generic;
using System.Linq;

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
                return Cards.Sum(card => card.GetValue());
            }
        }
    }
}
