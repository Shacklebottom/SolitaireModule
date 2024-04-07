using static SolitaireDomain.CardEnum;

namespace SolitaireDomain
{
    public class Deck
    {
        public List<Card> Cards { get; set; } = [];

        public Deck()
        {
            InitializeDeck();
        }

        private void InitializeDeck()
        {
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                {
                    Cards.Add(new Card(rank, suit));
                }
            }
        }

        public void Shuffle()
        {
            Random random = new Random();

            for (int i = 0; i < Cards.Count; i++)
            {
                int j = random.Next(i + 1);

                (Cards[i], Cards[j]) = (Cards[j], Cards[i]);
            }
        }

        public List<Card> Draw(int count)
        {
            List<Card> cards = Cards.Take(count).ToList();

            Cards = Cards.Skip(count).ToList();

            return cards;
        }
    }
}
