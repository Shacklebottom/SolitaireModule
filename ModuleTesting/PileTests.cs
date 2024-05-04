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

            _testPile.Cards = new List<Card>()
            {
                new(CardRank.Five, CardSuit.Diamonds),
                new(CardRank.Four, CardSuit.Hearts),
                new(CardRank.Three, CardSuit.Hearts) { FaceUp = true },
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
        public void ValidatePlay_ValidIsDescendingRankAndAlternatingColor_InvalidRank()
        {
            //Arrange
            var invalidCard = new Card(CardRank.Two, CardSuit.Diamonds);

            //Act
            var result = _testPile.ValidatePlay(invalidCard);

            //Assert
            Assert.IsFalse(result, "an invalid card was said to be valid");
        }

        [TestMethod]
        public void ValidatePlay_ValidIsDescendingRankAndAlternatingColor_InvalidColor()
        {
            //Arrange
            var invalidCard = new Card(CardRank.Ace, CardSuit.Spades);

            //Act
            var result = _testPile.ValidatePlay(invalidCard);

            //Assert
            Assert.IsFalse(result, "an invalid card was said to be valid");
        }

        [TestMethod]
        public void ValidatePlay_ValidIsDescendingRankAndAlternatingColor_ValidCard()
        {
            //Arrange
            var validCard = new Card(CardRank.Ace, CardSuit.Diamonds) { FaceUp = true };

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
            var validCard = new Card(CardRank.King, CardSuit.Spades) { FaceUp = true };

            //Act
            var result = _emptyPile.ValidatePlay(validCard);

            //Assert
            Assert.IsTrue(result, "a valid card was said to be invalid");
        }

        [TestMethod]
        public void ValidatePlay_DoesntConsiderFaceDownCards_InSourceCollection()
        {
            //Arrange
            var targetPile = new Pile();

            targetPile.Cards = new List<Card>
            {
                new Card(CardRank.Three, CardSuit.Spades) { FaceUp = true },
            };

            var invalidCard = new Card(CardRank.Two, CardSuit.Hearts);

            //Act
            var result = targetPile.ValidatePlay(invalidCard);

            //Assert
            //1. that this valid play is actually invalid because the invalidCard is FaceUp == false.
            Assert.IsFalse(result, "an invalid play was said to be valid");
        }

        [TestMethod]
        public void ValidatePlay_DoesntConsiderFaceDownCards_InTargetCollection()
        {
            //Arrange
            //A new Card instantiates FaceUp == false as default.
            var invalidTargetPile = new Pile();

            invalidTargetPile.Cards = new List<Card>
            {
                new Card(CardRank.Six, CardSuit.Hearts),
            };

            var validCard = new Card(CardRank.Five, CardSuit.Spades) { FaceUp = true };

            //Act
            var result = invalidTargetPile.ValidatePlay(validCard);

            //Assert
            //1. that this valid play is actually invalid because the invalidTargetPile card is FaceUp == false (default instantiation).
            Assert.IsFalse(result, "an invalid play was said to be valid");
        }
        #endregion

        #region ValidatePlay() ==PLAY TESTS==
        [TestMethod]
        public void ValidatePlay_CanPlayAValidKing()
        {
            //Arrange
            var validCard = new Card(CardRank.King, CardSuit.Clubs) { FaceUp = true };

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
            var validCard = new Card(CardRank.Ace, CardSuit.Diamonds) { FaceUp = true };

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

            sourcePile.Cards = new List<Card>
            {
                new Card(CardRank.Four, CardSuit.Hearts),
                validCard,
            };

            //Act
            _emptyPile.ValidatePlay(sourcePile.Cards.Last(), sourcePile);

            //Assert
            Assert.AreNotEqual(validCard, sourcePile.Cards.Last());
        }

        [TestMethod]
        public void ValidatePlay_ShouldRemoveTheValidCardFromTheSourcePile_ValidCard()
        {
            //Arrange
            var validCard = new Card(CardRank.Nine, CardSuit.Spades) { FaceUp = true };

            var sourcePile = new Pile();

            sourcePile.Cards = new List<Card>
            {
                new Card(CardRank.Four, CardSuit.Hearts),
                validCard
            };

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

            //Act
            _emptyPile.ValidatePlay(sourcePile.Cards.Last(), sourcePile);

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

        #region ValidateMove() ==VALIDATION TESTS==
        [TestMethod]
        public void ValidateMove_ValidIsDescendingRankAndAlternatingColor_InvalidRank()
        {
            //Arrange
            var invalidCard = new Card(CardRank.Nine, CardSuit.Hearts) { FaceUp = true };

            var sourcePile = new Pile();

            sourcePile.Cards = new List<Card>
            {
                invalidCard,
                new Card(CardRank.Eight, CardSuit.Spades) { FaceUp = true }
            };

            var targetPile = new Pile();

            targetPile.Cards = new List<Card>
            {
                new Card(CardRank.Jack, CardSuit.Spades) { FaceUp = true }
            };

            //Act
            var result = targetPile.ValidateMove(sourcePile, 0);

            //Assert
            Assert.IsFalse(result, "an invalid move was said to be a valid move");
        }

        [TestMethod]
        public void ValidateMove_ValidIsDescendingRankAndAlternatingColor_InvalidColor()
        {
            //Arrange
            var invalidCard = new Card(CardRank.Four, CardSuit.Hearts) { FaceUp = true };

            var sourcePile = new Pile();

            sourcePile.Cards = new List<Card>
            {
                invalidCard,
                new Card(CardRank.Three, CardSuit.Clubs) { FaceUp = true }
            };

            var targetPile = new Pile();

            targetPile.Cards = new List<Card>
            {
                new Card(CardRank.Five, CardSuit.Hearts) { FaceUp = true },
            };

            //Act
            var result = targetPile.ValidateMove(sourcePile, 0);

            //Assert
            Assert.IsFalse(result, "an invalid card was said to be valid");
        }

        [TestMethod]
        public void ValidateMove_ValidIsDescendingRankAndAlternatingColor_ValidCard()
        {
            //Arrange
            var validCard = new Card(CardRank.Ten, CardSuit.Clubs) { FaceUp = true };

            var sourcePile = new Pile();

            sourcePile.Cards = new List<Card>
            {
                validCard,
                new Card(CardRank.Nine, CardSuit.Diamonds) { FaceUp = true }
            };

            var targetPile = new Pile();

            targetPile.Cards = new List<Card>
            {
                new Card(CardRank.Jack, CardSuit.Hearts) { FaceUp = true }
            };

            //Act
            var result = targetPile.ValidateMove(sourcePile, 0);

            //Assert
            Assert.IsTrue(result, "a valid move was said to be invalid");
        }

        [TestMethod]
        public void ValidateMove_EmptyPileWillOnlyAcceptAKing_ValidCard()
        {
            //Arrange
            var validCard = new Card(CardRank.King, CardSuit.Spades) { FaceUp = true };

            var sourcePile = new Pile();

            sourcePile.Cards = new List<Card>
            {
                validCard,
                new Card(CardRank.Queen, CardSuit.Hearts) { FaceUp = true },
            };

            var targetPile = new Pile();

            //Act
            var result = targetPile.ValidateMove(sourcePile, 0);

            //Assert
            Assert.IsTrue(result, "a valid move was said to be invalid");
        }

        [TestMethod]
        public void ValidateMove_DoesntConsiderFaceDownCards_InSourceCollection()
        {
            //Arrange
            var invalidCard = new Card(CardRank.Nine, CardSuit.Spades);

            var sourcePile = new Pile();

            sourcePile.Cards = new List<Card>
            {
                invalidCard,
                new Card(CardRank.Five, CardSuit.Spades) { FaceUp = true },
            };

            var targetPile = new Pile();

            targetPile.Cards = new List<Card>
            {
                new Card(CardRank.Ten, CardSuit.Diamonds) { FaceUp = true },
            };

            //Act
            var result = targetPile.ValidateMove(sourcePile, 0);

            //Assert
            //1. that this valid move is actually invalid because the startingIndex yields a Card that is FaceUp == false.
            Assert.IsFalse(result, "an invalid move was said to be valid");
        }

        [TestMethod]
        public void ValidateMove_DoesntConsiderFaceDownCards_InTargetCollection()
        {
            //Arrange
            var validCard = new Card(CardRank.Seven, CardSuit.Diamonds) { FaceUp = true };
            
            var invalidTargetPile = new Pile();

            invalidTargetPile.Cards = new List<Card>
            {
                new Card(CardRank.Eight, CardSuit.Spades)
            };

            var sourcePile = new Pile();

            sourcePile.Cards = new List<Card>
            {
                validCard,
                new Card(CardRank.Six, CardSuit.Clubs) { FaceUp = true },
            };

            //Act
            var result = invalidTargetPile.ValidateMove(sourcePile, 0);

            //Assert
            //1. that this valid move is actually invalid because the invalidTargetPile card is FaceUp == false.
            Assert.IsFalse(result, "an invalid move was said to be valid");
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
