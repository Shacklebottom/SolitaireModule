
using static SolitaireDomain.EnumCardRank;

namespace SolitaireDomain
{
    public class Foundation : ICardCollection
    {
        public List<Card> Cards { get; set; } = [];

        public bool ValidatePlay(Card card)
        {
            if (!Cards.Any())
            {
                if (card.Rank == CardRank.Ace)
                {
                    return true;
                }
                return false;
            }
            if (Cards.Last().Color == card.Color)
            {
                if (Cards.Last().Rank == card.Rank - 1)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
