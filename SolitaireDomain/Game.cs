using static SolitaireDomain.EnumCardSuit;
using static SolitaireDomain.EnumCardRank;
using static SolitaireDomain.EnumCardColor;

namespace SolitaireDomain
{
    public class Game
    {
        public IDeckUnwrapper Deck { get; set; }

        public IPlayer Player { get; set; }

        public IHeap[] Foundations { get; set; }

        public IHeap[] Piles { get; set; }

        public Stack<Card> FlippedCards { get; set; } = [];

        //Constructor
        public Game(IDeckUnwrapper deck, IHeap[] foundations, IHeap[] piles, IPlayer? player = null)
        {
            Player = player ?? new Player("");

            Deck = deck;
            
            Deck.Shuffle();

            Foundations = foundations;

            Piles = piles;

            SetupPiles();
        }

        private void SetupPiles()
        {
            for (int i = 6; i >= 0; i--)
            {
                Piles[i].Cards.AddRange(Deck.Draw(i + 1));
                Piles[i].Cards.Last().FaceUp = true;
            }
        }

        public void FlipPileCard(int pileIndex)
        {
            Piles[pileIndex].Cards.Last().FaceUp = true;
        }

        public void FlipFromDeck(int drawCount)
        {
            Deck.Draw(drawCount).ForEach(c => { c.FaceUp = true; FlippedCards.Push(c); });
        }

        //ValidatePlay needs to be removed here after the dependency injection refactor is finished
        public static bool ValidatePlay(IEnumerable<Card> targetCollection, Card card, IEnumerable<List<Card>> parentCollection)
        {
            if (parentCollection.Count() == 4)
            {
                if (!targetCollection.Any())
                {
                    if (card.Rank == CardRank.Ace)
                    {
                        return true;
                    }
                    return false;
                }
                if (targetCollection.Last().Color != card.Color)
                {
                    if (targetCollection.Last().Rank == card.Rank - 1)
                    {
                        return true;
                    }
                }
                return false;
            }
            if (parentCollection.Count() == 7)
            {
                if (!targetCollection.Any())
                {
                    if (card.Rank == CardRank.King)
                    {
                        return true;
                    }
                    return false;
                }
                if (targetCollection.Last().FaceUp == false)
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
            throw new ArgumentOutOfRangeException(nameof(parentCollection), "Pile or Foundation doesn't have the correct number of elements");
        }

        public void PlayFromFlipped(List<Card> targetCollection, Stack<Card> flippedCards, List<Card>[] parentCollection)
        {
            if (ValidatePlay(targetCollection, flippedCards.Peek(), parentCollection))
            {
                targetCollection.Add(flippedCards.Pop());
            }
        }

        public void MovePile(List<Card> moveTo, List<Card> moveFrom, int index, List<Card>[] parentCollection)
        {
            if (moveFrom.Skip(index).Any(c => c.FaceUp == false))
            {
                throw new ArgumentException("Selected index includes invalid elements.");
            }

            if (parentCollection.Length == 7)
            {
                if (ValidatePlay(moveTo, moveFrom[index], parentCollection))
                {
                    var selectedCards = moveFrom.Skip(index).ToList();
                    foreach (var card in selectedCards)
                    {
                        moveTo.Add(card);
                        moveFrom.Remove(card);
                    }
                }
            }
            else if (parentCollection.Length == 4)
            {
                if (ValidatePlay(moveTo, moveFrom.Last(), parentCollection))
                {
                    var selectedCards = moveFrom.Skip(index).Reverse().ToList();
                    foreach (var card in selectedCards)
                    {
                        moveTo.Add(card);
                        moveFrom.Remove(card);
                    }
                }
            }
        }
        
        public bool GameOver(IEnumerable<Card>[] piles, IEnumerable<Card> deck, IEnumerable<Card> flipped, bool giveUp = false)
        {
            if (!giveUp)
            {
                if (piles.All(p => !p.Any()) && !deck.Any() && !flipped.Any())
                {
                    return true;
                }
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
