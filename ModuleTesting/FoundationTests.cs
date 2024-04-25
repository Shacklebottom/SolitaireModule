using static SolitaireDomain.EnumCardRank;
using static SolitaireDomain.EnumCardSuit;
using static SolitaireDomain.EnumCardColor;

using SolitaireDomain;

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

        [TestMethod]
        public void ValidatePlay_IsAscendingRankAndTheSameColor_InvalidRank()
        {
            //Arrange
            var invalidCard = new Card(CardRank.Five, CardSuit.Diamonds);

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
            var validCard = new Card(CardRank.Three, CardSuit.Diamonds);

            //Act
            var result = _testFoundation.ValidatePlay(_testFoundation.Cards, validCard);

            //Assert
            Assert.IsTrue(result);
        }


        [TestMethod]
        public void ValidatePlay_EmptyFoundationWillOnlyAcceptAnAce_ValidCard()
        {
            //Arrange
            var emptyFoundation = new Foundation(); //a new Foundation automatically instantiates Cards property as a new List<Card>

            var validCard = new Card(CardRank.Ace, CardSuit.Clubs);

            //Act
            var result = emptyFoundation.ValidatePlay(emptyFoundation.Cards, validCard);

            //Assert
            Assert.IsTrue(result);
        }


    }
}
