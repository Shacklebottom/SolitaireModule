﻿using static SolitaireDomain.Enums.EnumCardSuit;
using static SolitaireDomain.Enums.EnumCardRank;
using static SolitaireDomain.Enums.EnumCardColor;
using SolitaireDomain.Interfaces;

namespace SolitaireDomain.Objects
{
    public class Game
    {
        public IDeckUnwrapper Deck { get; set; }

        public IPlayer Player { get; set; }

        public ICardCollection[] Foundations { get; set; } = new ICardCollection[4];

        public ICardCollection[] Piles { get; set; } = new ICardCollection[7];

        public Stack<Card> FlippedCards { get; set; } = [];

        //Constructor
        public Game(IDeckUnwrapper deck, ICardCollection[] foundations, ICardCollection[] piles, IPlayer? player = null)
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
                Piles[i].SetupCardCollection(Deck.Draw(i + 1));
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

        public void PlayFromFlipped(ICardCollection targetCollection, Stack<Card> flippedCards)
        {
            if (targetCollection.ValidatePlay(flippedCards.Peek()))
            {
                targetCollection.Cards.Add(flippedCards.Pop());
            }
        }

        public void MovePile(ICardCollection moveTo, ICardCollection moveFrom, int index)
        {
            if (moveFrom.Cards.Skip(index).Any(c => c.FaceUp == false))
            {
                throw new ArgumentException("Selected index includes invalid elements.");
            }
            if (moveTo.ValidatePlay(moveFrom.Cards[index]))
            {
                var selectedCards = moveFrom.Cards.Skip(index).ToList();
                foreach (var card in selectedCards)
                {
                    moveTo.Cards.Add(card);
                    moveFrom.Cards.Remove(card);
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