using System.Drawing;
using static SolitaireDomain.EnumCardRank;
using static SolitaireDomain.EnumCardSuit;

namespace SolitaireDomain
{
    public class Card
    {
        public CardRank Rank { get; set; }

        public CardSuit Suit { get; set; }

        public bool FaceUp { get; set; } = false;

        public Color Color => Suit.GetSuitColor();

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
