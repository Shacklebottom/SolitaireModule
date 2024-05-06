using SolitaireDomain.Interfaces;

namespace SolitaireDomain.Objects
{
    public class Game
    {
        public IDeckUnwrapper Deck { get; set; }

        public IPlayer Player { get; set; }

        public ICardCollection[] Foundations { get; set; } = new ICardCollection[4];

        public ICardCollection[] Piles { get; set; } = new ICardCollection[7];

        //Constructor
        public Game(IDeckUnwrapper deck, ICardCollection[] foundations, ICardCollection[] piles, IPlayer? player = null)
        {
            Player = player ?? new Player("");

            Deck = deck;

            Deck.Shuffle();

            Foundations = foundations;

            Piles = piles;

            for (int i = 0; i < Piles.Length; i++)
            {
                Piles[i].SetupCardCollection(Deck.Draw(i + 1));
            }
        }

        public bool GameOver()
        {
            if (Piles.All(p => p.Cards.Count == 0) && Deck.Cards.Count == 0 && Deck.Flipped.Count == 0)
            {
                Player.Score += 1;

                return true;
            }
            return false;
        }
    }
}
