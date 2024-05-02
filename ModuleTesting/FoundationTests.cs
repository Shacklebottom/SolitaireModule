using static SolitaireDomain.Enums.EnumCardRank;
using static SolitaireDomain.Enums.EnumCardSuit;
using static SolitaireDomain.Enums.EnumCardColor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolitaireDomain.Objects;

namespace ModuleTesting
{
    [TestClass]
    public class FoundationTests
    {
        //Global Test Variables

        Foundation _testFoundation;

        [TestInitialize]
        public void FoundationTestsInitialize()
        {
            _testFoundation = new Foundation();

            _testFoundation.Cards = new List<Card>()
            {
                new Card(CardRank.Ace, CardSuit.Diamonds),
                new Card(CardRank.Two, CardSuit.Diamonds)
            };
        }

        #region ValidatePlay() tests
        [TestMethod]
        public void ValidatePlay_IsAscendingRankAndTheSameColor_InvalidRank()
        {
            //Arrange
            var invalidCard = new Card(CardRank.Five, CardSuit.Hearts);

            //Act
            var result = _testFoundation.ValidatePlay(invalidCard);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidatePlay_IsAscendingRankAndTheSameColor_InvalidColor()
        {
            //Arrange
            var invalidCard = new Card(CardRank.Three, CardSuit.Spades);

            //Act
            var result = _testFoundation.ValidatePlay(invalidCard);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidatePlay_IsAscendingRankAndTheSameColor_ValidCard()
        {
            //Arrange
            var validCard = new Card(CardRank.Three, CardSuit.Hearts);

            //Act
            var result = _testFoundation.ValidatePlay(validCard);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidatePlay_EmptyFoundationWillOnlyAcceptAnAce_ValidCard()
        {
            //Arrange
            var emptyFoundation = new Foundation(); //a new Foundation instantiates Cards property as a new List<Card> by default

            var validCard = new Card(CardRank.Ace, CardSuit.Clubs);

            //Act
            var result = emptyFoundation.ValidatePlay(validCard);

            //Assert
            Assert.IsTrue(result);
        }
        #endregion

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void SetupCardCollection_ThrowsANotImplementedException()
        {
            //Arrange
            var cards = new List<Card>();

            //Act
            _testFoundation.SetupCardCollection(cards);

            //Assert
            //1. that SetupCardCollection will throw an exception because it doesn't get implemented for the Foundation class.
        }
    }
}
