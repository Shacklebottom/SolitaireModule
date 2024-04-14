﻿
using static SolitaireDomain.EnumCardRank;
using static SolitaireDomain.EnumCardSuit;
using SolitaireDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ModuleTesting
{
    [TestClass]
    public class GameTests
    {
        //Global Test Variables
        private Player _testPlayer { get; set; } = new();
        private Game _testGame { get; set; } = new();
        private List<Card> _testCollection { get; set; } = [];

        [TestInitialize]
        public void GameTestsInitialize()
        {
            _testPlayer = new Player("Player 1");
            _testGame = new Game(_testPlayer);
            _testCollection = new List<Card>()
            {
                new (CardRank.Five, CardSuit.Diamonds),
                new (CardRank.Four, CardSuit.Hearts),
                new (CardRank.Three, CardSuit.Spades) { FaceUp = true },
                new (CardRank.Two, CardSuit.Clubs) { FaceUp = true }
            };
        }

        [TestMethod]
        public void PlayerHasName()
        {
            //We're preserving the Player obj being passed in to the Game constructor by the UI :)

            //Arrange
            //1. using _testGame

            //Act

            //Assert
            Assert.AreEqual("Player 1", _testGame.Player.Name);
        }

        [TestMethod]
        public void ThereAreFourFoundations()
        {
            //Arrange
            //1. using _testGame

            //Act

            //Assert
            Assert.IsTrue(_testGame.Foundations.Length == 4);
        }

        [TestMethod]
        public void ThereAreSevenPiles()
        {
            //Arrange
            //1. using _testGame

            //Act

            //Assert
            Assert.IsTrue(_testGame.Piles.Length == 7);
        }

        [TestMethod]
        public void DeckWasShuffled()
        {
            //Arrange
            //Due to how the Deck obj and SetupPile() works, Piles.Last() will look like the testPile if the deck has not been shuffled,
            //so if this test fails for an unknown reason, run the test again to be sure :)
            var testPile = new List<Card>()
            {
                 new (CardRank.Ace, CardSuit.Hearts),
                 new (CardRank.Two, CardSuit.Hearts),
                 new (CardRank.Three, CardSuit.Hearts),
                 new (CardRank.Four, CardSuit.Hearts),
                 new (CardRank.Five, CardSuit.Hearts),
                 new (CardRank.Six, CardSuit.Hearts),
                 new (CardRank.Seven, CardSuit.Hearts) { FaceUp = true },
            };

            //Act
            //1. in the Game() constructor, Deck.Shuffle() is called.

            //Assert.Dominance
            Assert.IsFalse(testPile.SequenceEqual(_testGame.Piles.Last(), new CardEqualityComparer()));
        }

        [TestMethod]
        public void PilesHaveTheCorrectCount()
        {
            //Arrange
            //1. using _testGame

            //Act

            //Assert.Dominance
            //1. We are testing for Piles[index].Count == index + 1;
            Assert.IsTrue(_testGame.Piles.Select((pile, index) => new { pile, index }).All(obj => obj.pile.Count == obj.index + 1),
                "At least 1 pile doesn't have the correct number of cards");
        }

        [TestMethod]
        public void EachPileOnlyHasOneFaceUpCard()
        {
            //Arrange
            //1. using _testGame

            //Act

            //Assert.Dominance
            Assert.IsTrue(_testGame.Piles.All(p => p.Count(c => c.FaceUp) == 1));
        }

        [TestMethod]
        public void FaceDownPileCardCanBeTurnedFaceUp()
        {
            //Arrange
            //1. using _testGame
            var pileIndex = 0;
            _testGame.Piles[pileIndex].Last().FaceUp = false;

            //Act
            _testGame.FlipPileCard(pileIndex);

            //Assert
            Assert.AreEqual(true, _testGame.Piles[pileIndex].Last().FaceUp);
        }

        [TestMethod]
        public void ANumberOfCardsCanBeFlipped()
        {
            //Arrange
            //1. using _testGame
            var drawCount = 3;

            //Act
            _testGame.FlipFromDeck(drawCount);

            //Assert
            //1. The correct number of cards are present.
            Assert.AreEqual(drawCount, _testGame.FlippedCards.Count);
            //2. and, each card revealed is FaceUp.
            Assert.IsTrue(_testGame.FlippedCards.ToList().TrueForAll(c => c.FaceUp == true));
        }

        [TestMethod]
        public void AValidPlayIsOfDescendingRankAndAlternatingColor_FalseRank()
        {
            //Arrange
            var invalidCard = new Card(CardRank.Two, CardSuit.Diamonds);

            //Act
            var result = _testGame.ValidPlay(_testCollection, invalidCard);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AValidPlayIsOfDescendingRankAndAlternatingColor_FalseColor()
        {
            //Arrange
            var invalidCard = new Card(CardRank.Ace, CardSuit.Spades);

            //Act
            var result = _testGame.ValidPlay(_testCollection, invalidCard);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AValidPlayIsOfDescendingRankAndAlternatingColor_ValidCard()
        {
            //Arrange
            var validCard = new Card(CardRank.Ace, CardSuit.Diamonds);

            //Act
            var result = _testGame.ValidPlay(_testCollection, validCard);

            //Assert
            //1. that the correct card is a valid play.
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GameCanPlayFromFlipped()
        {
            //Arrange
            var testStack = new Stack<Card>();
            testStack.Push(new Card(CardRank.Five, CardSuit.Clubs));
            testStack.Push(new Card(CardRank.Jack, CardSuit.Hearts));
            testStack.Push(new Card(CardRank.Ace, CardSuit.Diamonds));

            //Act
            _testGame.PlayFromFlipped(_testCollection, testStack);

            //Assert
            //1. The card was added to the collection.
            Assert.IsTrue(_testCollection.Last().Rank == CardRank.Ace, "The card was not added to the collection");
            //2. The card was removed from the stack.
            Assert.IsTrue(testStack.Peek().Rank != CardRank.Ace, "The card was not Pop()'d off the Stack");
        }

        [TestMethod]
        public void GameCanMovePileToPile()
        {

        }
    }
}
