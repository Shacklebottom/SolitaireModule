
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
                for (int i = 7; i > 0; i--)
                {
                    Piles[i - 1].AddRange(Deck.Draw(i));
                    Piles[i - 1].Last().FaceUp = true;
                }
            }
        }

        //public List<Card> GetPile(int pileIndex)
        //{
        //    return Piles[pileIndex].Where(fu => fu.FaceUp == true).ToList();
        //}
    }
}
