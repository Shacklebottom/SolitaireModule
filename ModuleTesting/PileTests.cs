using static SolitaireDomain.EnumCardRank;
using static SolitaireDomain.EnumCardSuit;
using static SolitaireDomain.EnumCardColor;
using SolitaireDomain;

namespace ModuleTesting
{
    [TestClass]
    public class PileTests
    {
        //Global Test Variables
        Pile _testPile;

        [TestInitialize]
        public void PileTestsInitialize()
        {
            _testPile = new Pile();

            _testPile.Cards = new List<Card>()
            {
                        new (CardRank.Five, CardSuit.Diamonds),
                        new (CardRank.Four, CardSuit.Hearts),
                        new (CardRank.Three, CardSuit.Spades) { FaceUp = true },
                        new (CardRank.Two, CardSuit.Clubs) { FaceUp = true }
            };
        }

        [TestMethod]
        public void ValidatePlay_IsDescendingRankAndAlternatingColor_InvalidRank()
        {
            //Arrange
            var invalidCard = new Card(CardRank.Two, CardSuit.Diamonds);

            //Act
            var result = _testPile.ValidatePlay(_testPile.Cards, invalidCard);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidatePlay_IsDescendingRankAndAlternatingColor_InvalidColor()
        {
            //Arrange
            var invalidCard = new Card(CardRank.Ace, CardSuit.Spades);

            //Act
            var result = _testPile.ValidatePlay(_testPile.Cards, invalidCard);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidatePlay_IsDescendingRankAndAlternatingColor_ValidCard()
        {
            //Arrange
            var validCard = new Card(CardRank.Ace, CardSuit.Diamonds);

            //Act
            var result = _testPile.ValidatePlay(_testPile.Cards, validCard);

            //Assert
            //1. that the correct card is a valid play.
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidatePlay_EmptyPileWillOnlyAcceptAKing_InvalidCard()
        {
            //Arrange
            var emptyPile = new Pile(); //a new Pile automatically instantiates Cards property as a new List<Card>
            var invalidCard = new Card(CardRank.Seven, CardSuit.Spades);

            //Act
            var result = emptyPile.ValidatePlay(emptyPile.Cards, invalidCard);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidatePlay_EmptyPileWillOnlyAcceptAKing_ValidCard()
        {
            //Arrange
            var emptyPile = new Pile();
            var validCard = new Card(CardRank.King, CardSuit.Spades);

            //Act
            var result = emptyPile.ValidatePlay(emptyPile.Cards, validCard);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidatePlay_DoesntConsiderFaceDownCards()
        {
            //Arrange
            //A new Card instantiates FaceUp == false as default.
            var testPile = new Pile();
            testPile.Cards = new List<Card>()
                    {
                        new Card(CardRank.Six, CardSuit.Hearts),
                    };

            var testCard = new Card(CardRank.Five, CardSuit.Spades);

            //Act
            var result = testPile.ValidatePlay(testPile.Cards, testCard);

            //Assert
            //1. that this valid play is actually invalid because the testPile card is FaceUp == false (default instantiation).
            Assert.IsFalse(result);
        }
    }
}
