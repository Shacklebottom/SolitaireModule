
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
            Assert.AreEqual("Player 1", _testGame.Player.Name);
        }

        [TestMethod]
        public void ThereAreFourFoundations()
        {
            //Arrange
            //1. using _mockGame

            //Act

            //Assert
            Assert.IsTrue(_testGame.Foundations.Length == 4);
        }

        [TestMethod]
        public void ThereAreSevenPiles()
        {
            //Arrange
            //1. using _mockGame

            //Act

            //Assert
            Assert.IsTrue(_testGame.Piles.Length == 7);
        }

        [TestMethod]
        public void PilesHaveTheCorrectCount()
        {
            //Arrange
            //1. using _mockGame

            //Act

            //Assert.Dominance
            //1. We are testing for Piles[index].Count == index + 1;
            Assert.IsTrue(_testGame.Piles.Select((item, index) => new { item, index }).All(x => x.item.Count == x.index + 1),
                "At least 1 pile doesn't have the correct number of cards");
        }

        [TestMethod]
        public void EachPileOnlyHasOneFaceUpCard()
        {
            //Arrange
            //1. using _mockGame

            //Act

            //Assert.Dominance
            Assert.IsTrue(_testGame.Piles.All(p => p.Count(c => c.FaceUp) == 1));
        }

        [TestMethod]
        public void PileCardCanBeTurnedFaceUp()
        {
            //Arrange
            //1. using _mockGame
            var pileIndex = 0;
            _testGame.Piles[pileIndex].Last().FaceUp = false;

            //Act
            _testGame.FlipPileCard(pileIndex);

            //Assert
            Assert.AreEqual(true, _testGame.Piles[pileIndex].Last().FaceUp);
        }

        [TestMethod]
        public void GetPileOnlyGetsFaceUpCards()
        {
            //Arrange
            //1. using _mockGame

            //Act
            var faceUpCards = _testGame.GetPile(0);

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
            _testGame.RevealFromDeck(drawCount);

            //Assert
            //1. The correct number of cards are present.
            Assert.AreEqual(drawCount, _testGame.RevealedCards.Count);
            //2. and, each card revealed is FaceUp.
            Assert.IsTrue(_testGame.RevealedCards.TrueForAll(c => c.FaceUp == true));
        }

        [TestMethod]
        public void PickedUpCardsAreAllFaceUp()
        {
            //Arrange
            //1. using _mockPlayer
            //2. using _mockGame
            //3. using _mockCollection

            //Act
            _testPlayer.Holding = _testGame.PickUpCards(_testCollection);

            //Assert
            Assert.IsTrue(_testPlayer.Holding.All(c => c.FaceUp == true));
        }

        [TestMethod]
        public void PutDownCardsOnlyChecksForFaceUpCards()
        {
            //Arrange
            var playerHoldering = new List<Card>()
            {
                new Card(Rank.Ace, Suit.Diamonds) { FaceUp = true },
            };
            _testPlayer.Holding = playerHoldering;

            //Act
            var onlyFaceUpCards = _testGame.PutDownCards(_testCollection);

            //Assert
            Assert.IsTrue(onlyFaceUpCards.TrueForAll(c => c.FaceUp == true));
        }

        [TestMethod]
        public void PlayerWillPutDownCardsOnlyOfDescendingRank()
        {
            //Arrange
            var playerHolding = new List<Card>()
            {
                new Card(Rank.Ace, Suit.Diamonds) { FaceUp = true },
            };
            _testPlayer.Holding = playerHolding;

            //Act
            var cardsPutDown = _testGame.PutDownCards(_testCollection);

            //Assert
            //1. that any card put down is of a descending rank. if it isn't, the card isn't put down and the original collection is returned.
            Assert.IsTrue(cardsPutDown.Select((item, index) => new { item.Rank, index }).Skip(1).All(obj => obj.Rank == cardsPutDown[obj.index - 1].Rank - 1));
        }
    }
}
