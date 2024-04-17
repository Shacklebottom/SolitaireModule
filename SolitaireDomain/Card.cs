using static SolitaireDomain.EnumCardRank;
using static SolitaireDomain.EnumCardSuit;
using static SolitaireDomain.EnumCardColor;

namespace SolitaireDomain
{
    public class Card
    {
        public CardRank Rank { get; set; }

        public CardSuit Suit { get; set; }

        public CardColor Color { get { return this.Suit == CardSuit.Spades || this.Suit == CardSuit.Clubs ? CardColor.Black : CardColor.Red; }  }

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
