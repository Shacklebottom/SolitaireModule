using static SolitaireDomain.EnumCardRank;
using static SolitaireDomain.EnumCardSuit;
using static SolitaireDomain.EnumCardColor;

namespace SolitaireDomain
{
    public class Card
    {
        public CardRank Rank { get; set; }

        public CardSuit Suit { get; set; }

        public bool FaceUp { get; set; } = false;

        public CardColor Color => Suit.GetSuitColor();

        public Card(CardRank rank, CardSuit suit)
        {
            Rank = rank;

            Suit = suit;
        }

        public override string ToString()
        {
            return $"{Rank} of {Suit}";
        }
    }
}
