using SolitaireDomain;

namespace ModuleTesting.DomainObjTests
{
    [TestClass]
    public class GameTests
    {
        //Global Test Variables
        Player? player;
        Game? game;

        [TestInitialize]
        public void GameTestsInitialize()
        {
            player = new Player("Player 1");
            game = new Game(player);
        }

        [TestMethod]
        public void PlayerHasName()
        {
            //We're preserving the Player obj being passed in to the Game constructor :)

            //Arrange
            //1. Global Initalizer

            //Act

            //Assert
            Assert.AreEqual("Player 1", game?.Player.Name);
        }

        [TestMethod]
        public void ThereAreFourFoundations()
        {
            //Arrange
            //1. Global Initalizer

            //Act

            //Assert
            Assert.IsTrue(game?.Foundations.Length == 4);
        }

        [TestMethod]
        public void ThereAreSevenPiles()
        {
            //Arrange
            //1. Global Initalizer

            //Act

            //Assert
            Assert.IsTrue(game?.Piles.Length == 7);
        }

        [TestMethod]
        public void PilesHaveTheCorrectCount()
        {
            //Arrange
            //1. Global Initalizer
            List<bool> correctCount = [];

            //Act
            for (var i = 7; i > 0; i--)
            {
                if (game?.Piles[i - 1].Count == i)
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
            //1. Global Initalizer

            //Act
            var pileCardsSum = game?.Piles.ToList().Sum(c => c.Count);

            //Assert
            //1. The piles were collectively dealt 28 cards.
            Assert.AreEqual(28, pileCardsSum);
            //2. The deck has 24 of its 52 cards remaining.
            Assert.AreEqual(24, game?.Deck.Cards.Count);
        }

        [TestMethod]
        public void TheLastCardInEachPileIsFaceUp()
        {
            //Arrange
            //1. Global Initalizer
            List<bool> faceUpCount = [];

            //Act
            for (var i = 7; i > 0; i--)
            {
                if (game?.Piles[i - 1].Last().FaceUp == true)
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

        [TestMethod]
        public void GetPileOnlyGetsFaceUpCards()
        {
            //Arrange
            //1. Global Initalizer
            var faceUpCards = new List<Card>();

            //Act
            faceUpCards = game?.GetPile(0);

            //Assert
            Assert.AreEqual(true, faceUpCards?.TrueForAll(c => c.FaceUp == true));
        }
    }
}
