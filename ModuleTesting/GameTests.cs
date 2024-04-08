using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolitaireDomain;
using System.Numerics;
using static SolitaireDomain.CardEnum;

namespace ModuleTesting
{
    [TestClass]
    public class GameTests
    {
        [TestMethod]
        public void PlayerHasName()
        {
            //We're preserving the Player obj being passed in to the Game constructor :)
            //Arrange
            var player = new Player("Player 1");
            var game = new Game(player);

            //Act

            //Assert
            Assert.AreEqual("Player 1", game.Player.Name);
        }

        [TestMethod]
        public void ThereAreFourFoundations()
        {
            //Arrange
            var game = new Game(new Player("Player 1"));

            //Act

            //Assert
            Assert.IsTrue(game.Foundations.Length == 4);
        }

        [TestMethod]
        public void ThereAreSevenPiles()
        {
            //Arrange
            var game = new Game(new Player("Player 1"));

            //Act

            //Assert
            Assert.IsTrue(game.Piles.Length == 7);
        }

        [TestMethod]
        public void PilesHaveTheCorrectCount()
        {
            //Arrange
            var game = new Game(new Player("Player 1"));
            List<bool> correctCount = [];

            //Act
            for (var i = 7; i > 0; i--)
            {
                if (game.Piles[i - 1].Count == i)
                {
                    correctCount.Add(true);
                }
                else
                {
                    correctCount.Add(false);
                }
            }

            //Assert.Dominance
            //1. Each pile, starting at the last and moving to the first, has 7 cards, then 6, then 5, [...], then 1.
            Assert.IsTrue(correctCount.TrueForAll(b => b == true));
        }

        [TestMethod]
        public void PilesHaveTwentyEightCardsTotal()
        {
            //Arrange
            var game = new Game(new Player("Player 1"));

            //Act
            var pileCardsSum = game.Piles.ToList().Sum(c => c.Count);

            //Assert
            //1. The piles were collectively dealt 28 cards.
            Assert.AreEqual(28, pileCardsSum);
            //2. The deck has 24 of its 52 cards remaining.
            Assert.AreEqual(24, game.Deck.Cards.Count);
        }

        [TestMethod]
        public void TheLastCardInEachPileIsFaceUp()
        {
            //Arrange
            var game = new Game(new Player(""));

            List<bool> faceUpCount = [];

            //Act
            for (var i = 7; i > 0; i--)
            {
                if (game.Piles[i - 1].Last().FaceUp == true)
                {
                    faceUpCount.Add(true);
                }
                else
                {
                    faceUpCount.Add(false);
                }
            }

            //Assert
            Assert.IsTrue(faceUpCount.TrueForAll(b => b == true));
        }
    }
}
