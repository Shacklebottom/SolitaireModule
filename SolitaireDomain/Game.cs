
namespace SolitaireDomain
{
    public class Game
    {
        public Deck Deck { get; set; } = new();

        public Player Player { get; set; }

        public List<Card>[] Foundations { get; set; } = [[], [], [], []];

        public List<Card>[] Piles { get; set; } = [[], [], [], [], [], [], []];

        public List<Card> RevealedCards { get; set; } = [];

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

        public void RevealFromDeck(int drawCount)
        {
            RevealedCards = Deck.Draw(drawCount);
        }
    }
}
