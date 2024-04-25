using static SolitaireDomain.EnumCardRank;
using static SolitaireDomain.EnumCardSuit;

namespace SolitaireDomain
{
    public class StandardDeck : IDeckUnwrapper
    {
        public List<Card> Cards { get; set; } = [];

        public StandardDeck()
        {
            UnwrapDeck();
        }

        public void UnwrapDeck()
        {
            foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
            {
                foreach (CardRank rank in Enum.GetValues(typeof(CardRank)))
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
