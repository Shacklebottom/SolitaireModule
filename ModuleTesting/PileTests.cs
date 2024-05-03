using static SolitaireDomain.Enums.EnumCardRank;
using static SolitaireDomain.Enums.EnumCardSuit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolitaireDomain.Objects;
using SolitaireDomain.Comparers;
using SolitaireDomain.Interfaces;

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

            _testPile.Cards = new List<Card>()
            {
                new(CardRank.Five, CardSuit.Diamonds),
                new(CardRank.Four, CardSuit.Hearts),
                new(CardRank.Three, CardSuit.Spades) { FaceUp = true },
                new(CardRank.Two, CardSuit.Clubs) { FaceUp = true }
            };

            _emptyPile = new Pile(); //a new Pile instantiates Cards property as a new List<Card> by default

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
            var validCard = new Card(CardRank.King, CardSuit.Spades);

            //Act
            var result = _emptyPile.ValidatePlay(validCard);

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
        [TestMethod]
        public void ValidatePlay_CanPlayAValidKing()
        {
            //Arrange
            var validCard = new Card(CardRank.King, CardSuit.Clubs);

            //Act
            _emptyPile.ValidatePlay(validCard);

            //Assert
            //1. that ValidatePlay will play a valid King to a valid pile.
            Assert.AreEqual(validCard, _emptyPile.Cards.First(), "the valid King was not played to the valid pile");
        }

        [TestMethod]
        public void ValidatePlay_CanPlayAValidCard()
        {
            //Arrange
            var validCard = new Card(CardRank.Ace, CardSuit.Diamonds);

            //Act
            _testPile.ValidatePlay(validCard);

            //Assert
            //1. that ValidatePlay will play a valid card to a valid pile.
            Assert.AreEqual(validCard, _testPile.Cards.Last());
        }

        [TestMethod]
        public void ValidatePlay_ShouldRemoveTheValidCardFromTheSourceStack_ValidKing()
        {
            //Arrange
            var validCard = new Card(CardRank.King, CardSuit.Diamonds) { FaceUp = true };

            var cards = new List<Card>
            {
                new Card(CardRank.Three, CardSuit.Hearts) { FaceUp = true },
                validCard
            };
            var sourceDeck = new StandardDeck();

            sourceDeck.Flipped = new Stack<Card>(cards);

            //Act
            _emptyPile.ValidatePlay(sourceDeck.Flipped.Peek(), sourceDeck: sourceDeck);

            //Assert
            Assert.AreNotEqual(validCard, sourceDeck.Flipped.Peek());
        }

        [TestMethod]
        public void ValidatePlay_ShouldRemoveTheValidCardFromTheSourceStack_ValidCard()
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
            _testPile.ValidatePlay(sourceDeck.Flipped.Peek(), sourceDeck: sourceDeck);

            //Assert
            Assert.AreNotEqual(validCard, sourceDeck.Flipped.Peek());
        }

        [TestMethod]
        public void ValidatePlay_ShouldRemoveTheValidCardFromTheSourcePile_ValidKing()
        {
            //Arrange
            var validCard = new Card(CardRank.King, CardSuit.Spades) { FaceUp = true };

            var sourcePile = new Pile();

            sourcePile.Cards =
            [
                new Card(CardRank.Four, CardSuit.Hearts), 
                validCard,
            ];

            var targetPile = new Pile();

            //Act
            targetPile.ValidatePlay(sourcePile.Cards.Last(), sourcePile);

            //Assert
            Assert.AreNotEqual(validCard, sourcePile.Cards.Last());
        }

        [TestMethod]
        public void ValidatePlay_ShouldRemoveTheValidCardFromTheSourcePile_AValidCard()
        {
            //Arrange
            var validCard = new Card(CardRank.Nine, CardSuit.Spades) { FaceUp = true };

            var sourcePile = new Pile();

            sourcePile.Cards =
            [
                new Card(CardRank.Four, CardSuit.Hearts),
                validCard,
            ];

            var targetPile = new Pile();

            targetPile.Cards = new List<Card>
            {
                new Card(CardRank.Ten, CardSuit.Hearts) { FaceUp = true }
            };

            //Act
            targetPile.ValidatePlay(sourcePile.Cards.Last(), sourcePile);

            //Assert
            Assert.AreNotEqual(validCard, sourcePile.Cards.Last());
        }

        [TestMethod]
        public void ValidatePlay_ShouldFlipFaceDownCardAfterPlaying_AKingFromAPile()
        {
            //Arrange
            var sourcePile = new Pile();
            
            sourcePile.Cards = new List<Card>
            {
                new Card(CardRank.Four, CardSuit.Hearts),
                new Card(CardRank.King, CardSuit.Spades) { FaceUp = true },
            };

            var targetPile = new Pile();

            //Act
            targetPile.ValidatePlay(sourcePile.Cards.Last(), sourcePile);

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
                new Card(CardRank.Seven, CardSuit.Spades) { FaceUp = true },
            };

            var targetPile = new Pile();

            targetPile.Cards = new List<Card>
            {
                new Card(CardRank.Eight, CardSuit.Hearts) { FaceUp = true }
            };

            //Act
            targetPile.ValidatePlay(sourcePile.Cards.Last(), sourcePile);

            //Assert
            //1. that the last card in the collection is FaceUp == true after a play has been made
            Assert.IsTrue(sourcePile.Cards.Last().FaceUp == true);
        }
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
