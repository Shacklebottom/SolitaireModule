using SolitaireDomain.Interfaces;
using static SolitaireDomain.Enums.EnumCardRank;

namespace SolitaireDomain.Objects
{
    public class Foundation : ICardCollection
    {
        public List<Card> Cards { get; set; } = [];

        public bool ValidatePlay(Card card, ICardCollection? sourcePile = null, IDeckUnwrapper? sourceDeck = null)
        {
            if (Cards.Count == 0)
            {
                if (card.Rank == CardRank.Ace)
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
            if (Cards.Last().Color == card.Color)
            {
                if (Cards.Last().Rank == card.Rank - 1)
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
            throw new NotImplementedException("We don't move collections to a Foundation");
        }

        public void SetupCardCollection(List<Card> cards)
        {
            throw new NotImplementedException("We don't set up the Foundation collections");
        }
    }
}
