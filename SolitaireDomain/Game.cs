
namespace SolitaireDomain
{
    public class Game
    {
        public Deck Deck { get; set; } = new();

        public Player Player { get; set; } = new("");

        public List<Card>[] Foundations { get; set; } = [[], [], [], []];

        public List<Card>[] Piles { get; set; } = [[], [], [], [], [], [], []];

        //Constructor
        public Game(Player player)
        {
            InitializeGame(player);
        }

        private void InitializeGame(Player player)
        {
            Player = player;
            SetupPiles();

            void SetupPiles()
            {
                for (int i = 6; i >= 0; i--)
                {
                    Piles[i].AddRange(Deck.Draw(i + 1));
                    Piles[i].Last().FaceUp = true;
                }
            }
        }

        //public List<Card> GetPile(int pileIndex)
        //{
        //    return Piles[pileIndex].Where(p => p.FaceUp == true).ToList();
        //}
    }
}
