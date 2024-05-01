using SolitaireDomain.Interfaces;
using static SolitaireDomain.Enums.EnumCardRank;
using static SolitaireDomain.Enums.EnumCardSuit;

namespace SolitaireDomain.Objects
{
    public class StandardDeck : IDeckUnwrapper
    {
        public List<Card> Cards { get; set; } = [];

        public Stack<Card> Flipped {  get; set; } = new Stack<Card>();

        public StandardDeck()
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

        public void Flip(int count)
        {
            if (Cards.Count == 0)
            {
                Cards = Flipped.Reverse().ToList();

                Flipped.Clear();
            }

            List<Card> cards = Cards.Take(count).ToList();

            Cards = Cards.Skip(count).ToList();

            for (var i = 0; i < cards.Count; i++)
            {
                Flipped.Push(cards[i]);
            }
        }
    }
}
