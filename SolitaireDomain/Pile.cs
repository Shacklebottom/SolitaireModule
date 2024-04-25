using static SolitaireDomain.EnumCardRank;

namespace SolitaireDomain
{
    public class Pile : IHeap
    {
        public List<Card> Cards { get; set; } = [];

        public bool ValidatePlay(IEnumerable<Card> targetCollection, Card card)
        {
            if (!targetCollection.Any())
            {
                if (card.Rank == CardRank.King)
                {
                    return true;
                }
                return false;
            }
            else if (targetCollection.Last().FaceUp == false)
            {
                return false;
            }
            else if (targetCollection.Last().Color != card.Color)
            {
                if (targetCollection.Last().Rank == card.Rank + 1)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
