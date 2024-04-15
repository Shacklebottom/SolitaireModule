using static SolitaireDomain.EnumCardSuit;
using static SolitaireDomain.EnumCardRank;
using static SolitaireDomain.EnumCardColor;

namespace SolitaireDomain
{
    public class Game
    {
        public DeckOfCards Deck { get; set; } = new();

        public Player Player { get; set; }

        public List<Card>[] Foundations { get; set; } = [[], [], [], []];

        public List<Card>[] Piles { get; set; } = [[], [], [], [], [], [], []];

        public Stack<Card> FlippedCards { get; set; } = [];

        //Constructor
        public Game(Player? player = null)
        {
            if (player != null) { Player = player; }
            else { Player = new(""); }

            Deck.Shuffle();

            SetupPiles();
        }

        private void SetupPiles()
        {
            for (int i = 6; i >= 0; i--)
            {
                Piles[i].AddRange(Deck.Draw(i + 1));
                Piles[i].Last().FaceUp = true;
            }
        }

        public void FlipPileCard(int pileIndex)
        {
            Piles[pileIndex].Last().FaceUp = true;
        }

        public void FlipFromDeck(int drawCount)
        {
            Deck.Draw(drawCount).ForEach(c => { c.FaceUp = true; FlippedCards.Push(c); });
        }

        public bool ValidatePlay(IEnumerable<Card> targetCollection, Card card)
        {
            if (!targetCollection.Any())
            {
                if (card.Rank == CardRank.King)
                {
                    return true;
                }
                else { return false; }
            }
            if (targetCollection.Last().Color != card.Color)
            {
                if (targetCollection.Last().Rank == card.Rank + 1)
                {
                    return true;
                }
            }
            return false;
        }

        public void PlayFromFlipped(List<Card> targetCollection, Stack<Card> flippedCards)
        {
            if (ValidatePlay(targetCollection, flippedCards.Peek()))
            {
                targetCollection.Add(flippedCards.Pop());
            }
        }

        //public void MovePileToPile(List<Card> moveTo, List<Card> moveFrom)
        //{
        //    moveTo.AddRange(moveFrom);
        //}
    }
}
