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

        Foundation _emptyFoundation;

        [TestInitialize]
        public void FoundationTestsInitialize()
        {
            _testFoundation = new Foundation();

            _testFoundation.Cards = new List<Card>()
            {
                new Card(CardRank.Ace, CardSuit.Diamonds),
                new Card(CardRank.Two, CardSuit.Diamonds)
            };

            _emptyFoundation = new Foundation(); //a new Foundation instantiates Cards property as a new List<Card> by default
        }

        #region ValidatePlay() ==VALIDATION TESTS==
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
            var validCard = new Card(CardRank.Ace, CardSuit.Clubs);

            //Act
            var result = _emptyFoundation.ValidatePlay(validCard);

            //Assert
            Assert.IsTrue(result);
        }
        #endregion

        #region ValidatePlay() ==PLAY TESTS==
        [TestMethod]
        public void ValidatePlay_CanPlayAValidAce()
        {
            //Arrange
            var validCard = new Card(CardRank.Ace, CardSuit.Clubs);

            //Act
            _emptyFoundation.ValidatePlay(validCard);

            //Assert
            //1. that ValidatePlay will play a valid Ace to a valid foundation.
            Assert.AreEqual(validCard, _emptyFoundation.Cards.First(), "the valid Ace was not played to the valid foundation");
        }

        [TestMethod]
        public void ValidatePlay_CanPlayAValidCard()
        {
            //Arrange
            var validCard = new Card(CardRank.Three, CardSuit.Hearts);

            //Act
            _testFoundation.ValidatePlay(validCard);

            //Assert
            //1. that ValidatePlay will play a valid card to a valid foundation
            Assert.AreEqual(validCard, _testFoundation.Cards.Last());
        }


        [TestMethod]
        public void ValidatePlay_ShouldRemoveTheValidCardFromTheSourceStack_ValidAce()
        {
            //Arrange
            var validCard = new Card(CardRank.Ace, CardSuit.Diamonds) { FaceUp = true };

            var cards = new List<Card>
            {
                new Card(CardRank.Three, CardSuit.Hearts) { FaceUp = true },
                validCard
            };
            var sourceDeck = new StandardDeck();

            sourceDeck.Flipped = new Stack<Card>(cards);

            //Act
            _emptyFoundation.ValidatePlay(sourceDeck.Flipped.Peek(), sourceDeck: sourceDeck);

            //Assert
            Assert.AreNotEqual(validCard, sourceDeck.Flipped.Peek());
        }

        [TestMethod]
        public void ValidatePlay_ShouldRemoveTheValidCardFromTheSourceStack_ValidCard()
        {
            //Arrange
            var validCard = new Card(CardRank.Three, CardSuit.Diamonds) { FaceUp = true };

            var cards = new List<Card>
            {
                new Card(CardRank.Three, CardSuit.Hearts) { FaceUp = true },
                validCard
            };
            var sourceDeck = new StandardDeck();

            sourceDeck.Flipped = new Stack<Card>(cards);

            //Act
            _testFoundation.ValidatePlay(sourceDeck.Flipped.Peek(), sourceDeck: sourceDeck);

            //Assert
            Assert.AreNotEqual(validCard, sourceDeck.Flipped.Peek());
        }

        [TestMethod]
        public void ValidatePlay_ShouldRemoveTheValidCardFromTheSourcePile_ValidAce()
        {
            //Arrange
            var validCard = new Card(CardRank.Ace, CardSuit.Spades) { FaceUp = true };

            var sourcePile = new Pile();

            sourcePile.Cards = new List<Card>
            {
                new Card(CardRank.Four, CardSuit.Hearts),
                validCard,
            };

            //Act
            _emptyFoundation.ValidatePlay(sourcePile.Cards.Last(), sourcePile);

            //Assert
            Assert.AreNotEqual(validCard, sourcePile.Cards.Last());
        }

        [TestMethod]
        public void ValidatePlay_ShouldRemoveTheValidCardFromTheSourcePile_ValidCard()
        {
            //Arrange
            var validCard = new Card(CardRank.Three, CardSuit.Diamonds) { FaceUp = true };

            var sourcePile = new Pile();

            sourcePile.Cards = new List<Card>
            {
                new Card(CardRank.Four, CardSuit.Hearts),
                validCard
            };

            //Act
            _testFoundation.ValidatePlay(sourcePile.Cards.Last(), sourcePile);

            //Assert
            Assert.AreNotEqual(validCard, sourcePile.Cards.Last());
        }

        [TestMethod]
        public void ValidatePlay_ShouldFlipFaceDownCardAfterPlaying_AnAceFromAPile()
        {
            //Arrange
            var sourcePile = new Pile();

            sourcePile.Cards = new List<Card>
            {
                new Card(CardRank.Four, CardSuit.Hearts),
                new Card(CardRank.Ace, CardSuit.Spades) { FaceUp = true },
            };

            //Act
            _emptyFoundation.ValidatePlay(sourcePile.Cards.Last(), sourcePile);

            //Assert
            //1. that the last card in the collection is FaceUp == true after a play has been made
            Assert.IsTrue(sourcePile.Cards.Last().FaceUp == true);
        }

        [TestMethod]
        public void ValidatePlay_ShouldFlipFaceDownCardAfterPlaying_ACardFromAPile()
        {
            //Arrange
            var sourcePile = new Pile();

            sourcePile.Cards = new List<Card>
            {
                new Card(CardRank.Four, CardSuit.Hearts),
                new Card(CardRank.Three, CardSuit.Diamonds) { FaceUp = true },
            };

            //Act
            _testFoundation.ValidatePlay(sourcePile.Cards.Last(), sourcePile);

            //Assert
            //1. that the last card in the collection is FaceUp == true after a play has been made
            Assert.IsTrue(sourcePile.Cards.Last().FaceUp == true);
        }
        #endregion


        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void ValidateMove_ThrowsANotImplementedException()
        {
            //Arrange
            var cards = new List<Card>();

            var startingIndex = 0;

            //Act
            _testFoundation.ValidateMove(cards, startingIndex);

            //Assert
            //1. that ValidateMove() will throw an exception because it doesn't get implemented for the Foundation class.
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void SetupCardCollection_ThrowsANotImplementedException()
        {
            //Arrange
            var cards = new List<Card>();

            //Act
            _testFoundation.SetupCardCollection(cards);

            //Assert
            //1. that SetupCardCollection() will throw an exception because it doesn't get implemented for the Foundation class.
        }
    }
}
