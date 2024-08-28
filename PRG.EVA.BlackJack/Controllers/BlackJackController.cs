using Microsoft.AspNetCore.Mvc;
using PRG.EVA.BlackJack.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Linq;

namespace PRG.EVA.BlackJack.Controllers
{
    public class BlackJackController : Controller
    {
        private static BlackJackGame _game;
        private readonly ApplicationDbContext _context;

        public BlackJackController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // InitGame action for initializing a new game
        public async Task<IActionResult> InitGame(decimal bet)
        {
            _game = new BlackJackGame
            {
                DealerDeck = new Deck { Cards = new List<Card>() },
                PlayerDeck = new Deck { Cards = new List<Card>() },
                Bet = bet,
                Status = GameStatus.Playing
            };

            // Get two cards for the dealer
            Card dealerCard1 = await GetCardAsync();
            Card dealerCard2 = await GetCardAsync();

            // Add the cards to the dealer's deck
            _game.DealerDeck.Cards.Add(dealerCard1);
            _game.DealerDeck.Cards.Add(dealerCard2);

            // Ensure the cards are set in the ViewBag
            ViewBag.DealerFirstCard = dealerCard1; // Visible card
            ViewBag.DealerHiddenCard = dealerCard2; // Hidden card for later

            // Get two cards for the player
            Card playerCard1 = await GetCardAsync();
            Card playerCard2 = await GetCardAsync();

            // Add the cards to the player's deck
            _game.PlayerDeck.Cards.Add(playerCard1);
            _game.PlayerDeck.Cards.Add(playerCard2);

            // Set initial ViewBag values
            ViewBag.Result = "Playing";
            ViewBag.TotalDealer = dealerCard1.GetValue(); // Show only the value of the first dealer card
            ViewBag.TotalPlayer = _game.PlayerDeck.TotalValue;

            // Log the game initialization
            var initGameLog = new GameLog
            {
                CreatedOn = DateTime.Now,
                PlayOption = "InitGame",
                CardSuit = $"{dealerCard1.suit}, {dealerCard2.suit}, {playerCard1.suit}, {playerCard2.suit}",
                CardRank = $"{dealerCard1.rank}, {dealerCard2.rank}, {playerCard1.rank}, {playerCard2.rank}",
                Result = "Playing",
                Wins = 0
            };
            _context.GameLogs.Add(initGameLog);
            await _context.SaveChangesAsync();

            return View("Play", _game);
        }

        public async Task<IActionResult> Play(string option)
        {
            // Ensure the game is initialized
            if (_game == null)
            {
                ViewBag.Result = "GameError";
                return View("Play");
            }

            // Ensure the game is still active
            if (_game.Status != GameStatus.Playing)
            {
                ViewBag.Result = _game.Status.ToString();
                ViewBag.TotalDealer = _game.DealerDeck.TotalValue;
                ViewBag.TotalPlayer = _game.PlayerDeck.TotalValue;
                ViewBag.DealerFirstCard = _game.DealerDeck.Cards.ElementAtOrDefault(0);
                ViewBag.DealerHiddenCard = _game.DealerDeck.Cards.ElementAtOrDefault(1);
                return View("Play");
            }

            if (option == "H")
            {
                // Draw a card for the player
                Card newCard = await GetCardAsync();
                _game.PlayerDeck.Cards.Add(newCard);

                // Check if the player busted
                if (_game.PlayerDeck.TotalValue > 21)
                {
                    _game.Status = GameStatus.Lost;
                    ViewBag.Result = "Lost";
                    ViewBag.Wins = _game.Bet; // Display bet amount as loss
                }
                else
                {
                    ViewBag.Result = "Playing";
                    ViewBag.Wins = 0;
                }

                // Ensure dealer's total remains unchanged
                ViewBag.TotalDealer = _game.DealerDeck.TotalValue;
                ViewBag.TotalPlayer = _game.PlayerDeck.TotalValue;

                // Log the player's action
                var hitLog = new GameLog
                {
                    CreatedOn = DateTime.Now,
                    PlayOption = "Hit",
                    CardSuit = newCard.suit.ToString(),
                    CardRank = newCard.rank.ToString(),
                    Result = ViewBag.Result,
                    Wins = ViewBag.Wins
                };
                _context.GameLogs.Add(hitLog);
                await _context.SaveChangesAsync();

                return View("Play");
            }
            else if (option == "S")
            {
                // Dealer hits until at least 17
                while (_game.DealerDeck.TotalValue < 17)
                {
                    Card newCard = await GetCardAsync();
                    _game.DealerDeck.Cards.Add(newCard);
                }

                // Determine the outcome
                if (_game.DealerDeck.TotalValue > 21 || _game.PlayerDeck.TotalValue > _game.DealerDeck.TotalValue)
                {
                    _game.Status = GameStatus.Won;
                    ViewBag.Result = "Won";
                    ViewBag.Wins = _game.Bet * 2;
                }
                else if (_game.PlayerDeck.TotalValue == _game.DealerDeck.TotalValue)
                {
                    _game.Status = GameStatus.Draw;
                    ViewBag.Result = "Draw";
                    ViewBag.Wins = _game.Bet;
                }
                else
                {
                    _game.Status = GameStatus.Lost;
                    ViewBag.Result = "Lost";
                    ViewBag.Wins = _game.Bet; // Display bet amount as loss
                }
                ViewBag.TotalDealer = _game.DealerDeck.TotalValue;
                ViewBag.TotalPlayer = _game.PlayerDeck.TotalValue;
                ViewBag.DealerFirstCard = _game.DealerDeck.Cards.ElementAtOrDefault(0); //Get the firs random card
                ViewBag.DealerHiddenCard = _game.DealerDeck.Cards.ElementAtOrDefault(1); //Get the second random card

                // Log the stand and game result
                var standLog = new GameLog
                {
                    CreatedOn = DateTime.Now,
                    PlayOption = "Stand",
                    CardSuit = string.Join(", ", _game.DealerDeck.Cards.Select(c => $"{c.suit}")),
                    CardRank = string.Join(", ", _game.DealerDeck.Cards.Select(c => $"{c.rank}")),
                    Result = ViewBag.Result,
                    Wins = ViewBag.Wins
                };
                _context.GameLogs.Add(standLog);
                await _context.SaveChangesAsync();

                return View("Play");
            }
            else
            {
                ViewBag.Result = "UnknownOption";
                ViewBag.TotalDealer = _game.DealerDeck.TotalValue;
                ViewBag.TotalPlayer = _game.PlayerDeck.TotalValue;

                return View("Play");
            }
        }

        // Method to retrieve a card from the API
        private async Task<Card> GetCardAsync()
        {
            using HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://mgp32-api.azurewebsites.net/");
            return await client.GetFromJsonAsync<Card>("blackjack/getcard");
        }
    }
}
