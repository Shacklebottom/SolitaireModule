
namespace SolitaireDomain
{
    public class Game
    {
        public Deck Deck { get; set; } = new();

        public Player Player { get; set; } = new("");

        public List<Card>[] Foundations { get; set; } = new List<Card>[4];

        public List<Card>[] Piles { get; set; } = [ [], [], [], [], [], [], [] ];

        public Game(Player player)
        {
            Player = player;

            for (int i = 7; i > 0; i--)
            {
                Piles[i - 1].AddRange(Deck.Draw(i));
            }
            //for (int i = 7; i > 0; i--)
            //{
            //    Piles[i - 1].Add(new Card(CardEnum.Rank.Ace, CardEnum.Suit.Diamonds)); 
            //}
        }
    }
}
