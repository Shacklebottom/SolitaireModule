using SolitaireDomain.Interfaces;
using static SolitaireDomain.Enums.EnumCardRank;

namespace SolitaireDomain.Objects
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
                    Cards.Add(card);

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
                    Cards.Add(card);

                    return true;
                }
            }
            return false;
        }

        public bool ValidateMove(IEnumerable<Card> sourceCollection, int startingIndex)
        {
            return true;
        }

        public void SetupCardCollection(List<Card> cards)
        {
            Cards.AddRange(cards);

            Cards.Last().FaceUp = true;
        }
    }
}
