
using System.Linq;

namespace SolitaireDomain
{
    public class Game
    {
        public Deck Deck { get; set; } = new();

        public Player Player { get; set; }

        public List<Card>[] Foundations { get; set; } = [[], [], [], []];

        public List<Card>[] Piles { get; set; } = [[], [], [], [], [], [], []];

        public List<Card> FlippedCards { get; set; } = [];

        //Constructor
        public Game(Player? player = null)
        {
            if (player != null)
            {
                Player = player;
            }
            else
            {
                Player = new("");
            }

            SetupPiles();
        }

        private void SetupPiles()
        {
            for (int i = 6; i >= 0; i--)
            {
                Piles[i].AddRange(Deck.Draw(i + 1));
                Piles[i].Last().FaceUp = true;
            }
        }

        public void FlipPileCard(int pileIndex)
        {
            Piles[pileIndex].Last().FaceUp = true;
        }

        public List<Card> GetPile(int pileIndex)
        {
            return Piles[pileIndex].Where(p => p.FaceUp == true).ToList();
        }

        public void FlipFromDeck(int drawCount)
        {
            FlippedCards = Deck.Draw(drawCount);

            FlippedCards.ForEach(c => c.FaceUp = true);
        }

        public List<Card> PickUpCards(IEnumerable<Card> collection)
        {
            return collection.Where(c => c.FaceUp == true).ToList();
        }

        public List<Card> PutDownCards(IEnumerable<Card> collection)
        {
            var faceDownCards = collection.Where(c => c.FaceUp == false).ToList();
            var faceUpCards = collection.Where(c => c.FaceUp == true).ToList();

            if (Player.Holding.First().Rank != collection.Last().Rank - 1)
            {
                return collection.ToList();
            }
            else
            {
                faceUpCards.AddRange(Player.Holding);
                var fullCollection = faceDownCards;
                fullCollection.AddRange(faceUpCards);
                return fullCollection;
            }
        }
    }
}
