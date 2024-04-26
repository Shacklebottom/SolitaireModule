using static SolitaireDomain.Enums.EnumCardRank;
using static SolitaireDomain.Enums.EnumCardSuit;
using static SolitaireDomain.Enums.EnumCardColor;

namespace SolitaireDomain.Objects
{
    public class Card
    {
        public CardRank Rank { get; set; }

        public CardSuit Suit { get; set; }

        public CardColor Color { get { return Suit == CardSuit.Spades || Suit == CardSuit.Clubs ? CardColor.Black : CardColor.Red; } }

        public bool FaceUp { get; set; } = false;

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
