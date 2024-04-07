using static SolitaireDomain.CardEnum;

namespace SolitaireDomain
{
    public class Card
    {
        Rank Rank { get; set; }

        Suit Suit { get; set; }


        public Card(Rank rank, Suit suit)
        {
            Rank = rank;

            Suit = suit;
        }
    }
}
