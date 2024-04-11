
using static SolitaireDomain.CardEnum;
using SolitaireDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection.Metadata.Ecma335;

namespace ModuleTesting
{
    [TestClass]
    public class GameTests
    {
        //Global Test Variables
        private Player _mockPlayer = new();
        private Game _mockGame = new();
        private List<Card> _mockCollection = [];

        [TestInitialize]
        public void GameTestsInitialize()
        {
            _mockPlayer = new Player("Player 1");
            _mockGame = new Game(_mockPlayer);
            _mockCollection = new List<Card>()
            {
                new (Rank.Five, Suit.Diamonds),
                new (Rank.Four, Suit.Hearts),
                new (Rank.Three, Suit.Spades) { FaceUp = true },
                new (Rank.Two, Suit.Clubs) { FaceUp = true }
            };
        }

        [TestMethod]
        public void PlayerHasName()
        {
            //We're preserving the Player obj being passed in to the Game constructor by the UI :)

            //Arrange
            //1. using _mockGame

            //Act

            //Assert
            Assert.AreEqual("Player 1", _mockGame.Player.Name);
        }

        [TestMethod]
        public void ThereAreFourFoundations()
        {
            //Arrange
            //1. using _mockGame

            //Act

            //Assert
            Assert.IsTrue(_mockGame.Foundations.Length == 4);
        }

        [TestMethod]
        public void ThereAreSevenPiles()
        {
            //Arrange
            //1. using _mockGame

            //Act

            //Assert
            Assert.IsTrue(_mockGame.Piles.Length == 7);
        }

        [TestMethod]
        public void PilesHaveTheCorrectCount()
        {
            //Arrange
            //1. using _mockGame

            //Act

            //Assert.Dominance
            //1. We are testing for Piles[index].Count == index + 1;
            Assert.IsTrue(_mockGame.Piles.Select((item, index) => new { item, index }).All(x => x.item.Count == x.index + 1),
                "At least 1 pile doesn't have the correct number of cards");
        }

        [TestMethod]
        public void EachPileOnlyHasOneFaceUpCard()
        {
            //Arrange
            //1. using _mockGame

            //Act

            //Assert.Dominance
            Assert.IsTrue(_mockGame.Piles.All(p => p.Count(c => c.FaceUp) == 1));
        }

        [TestMethod]
        public void PileCardCanBeTurnedFaceUp()
        {
            //Arrange
            //1. using _mockGame
            var pileIndex = 0;
            _mockGame.Piles[pileIndex].Last().FaceUp = false;

            //Act
            _mockGame.FlipPileCard(pileIndex);

            //Assert
            Assert.AreEqual(true, _mockGame.Piles[pileIndex].Last().FaceUp);
        }

        [TestMethod]
        public void GetPileOnlyGetsFaceUpCards()
        {
            //Arrange
            //1. using _mockGame

            //Act
            var faceUpCards = _mockGame.GetPile(0);

            //Assert
            Assert.IsTrue(faceUpCards.TrueForAll(c => c.FaceUp == true));
        }

        [TestMethod]
        public void ANumberOfCardsWereRevealed()
        {
            //Arrange
            //1. using _mockGame
            var drawCount = 3;

            //Act
            _mockGame.RevealFromDeck(drawCount);

            //Assert
            //1. The correct number of cards are present.
            Assert.AreEqual(drawCount, _mockGame.RevealedCards.Count);
            //2. and, each card revealed is FaceUp.
            Assert.IsTrue(_mockGame.RevealedCards.TrueForAll(c => c.FaceUp == true));
        }

        [TestMethod]
        public void PickedUpCardsAreAllFaceUp()
        {
            //Arrange
            //1. using _mockPlayer
            //2. using _mockGame
            //3. using _mockCollection

            //Act
            _mockPlayer.Holding = _mockGame.PickUpCards(_mockCollection);

            //Assert
            Assert.IsTrue(_mockPlayer.Holding.All(c => c.FaceUp == true));
        }

        [TestMethod]
        public void PlayerWillPutDownCardsOnlyOfDescendingRank()
        {
            //Arrange
            var mockPlayerHolding = new List<Card>()
            {
                new Card(Rank.Ace, Suit.Diamonds) { FaceUp = true },
            };
            _mockPlayer.Holding = mockPlayerHolding;

            //Act
            var inDescendingOrder = _mockGame.PutDownCards(_mockCollection);

            //Assert
            Assert.IsTrue(inDescendingOrder.Select((item, index) => new { item.Rank, index }).Skip(1).All(obj => obj.Rank == _mockCollection[obj.index - 1].Rank - 1));
        }
    }
}
