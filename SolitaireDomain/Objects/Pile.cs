using SolitaireDomain.Interfaces;
using static SolitaireDomain.Enums.EnumCardRank;

namespace SolitaireDomain.Objects
{
    public class Pile : ICardCollection
    {
        public List<Card> Cards { get; set; } = [];

        public bool ValidatePlay(Card card, ICardCollection? sourcePile = null, IDeckUnwrapper? sourceDeck = null)
        {
            if (card.FaceUp == false)
            {
                return false;
            }
            if (Cards.Count == 0)
            {
                if (card.Rank == CardRank.King)
                {
                    Cards.Add(card);

                    if (sourcePile != null)
                    {
                        sourcePile.Cards.Remove(card);

                        if (sourcePile.Cards.Last().FaceUp == false)
                        {
                            Card lastCard = sourcePile.Cards.Last();

                            lastCard.FaceUp = true;
                        }
                    }

                    sourceDeck?.Flipped.Pop();

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

                    if (sourcePile != null)
                    {
                        sourcePile.Cards.Remove(card);

                        if (sourcePile.Cards.Last().FaceUp == false)
                        {
                            Card lastCard = sourcePile.Cards.Last();

                            lastCard.FaceUp = true;
                        }
                    }

                    sourceDeck?.Flipped.Pop();

                    return true;
                }
            }
            return false;
        }

        public bool ValidateMove(ICardCollection sourceCollection, int startingIndex)
        {
            if (sourceCollection.Cards[startingIndex].FaceUp == false)
            {
                return false;
            }
            if (Cards.Count == 0)
            {
                if (sourceCollection.Cards[startingIndex].Rank == CardRank.King)
                {
                    return true;
                }
            }
            else if (Cards.Last().FaceUp == false)
            {
                return false;
            }
            else if (Cards.Last().Color != sourceCollection.Cards[startingIndex].Color)
            {
                if (Cards.Last().Rank == sourceCollection.Cards[startingIndex].Rank + 1)
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
