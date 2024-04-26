using static SolitaireDomain.EnumCardRank;

namespace SolitaireDomain
{
    public class Pile : ICardCollection
    {
        public List<Card> Cards { get; set; } = [];

        public bool ValidatePlay(Card card)
        {
            if (Cards.Count == 0)
            {
                if (card.Rank == CardRank.King)
                {
                    return true;
                }
                return false;
            }
            else if (Cards.Last().FaceUp == false)
            {
                return false;
            }
            else if (Cards.Last().Color != card.Color)
            {
                if (Cards.Last().Rank == card.Rank + 1)
                {
                    return true;
                }
            }
            return false;
        }

        public void SetupCardCollection(List<Card> cards)
        {
            Cards.AddRange(cards);
            Cards.Last().FaceUp = true;
        }
    }
}
