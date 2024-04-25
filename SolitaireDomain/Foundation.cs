
using static SolitaireDomain.EnumCardRank;

namespace SolitaireDomain
{
    public class Foundation : ICardCollection
    {
        public List<Card> Cards { get; set; } = [];

        public bool ValidatePlay(IEnumerable<Card> targetCollection, Card card)
        {
            if (!targetCollection.Any())
            {
                if (card.Rank == CardRank.Ace)
                {
                    return true;
                }
                return false;
            }
            if (targetCollection.Last().Color == card.Color)
            {
                if (targetCollection.Last().Rank == card.Rank - 1)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
