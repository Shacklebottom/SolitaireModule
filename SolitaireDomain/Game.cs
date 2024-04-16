﻿using static SolitaireDomain.EnumCardSuit;
using static SolitaireDomain.EnumCardRank;
using static SolitaireDomain.EnumCardColor;

namespace SolitaireDomain
{
    public class Game
    {
        public DeckOfCards Deck { get; set; } = new();

        public Player Player { get; set; }

        public List<Card>[] Foundations { get; set; } = [ [], [], [], [] ];

        public List<Card>[] Piles { get; set; } = [ [], [], [], [], [], [], [] ];

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

        public void MovePileToPile(List<Card> moveTo, List<Card> moveFrom, List<Card>[] parentCollection)
        {
            if (ValidatePlay(moveTo, moveFrom.First(c => c.FaceUp == true), parentCollection))
            {
                var faceUpCards = moveFrom.Where(c => c.FaceUp == true).ToList();
                foreach (var card in faceUpCards)
                {
                    moveTo.Add(card);
                    moveFrom.Remove(card);
                }
            }
        }
    }
}
