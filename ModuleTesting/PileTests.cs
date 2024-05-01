using static SolitaireDomain.Enums.EnumCardRank;
using static SolitaireDomain.Enums.EnumCardSuit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolitaireDomain.Objects;
using SolitaireDomain.Comparers;

namespace ModuleTesting
{
    [TestClass]
    public class PileTests
    {
        //Global Test Variables
        private Pile _testPile;
        private Pile _emptyPile;
        private List<Card> _cards;

        [TestInitialize]
        public void Initialize_PileTest()
        {
            _testPile = new Pile();

            _emptyPile = new Pile();

            _testPile.Cards = new List<Card>()
            {
                new(CardRank.Five, CardSuit.Diamonds),
                new(CardRank.Four, CardSuit.Hearts),
                new(CardRank.Three, CardSuit.Spades) { FaceUp = true },
                new(CardRank.Two, CardSuit.Clubs) { FaceUp = true }
            };

            _emptyPile.Cards = new List<Card>();

            _cards = new List<Card>()
            {
                new(CardRank.Four, CardSuit.Spades),
                new(CardRank.Three, CardSuit.Hearts),
                new(CardRank.Ten, CardSuit.Hearts),
                new(CardRank.Seven, CardSuit.Clubs)
            };
        }

        #region ValidatePlay() ==VALIDATION TESTS==
        [TestMethod]
        public void ValidatePlay_IsDescendingRankAndAlternatingColor_InvalidRank()
        {
            //Arrange
            var invalidCard = new Card(CardRank.Two, CardSuit.Diamonds);

            //Act
            var result = _testPile.ValidatePlay(invalidCard);

            //Assert
            Assert.IsFalse(result, "an invalid card was said to be valid");
        }

        [TestMethod]
        public void ValidatePlay_IsDescendingRankAndAlternatingColor_InvalidColor()
        {
            //Arrange
            var invalidCard = new Card(CardRank.Ace, CardSuit.Spades);

            //Act
            var result = _testPile.ValidatePlay(invalidCard);

            //Assert
            Assert.IsFalse(result, "an invalid card was said to be valid");
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
            Assert.IsTrue(result, "a valid card was said to be invalid");
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
            Assert.IsTrue(result, "a valid card was said to be invalid");
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
            Assert.IsFalse(result, "an invalid play was said to be valid");
        }
        #endregion

        #region ValidatePlay() ==PLAY TESTS==



        #endregion
        
        
        #region SetupCardCollection() Tests
        [TestMethod]
        public void SetupCardCollection_CanSetup()
        {
            //Arrange

            //Act
            _emptyPile.SetupCardCollection(_cards);

            //Assert
            //1. that SetupCardCollection added cards to the pile, and
            Assert.IsTrue(_emptyPile.Cards.SequenceEqual(_cards, new CardEqualityComparer()), "the cards were not added to the collection.");

        }

        [TestMethod]
        public void SetupCardCollection_CollectionHasASingleFaceUpCard()
        {
            //Arrange

            //Act
            _emptyPile.SetupCardCollection(_cards);

            //Assert
            Assert.IsTrue(_emptyPile.Cards.Count(c => c.FaceUp == true) == 1, "there was not exactly one FaceUp card in the collection");
        }

        [TestMethod]
        public void SetupCardCollection_TheFaceUpCardIsTheLastCard()
        {
            //Arrange

            //Act
            _emptyPile.SetupCardCollection(_cards);

            //Assert
            //1. that the last card in the pile is FaceUp == true.
            Assert.IsTrue(_emptyPile.Cards.Last().FaceUp == true, "the last element in the collection is not FaceUp");
        }
        #endregion
    }
}
