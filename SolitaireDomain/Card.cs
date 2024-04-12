using static SolitaireDomain.EnumCardRank;
using static SolitaireDomain.EnumCardSuit;

namespace SolitaireDomain
{
    public class Card
    {
        public Rank Rank { get; set; }

        public Suit Suit { get; set; }

        public bool FaceUp { get; set; } = false;

        public Card(Rank rank, Suit suit)
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
