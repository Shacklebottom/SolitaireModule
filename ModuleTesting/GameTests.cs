
using SolitaireDomain;

namespace ModuleTesting
{
    [TestClass]
    public class GameTests
    {
        //Global Test Variables
        private Player _player = new();
        private Game _game = new();

        [TestInitialize]
        public void GameTestsInitialize()
        {
            _player = new Player("Player 1");
            _game = new Game(_player);
        }

        [TestMethod]
        public void PlayerHasName()
        {
            //We're preserving the Player obj being passed in to the Game constructor by the UI :)

            //Arrange
            //1. Global Initalizer

            //Act

            //Assert
            Assert.AreEqual("Player 1", _game.Player.Name);
        }

        [TestMethod]
        public void ThereAreFourFoundations()
        {
            //Arrange
            //1. Global Initalizer

            //Act

            //Assert
            Assert.IsTrue(_game.Foundations.Length == 4);
        }

        [TestMethod]
        public void ThereAreSevenPiles()
        {
            //Arrange
            //1. Global Initalizer

            //Act

            //Assert
            Assert.IsTrue(_game.Piles.Length == 7);
        }

        [TestMethod]
        public void PilesHaveTheCorrectCount()
        {
            //Arrange
            //1. Global Initalizer
            var error = false;

            //Act

            //Assert.Dominance
            for (var i = 6; i >= 0; i--)
            {
                if (!error)
                {
                    if (_game.Piles[i].Count != i + 1)
                    {
                        error = true;
                    }
                }
                else { break; }
            }
            //We are testing for (Index + 1) number of cards in each pile.
            Assert.AreEqual(false, error, "At least 1 pile doesn't have the correct number of cards");

            //Mentors says that I can assert dominance in this fashion, but I find this too hard to parse :P
            //Assert.IsTrue(game.Piles.Select((item, index) => new { item, index }).All(x => x.item.Count == x.index + 1));
        }

        [TestMethod]
        public void EachPileOnlyHasOneFaceUpCard()
        {
            //Arrange
            //1.Global Initalizer
            var expectedCount = 7;
            //Act

            //Assert
            Assert.IsTrue(_game.Piles.Select(p => p.Where(c => c.FaceUp)).Count() == expectedCount);
        }

        [TestMethod]
        public void PlayerCanFlipACardFaceUp()
        {
            //Arrange
            //1.Global Initalizer
            var pileIndex = 0;
            _game.Piles[pileIndex].Last().FaceUp = false;

            //Act
            _game.FlipPileCard(pileIndex);

            //Assert
            Assert.AreEqual(true, _game.Piles[pileIndex].Last().FaceUp);
        }

        [TestMethod]
        public void GetPileOnlyGetsFaceUpCards()
        {
            //Arrange
            //1. Global Initalizer

            //Act
            var faceUpCards = _game.GetPile(0);

            //Assert
            Assert.AreEqual(true, faceUpCards.TrueForAll(c => c.FaceUp == true));
        }

        [TestMethod]
        public void ANumberOfCardsWereRevealed()
        {
            //Arrange
            //1. Global Initalizer
            var drawCount = 3;

            //Act
            _game.RevealFromDeck(drawCount);

            //Assert
            Assert.AreEqual(drawCount, _game.RevealedCards.Count);
        }
    }
}
