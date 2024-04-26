using static SolitaireDomain.Enums.EnumCardRank;
using static SolitaireDomain.Enums.EnumCardSuit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolitaireDomain.Objects;

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
                new(CardRank.Five, CardSuit.Diamonds),
                new(CardRank.Four, CardSuit.Hearts),
                new(CardRank.Three, CardSuit.Spades) { FaceUp = true },
                new(CardRank.Two, CardSuit.Clubs) { FaceUp = true }
            };
        }

        #region ValidatePlay() tests
        [TestMethod]
        public void ValidatePlay_IsDescendingRankAndAlternatingColor_InvalidRank()
        {
            //Arrange
            var invalidCard = new Card(CardRank.Two, CardSuit.Diamonds);

            //Act
            var result = _testPile.ValidatePlay(invalidCard);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidatePlay_IsDescendingRankAndAlternatingColor_InvalidColor()
        {
            //Arrange
            var invalidCard = new Card(CardRank.Ace, CardSuit.Spades);

            //Act
            var result = _testPile.ValidatePlay(invalidCard);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidatePlay_IsDescendingRankAndAlternatingColor_ValidCard()
        {
            //Arrange
            var validCard = new Card(CardRank.Ace, CardSuit.Diamonds);

            //Act
            var result = _testPile.ValidatePlay(validCard);

            //Assert
            //1. that the correct card is a valid play.
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidatePlay_EmptyPileWillOnlyAcceptAKing_ValidCard()
        {
            //Arrange
            var emptyPile = new Pile();
            var validCard = new Card(CardRank.King, CardSuit.Spades);

            //Act
            var result = emptyPile.ValidatePlay(validCard);

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

            var testCard = new Card(CardRank.Five, CardSuit.Spades) { FaceUp = true };

            //Act
            var result = testPile.ValidatePlay(testCard);

            //Assert
            //1. that this valid play is actually invalid because the testPile card is FaceUp == false (default instantiation).
            Assert.IsFalse(result);
        }
        #endregion

        [TestMethod]
        public void SetupCardCollection_CanSetup()
        {
            //Arrange
            Pile emptyPile = new Pile();

            emptyPile.Cards = new List<Card>();

            var cards = new List<Card>()
            {
                new(CardRank.Four, CardSuit.Spades),
                new(CardRank.Three, CardSuit.Hearts),
                new(CardRank.Ten, CardSuit.Hearts),
                new(CardRank.Seven, CardSuit.Clubs)
            };

            //Act
            emptyPile.SetupCardCollection(cards);

            //Assert
            //1. that SetupCardCollection added cards to the pile, and
            Assert.IsTrue(emptyPile.Cards.SequenceEqual(cards, new CardEqualityComparer()), "the cards were not added to the collection.");
            //2. that the last card in the pile is FaceUp == true.
            Assert.IsTrue(emptyPile.Cards.Last().FaceUp == true, "the last element in the collection is not FaceUp");
        }
    }
}
