using SolitaireDomain;

namespace ModuleTesting
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
            //We're preserving the Player obj being passed in to the Game constructor by the UI :)

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

            //Assert.Dominance
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
            //1. Each pile, starting at the last and moving to the first, has 7 cards, then 6, then 5, [...], then 1.
            Assert.IsTrue(correctCount.TrueForAll(b => b == true));

            //Mentors says that I can assert dominance in this fashion, but I find this too hard to parse :P
            //Assert.IsTrue(game.Piles.Select((item, index) => new { item, index }).All(x => x.item.Count == x.index + 1));
        }

        [TestMethod]
        public void TheLastCardInEachPileIsFaceUp()
        {
            //Refactor note: add a new test for the FaceDown cards OR handle FaceDown cards here.
            //Refactor to check for the first Fail Condition instead of checking for every True Condition.

            //Arrange
            //1.Global Initalizer
            List<bool> faceUpCount = [];

            //Act

            //Assert
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
            Assert.IsTrue(faceUpCount.TrueForAll(b => b == true));

            //Mentors says that I can assert dominance in this fashion, but I find this too hard to parse :P
            //Assert.IsTrue(game?.Piles.All(p => p.Where(c => c.FaceUp).SequenceEqual([p.Last()])));
        }

        //[TestMethod]
        //public void GetPileOnlyGetsFaceUpCards()
        //{
        //    //Arrange
        //    //1. Global Initalizer

        //    //Act
        //    var faceUpCards = game?.GetPile(0);

        //    //Assert
        //    Assert.AreEqual(true, faceUpCards?.TrueForAll(c => c.FaceUp == true));
        //}
    }
}
