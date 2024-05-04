using static SolitaireDomain.Enums.EnumCardRank;
using static SolitaireDomain.Enums.EnumCardSuit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SolitaireDomain.Interfaces;
using SolitaireDomain.Extensions;
using SolitaireDomain.Objects;

namespace ModuleTesting
{
    [TestClass]
    public class GameTests
    {
        //Global Test Variables
        private Game _testGame;
        private Mock<IPlayer> _mockPlayer;
        private Mock<IDeckUnwrapper> _mockDeckUnwrapper;
        private Mock<ICardCollection>[] _mockFoundations;
        private Mock<ICardCollection>[] _mockPiles;

        [TestInitialize]
        public void Initialize_GameTest()
        {
            //player setup
            _mockPlayer = new Mock<IPlayer>();

            _mockPlayer.Setup(p => p.Name).Returns("Player 1");

            //deck setup
            _mockDeckUnwrapper = new Mock<IDeckUnwrapper>();

            //flippedCards setup
            var setupCards = new List<Card>
            {
                { new Card(CardRank.Three, CardSuit.Spades) { FaceUp = true } },
                { new Card(CardRank.Seven, CardSuit.Hearts) { FaceUp = true } }
            };
            _mockDeckUnwrapper.Setup(d => d.Flipped).Returns(new Stack<Card>(setupCards));

            //foundations setup
            _mockFoundations = new Mock<ICardCollection>[4];

            for (var i = 0; i < _mockFoundations.Length; i++)
            {
                _mockFoundations[i] = new Mock<ICardCollection>();
                _mockFoundations[i].Setup(f => f.Cards).Returns(new List<Card>());
            }

            //piles setup
            _mockPiles = new Mock<ICardCollection>[7];

            for (var i = 0; i < _mockPiles.Length; i++)
            {
                _mockPiles[i] = new Mock<ICardCollection>();
                _mockPiles[i].Setup(p => p.Cards).Returns(new List<Card>());
            }

            //game setup
            _testGame = new Game(_mockDeckUnwrapper.Object,
                _mockFoundations.Select(f => f.Object).ToArray(),
                _mockPiles.Select(p => p.Object).ToArray(),
                _mockPlayer.Object);
        }

        #region Constructor and Instantiation Tests
        [TestMethod]
        public void Constructor_ShouldSetPlayer()
        {
            //Arrange

            //Act
            var name = _testGame.Player.Name;

            //Assert
            Assert.AreEqual("Player 1", name);
        }

        [TestMethod]
        public void Foundations_AreFour()
        {
            //Arrange
            var expectedCount = 4;

            //Act
            var foundationCount = _testGame.Foundations.Length;

            //Assert
            Assert.AreEqual(expectedCount, foundationCount);
        }

        [TestMethod]
        public void Piles_AreSeven()
        {
            //Arrange
            var expectedCount = 7;

            //Act
            var pilesCount = _testGame.Piles.Length;

            //Assert
            Assert.AreEqual(expectedCount, pilesCount);
        }

        [TestMethod]
        public void Constructor_DeckWasShuffled_Once()
        {
            //Arrange

            //Act

            //Assert
            //1. that deck.Shuffle() was called exactly once by the Game() constructor.
            _mockDeckUnwrapper.Verify(d => d.Shuffle(), Times.Once);
        }

        [TestMethod]
        public void Constructor_SetupCardCollectionWasCalled_OnceForEachPile()
        {
            //Arrange

            //Act

            //Assert
            //1. that Piles[i].SetupCardCollection was called once for each pile.
            _mockPiles.ForEach(
                p => p.Verify(
                    c => c.SetupCardCollection(It.IsAny<List<Card>>()),
                    Times.Once,
                    "the method wasn't called once for each pile in piles"));
        }

        [TestMethod]
        public void Constructor_DrawWasCalled_OnceForEachPile()
        {
            //Arrange

            //Act

            //Assert
            //1. that Deck.Draw() was called once for each pile.
            _mockDeckUnwrapper.Verify(d => d.Draw(It.IsAny<int>()), Times.Exactly(_mockPiles.Length));
        }

        [TestMethod]
        public void Constructor_PilesHaveTheCorrectNumberOfElements()
        {
            //Arrange

            //Act

            //Assert
            //1. that each pile will contain a number of elements equal to its index + 1.
            for (var i = 0; i < _mockPiles.Length; ++i)
            {
                _mockDeckUnwrapper.Verify(d => d.Draw(i + 1), Times.Once);
            }
        }
        #endregion


        #region GameOver()
        [TestMethod]
        public void GameOver_PlayerCanWin_InvalidPiles()
        {
            //Arrange
            _mockDeckUnwrapper.Setup(d => d.Cards).Returns(new List<Card>());

            _mockDeckUnwrapper.Setup(d => d.Flipped).Returns(new Stack<Card>());

            _mockPiles.ForEach(p => p.Setup(c => c.Cards).Returns(new List<Card>()));

            _mockPiles[0].Setup(c => c.Cards).Returns(new List<Card> { new Card(CardRank.Ace, CardSuit.Hearts) { FaceUp = true } });

            //Act
            var result = _testGame.GameOver();

            //Assert
            //1. that GameOver() is false because one of the Piles has one or more elements.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GameOver_PlayerCanWin_InvalidDeck()
        {
            //Arrange
            _mockDeckUnwrapper.Setup(d => d.Cards).Returns(new List<Card> { new Card(CardRank.Three, CardSuit.Spades) });

            _mockDeckUnwrapper.Setup(d => d.Flipped).Returns(new Stack<Card>());

            _mockPiles.ForEach(p => p.Setup(c => c.Cards).Returns(new List<Card>()));

            //Act
            var result = _testGame.GameOver();

            //Assert
            //1. that GameOver() is false because the Deck has one or more elements.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GameOver_PlayerCanWin_InvalidFlipped()
        {
            //Arrange
            _mockDeckUnwrapper.Setup(d => d.Cards).Returns(new List<Card>());

            var card = new Stack<Card>();
            card.Push(new Card(CardRank.Three, CardSuit.Spades));

            _mockDeckUnwrapper.Setup(d => d.Flipped).Returns(new Stack<Card>(card));

            _mockPiles.ForEach(p => p.Setup(c => c.Cards).Returns(new List<Card>()));

            //Act
            var result = _testGame.GameOver();

            //Assert
            //1. that GameOver() is false because testFlipped has one or more elements.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GameOver_PlayerCanWin_ValidCollections()
        {
            //Arrange
            _mockDeckUnwrapper.Setup(d => d.Cards).Returns(new List<Card>());

            _mockDeckUnwrapper.Setup(d => d.Flipped).Returns(new Stack<Card>());

            _mockPiles.ForEach(p => p.Setup(c => c.Cards).Returns(new List<Card>()));

            //Act
            var result = _testGame.GameOver();

            //Assert
            //1. that GameOver() is true if the Piles, Deck, _and_ Flipped are empty.
            Assert.IsTrue(result);
        }
        #endregion
    }
}
