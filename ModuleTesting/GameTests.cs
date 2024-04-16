
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
        private List<Card>[] _parentOfPiles { get; set; } = [[], [], [], [], [], [], []];
        private List<Card>[] _parentOfFoundations { get; set; } = [[], [], [], []];
        private List<Card> _testPile { get; set; } = [];
        private List<Card> _testFoundation { get; set; } = [];

        [TestInitialize]
        public void GameTestsInitialize()
        {
            _testPlayer = new Player("Player 1");
            _testGame = new Game(_testPlayer);
            _testPile = new List<Card>()
            {
                new (CardRank.Five, CardSuit.Diamonds),
                new (CardRank.Four, CardSuit.Hearts),
                new (CardRank.Three, CardSuit.Spades) { FaceUp = true },
                new (CardRank.Two, CardSuit.Clubs) { FaceUp = true }
            };
            _testFoundation = new List<Card>()
            {
                new (CardRank.Ace, CardSuit.Hearts) { FaceUp = true },
                new (CardRank.Two, CardSuit.Spades) { FaceUp = true },
            };
        }
        #region Constructor and Instantiation Tests
        [TestMethod]
        public void Player_HasName()
        {
            //We're preserving the Player obj being passed in to the Game constructor by the UI :)

            //Arrange
            //1. using _testGame

            //Act
            //1. The Game() Constructor assigns a default name to the Player or the name the UI passes in.

            //Assert
            Assert.AreEqual("Player 1", _testGame.Player.Name);
        }

        [TestMethod]
        public void Foundations_AreFour()
        {
            //Arrange
            //1. using _testGame

            //Act

            //Assert
            Assert.IsTrue(_testGame.Foundations.Length == 4);
        }

        [TestMethod]
        public void Piles_AreSeven()
        {
            //Arrange
            //1. using _testGame

            //Act

            //Assert
            Assert.IsTrue(_testGame.Piles.Length == 7);
        }

        [TestMethod]
        public void Deck_WasShuffled_Operation()
        {
            //Arrange
            //Due to how the Deck obj and SetupPile() works, Piles.Last() will look like the testPile if the deck has not been shuffled,
            //so if this test fails for an unknown reason, run the test again to be sure :)
            var testPile = new List<Card>()
            {
                 new (CardRank.Ace, CardSuit.Hearts),
                 new (CardRank.Two, CardSuit.Hearts),
                 new (CardRank.Three, CardSuit.Hearts),
                 new (CardRank.Four, CardSuit.Hearts),
                 new (CardRank.Five, CardSuit.Hearts),
                 new (CardRank.Six, CardSuit.Hearts),
                 new (CardRank.Seven, CardSuit.Hearts) { FaceUp = true },
            };

            //Act
            //1. in the Game() constructor, Deck.Shuffle() is called.

            //Assert.Dominance
            Assert.IsFalse(testPile.SequenceEqual(_testGame.Piles.Last(), new CardEqualityComparer()));
        }

        [TestMethod]
        public void Piles_HaveTheCorrectCount_Operation()
        {
            //Arrange
            //1. using _testGame

            //Act
            //1. In the Game() Constructor, SetupPiles() is called, which deals out to each pile.

            //Assert.Dominance
            //1. We are testing for Piles[index].Count == index + 1;
            Assert.IsTrue(_testGame.Piles.Select((pile, index) => new { pile, index }).All(obj => obj.pile.Count == obj.index + 1),
                "At least 1 pile doesn't have the correct number of cards");
        }

        [TestMethod]
        public void Piles_HaveOneFaceUpCardEach_Operation()
        {
            //Arrange
            //1. using _testGame

            //Act
            //1. In the Game() Constructor, SetupPiles() is called, which turns the last card in each Pile FaceUp.

            //Assert.Dominance
            Assert.IsTrue(_testGame.Piles.All(p => p.Count(c => c.FaceUp) == 1));
        }
        #endregion

        #region FlipPileCard()
        [TestMethod]
        public void FlipPileCard_CanFlip()
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
        #endregion

        #region FlipFromDeck()
        [TestMethod]
        public void FlipFromDeck_CanFlip()
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
        #endregion

        #region ValidatePlay() Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ValidatePlay_ThrowsAnExceptionIfParentCollectionDoesntHaveTheRightNumberOfItems()
        {
            //Arrange
            var invalidParentCollection = new List<Card>[3];
            var testCard = new Card(CardRank.Two, CardSuit.Clubs);

            //Act
            Game.ValidatePlay(_testPile, testCard, invalidParentCollection);

            //Assert
            //1. that ValidatePlay will throw an out of range exception if the Pile or Foundation doesn't have the right number of members.
        }

        [TestMethod]
        public void ValidatePlay_ToAPile_IsDescendingRankAndAlternatingColor_FalseRank()
        {
            //Arrange
            var invalidCard = new Card(CardRank.Two, CardSuit.Diamonds);

            //Act
            var result = Game.ValidatePlay(_testPile, invalidCard, _parentOfPiles);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidatePlay_ToAPile_IsDescendingRankAndAlternatingColor_FalseColor()
        {
            //Arrange
            var invalidCard = new Card(CardRank.Ace, CardSuit.Spades);

            //Act
            var result = Game.ValidatePlay(_testPile, invalidCard, _parentOfPiles);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidatePlay_ToAPile_IsDescendingRankAndAlternatingColor_TrueCard()
        {
            //Arrange
            var validCard = new Card(CardRank.Ace, CardSuit.Diamonds);

            //Act
            var result = Game.ValidatePlay(_testPile, validCard, _parentOfPiles);

            //Assert
            //1. that the correct card is a valid play.
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidatePlay_EmptyPileWillOnlyAcceptAKing_FalsePile()
        {
            //Arrange
            var validCard = new Card(CardRank.King, CardSuit.Diamonds);
            var falsePile = new List<Card>()
            {
                new Card(CardRank.Six, CardSuit.Spades)
            };

            //Act
            var result = Game.ValidatePlay(falsePile, validCard, _parentOfPiles);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidatePlay_EmptyPileWillOnlyAcceptAKing_FalseCard()
        {
            //Arrange
            var truePile = new List<Card>();
            var invalidCard = new Card(CardRank.Seven, CardSuit.Spades);

            //Act
            var result = Game.ValidatePlay(truePile, invalidCard, _parentOfPiles);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidatePlay_EmptyPileWillOnlyAcceptAKing_TrueCard()
        {
            //Arrange
            var truePile = new List<Card>();
            var validCard = new Card(CardRank.King, CardSuit.Spades);

            //Act
            var result = Game.ValidatePlay(truePile, validCard, _parentOfPiles);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidatePlay_DoesntConsiderFaceDownCards()
        {
            //Arrange
            var testPile = new List<Card>()
            {
                new Card(CardRank.Six, CardSuit.Hearts),
            };
            var testCard = new Card(CardRank.Five, CardSuit.Spades);

            //Act
            var result = Game.ValidatePlay(testPile, testCard, _parentOfPiles);

            //Assert
            //1. that this valid play is invalid because the testPile card is FaceUp == false (default instantiation).
            Assert.IsFalse(result);
        }


        [TestMethod]
        public void ValidatePlay_ToAFoundation_IsAscendingRankAndAlternatingColor_FalseRank()
        {
            //Arrange
            var invalidCard = new Card(CardRank.Five, CardSuit.Diamonds);

            //Act
            var result = Game.ValidatePlay(_testFoundation, invalidCard, _parentOfFoundations);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidatePlay_ToAFoundation_IsAscendingRankAndAlternatingColor_FalseColor()
        {
            //Arrange
            var invalidCard = new Card(CardRank.Three, CardSuit.Spades);

            //Act
            var result = Game.ValidatePlay(_testFoundation, invalidCard, _parentOfFoundations);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidatePlay_ToAFoundation_IsAscendingRankAndAlternatingColor_TrueCard()
        {
            //Arrange
            var validCard = new Card(CardRank.Three, CardSuit.Diamonds);

            //Act
            var result = Game.ValidatePlay(_testFoundation, validCard, _parentOfFoundations);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidatePlay_EmptyFoundationWillOnlyAcceptAnAce_FalseCard()
        {
            //Arrange
            var testFoundation = new List<Card>();
            var invalidCard = new Card(CardRank.Two, CardSuit.Clubs);

            //Act
            var result = Game.ValidatePlay(testFoundation, invalidCard, _parentOfFoundations);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidatePlay_EmptyFoundationWillOnlyAcceptAnAce_TrueCard()
        {
            //Arrange
            var testFoundation = new List<Card>();
            var validCard = new Card(CardRank.Ace, CardSuit.Clubs);

            //Act
            var result = Game.ValidatePlay(testFoundation, validCard, _parentOfFoundations);

            //Assert
            Assert.IsTrue(result);
        }
        #endregion

        [TestMethod]
        public void PlayFromFlipped_WillValidatePlay()
        {
            //Arrange
            var testFlipped = new Stack<Card>();
            testFlipped.Push(new Card(CardRank.Five, CardSuit.Clubs) { FaceUp = true });
            testFlipped.Push(new Card(CardRank.Jack, CardSuit.Hearts) { FaceUp = true });
            testFlipped.Push(new Card(CardRank.Ace, CardSuit.Diamonds) { FaceUp = true });

            var testPile = new List<Card>();

            //Act
            _testGame.PlayFromFlipped(testPile, testFlipped, _parentOfPiles);

            //Assert
            //1. that this test will fail because ValidatePlay says the play is invalid.
            Assert.IsFalse(testPile.Count > 0);
        }

        [TestMethod]
        public void PlayFromFlipped_CanPlay()
        {
            //Arrange
            var testFlipped = new Stack<Card>();
            testFlipped.Push(new Card(CardRank.Five, CardSuit.Clubs) { FaceUp = true });
            testFlipped.Push(new Card(CardRank.Jack, CardSuit.Hearts) { FaceUp = true });
            testFlipped.Push(new Card(CardRank.Ace, CardSuit.Diamonds) { FaceUp = true });

            //Act
            _testGame.PlayFromFlipped(_testPile, testFlipped, _parentOfPiles);

            //Assert
            //1. The card was added to the collection.
            Assert.IsTrue(_testPile.Last().Rank == CardRank.Ace, "The card was not added to the collection");
            //2. The card was removed from the stack.
            Assert.IsTrue(testFlipped.Peek().Rank != CardRank.Ace, "The card was not Pop()'d off the Stack");
        }

        [TestMethod]
        public void MovePileToPile_WillValidateMove()
        {
            //Arrange
            var testTargetPile = new List<Card>();

            //Act
            _testGame.MovePileToPile(testTargetPile, _testPile, _parentOfPiles);

            //Assert
            //1. that this test will fail because ValidatePlay() says the move is invalid.
            Assert.IsFalse(testTargetPile.Count > 0);
        }

        [TestMethod]
        public void MovePileToPile_CanMove()
        {
            //Arrange


            //Act


            //Assert

        }

        [TestMethod]
        public void MovePileToPile_IgnoresFaceDownCards()
        {
            //Arrange


            //Act


            //Assert

        }
    }
}
