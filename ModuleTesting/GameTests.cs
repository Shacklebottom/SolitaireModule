
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
        public void PilesHaveTheCorrectCount()
        {
            //Arrange
            //1. using _testGame

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
            //1. using _testGame

            //Act

            //Assert.Dominance
            Assert.IsTrue(_testGame.Piles.All(p => p.Count(c => c.FaceUp) == 1));
        }

        [TestMethod]
        public void PileCardCanBeTurnedFaceUp()
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
        public void ANumberOfCardsWereFlipped()
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
        public void AValidPlayIsOfDescendingRankAndAlternatingColor()
        {
            //Arrange
            var validCard = new Card(CardRank.Ace, CardSuit.Diamonds);
            var invalidCard = new Card(CardRank.Two, CardSuit.Spades);

            //Act
            var testForCorrect = _testGame.ValidPlay(_testCollection, validCard);
            var testForIncorrect = _testGame.ValidPlay(_testCollection, invalidCard);

            //Assert
            //1. that the correct card is a valid play.
            Assert.IsTrue(testForCorrect);
            //2. and, that the incorrect card is an invalid play.
            Assert.IsFalse(testForIncorrect);
        }


        //[TestMethod]
        //public void GetPileOnlyGetsFaceUpCards()
        //{
        //    //Arrange
        //    //1. using _mockGame

        //    //Act
        //    var faceUpCards = _testGame.GetPile(0);

        //    //Assert
        //    Assert.IsTrue(faceUpCards.TrueForAll(c => c.FaceUp == true));
        //}
    }
}
