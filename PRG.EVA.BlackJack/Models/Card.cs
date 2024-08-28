﻿using PRG.EVA.BlackJack.Models;

public class Card
{
    public int Id { get; set; }

    public Suit suit { get; set; }
    public Rank rank { get; set; }

    public int DeckId { get; set; } // Foreign Key for Decks
    public Deck Deck { get; set; } // Navigation Property
    public int GetValue() // This method is used to get the value of the card
    {
        switch (rank)
        {
            case Rank.Ace: return 1;
            case Rank.Two: return 2;
            case Rank.Three: return 3;
            case Rank.Four: return 4;
            case Rank.Five: return 5;
            case Rank.Six: return 6;
            case Rank.Seven: return 7;
            case Rank.Eight: return 8;
            case Rank.Nine: return 9;
            case Rank.Ten: return 10;
            case Rank.Jack: return 10;
            case Rank.Queen: return 10;
            case Rank.King: return 10;
            default: return 0;
        }
    }
}
