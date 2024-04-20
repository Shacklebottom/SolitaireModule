
using static SolitaireDomain.EnumCardRank;
using static SolitaireDomain.EnumCardSuit;
using SolitaireDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ModuleTesting
{
    [TestClass]
    public class GameTests
    {
        //LOOKIT HERE BUDDY (IM NOT YOUR BUDDY, FRIEND), Change ValidatePlay() away from a static method because Mock framework can't handle static methods.
        //Also, you may be missing nuances when testing variables outside the Game obj

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

        //probably use a test to check that the last card is the face up card in each pile.
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
            //store how many cards are in flipped and then flip more and compare the two values.

            //Act
            _testGame.FlipFromDeck(drawCount);

            //Assert
            //1. The correct number of cards are present.
            Assert.AreEqual(drawCount, _testGame.FlippedCards.Count);
            //2. and, each card revealed is FaceUp.
            Assert.IsTrue(_testGame.FlippedCards.ToList().TrueForAll(c => c.FaceUp == true));
        }
        #endregion

        #region ValidatePlay()
        //NOTE! Change Piles and Foundations into their own objs, which then can have a ValidatePlay(), and also make test instantiation easier and more clean.

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ValidatePlay_ThrowsAnException_IfParentCollectionDoesntHaveTheRightNumberOfItems()
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
            //redundant!
            //Arrange
            var validCard = new Card(CardRank.King, CardSuit.Diamonds);
            var notEmptyPile = new List<Card>()
            {
                new Card(CardRank.Six, CardSuit.Spades)
            };

            //Act
            var result = Game.ValidatePlay(notEmptyPile, validCard, _parentOfPiles);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidatePlay_EmptyPileWillOnlyAcceptAKing_FalseCard()
        {
            //Arrange
            var emptyPile = new List<Card>();
            var invalidCard = new Card(CardRank.Seven, CardSuit.Spades);

            //Act
            var result = Game.ValidatePlay(emptyPile, invalidCard, _parentOfPiles);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidatePlay_EmptyPileWillOnlyAcceptAKing_TrueCard()
        {
            //Arrange
            var emptyPile = new List<Card>();
            var validCard = new Card(CardRank.King, CardSuit.Spades);

            //Act
            var result = Game.ValidatePlay(emptyPile, validCard, _parentOfPiles);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidatePlay_DoesntConsiderFaceDownCards()
        {
            //Arrange
            //card default FaceUp == false (default instantiation).

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
            //Based on the ruleset I was reading, I made an assumption that Foundations were ALSO filled using alternating color. This is not the case
            //and we're checking just for ascending rank. OOPS LMAO.
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

        #region PlayFromFlipped()
        [TestMethod]
        public void PlayFromFlipped_WillValidatePlay_InvalidPlay()
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
            //1. that this test will be false because ValidatePlay says the play is invalid.
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
        #endregion

        #region MovePile()
        [TestMethod]
        public void MovePile_WillValidateMove_InvalidMove()
        {
            //Arrange
            var testTargetPile = new List<Card>();

            var startingIndex = 2;

            //Act
            _testGame.MovePile(testTargetPile, _testPile, startingIndex, _parentOfPiles);

            //Assert
            //1. that this test will be false because ValidatePlay() says the move is invalid.
            Assert.IsFalse(testTargetPile.Count > 0);
        }

        [TestMethod]
        public void MovePile_CanMove_ToAPile_BasedOnStartingIndex()
        {
            //NOTE!: this test could be better constructed to look at the actual contents of each list to ensure that the rules were properly addressed.

            //Arrange
            var testTargetPile = new List<Card>();

            var startingIndex = 2;

            var testPile = new List<Card>()
            {
                new(CardRank.Four, CardSuit.Hearts),
                new(CardRank.Six, CardSuit.Diamonds),
                new(CardRank.King, CardSuit.Spades) { FaceUp = true },
                new(CardRank.Queen, CardSuit.Hearts) { FaceUp = true },
                new(CardRank.Jack, CardSuit.Clubs) { FaceUp = true },
            };

            //Act
            _testGame.MovePile(testTargetPile, testPile, startingIndex, _parentOfPiles);

            //Assert
            //1. That the cards were added to the collection, and
            Assert.IsTrue(testTargetPile.First().Rank == CardRank.King, "the collection was not properly added to the target collection");
            Assert.IsTrue(testTargetPile.Count == 3, "the FaceUp == false cards were added to the collection");
            //2. that the cards were removed from their source pile.
            Assert.IsFalse(testTargetPile.Any(testPile.Contains), "the collection was not removed from its source");
        }

        [TestMethod]
        public void MovePile_CanMove_ToAFoundation_BasedOnEndingIndex()
        {
            //NOTE!: this test could be better constructed to look at the actual contents of each list to ensure that the rules were properly addressed.

            //Arrange
            var testTargetFoundation = new List<Card>();

            var endingIndex = 2;

            var testPile = new List<Card>()
            {
                new(CardRank.Four, CardSuit.Hearts),
                new(CardRank.Six, CardSuit.Diamonds),
                new(CardRank.Three, CardSuit.Spades) { FaceUp = true },
                new(CardRank.Two, CardSuit.Hearts) { FaceUp = true },
                new(CardRank.Ace, CardSuit.Clubs) { FaceUp = true },
            };

            //Act
            _testGame.MovePile(testTargetFoundation, testPile, endingIndex, _parentOfFoundations);

            //Assert
            //1. That the cards were added to the collection, and
            Assert.IsTrue(testTargetFoundation.First().Rank == CardRank.Ace, "the collection was not properly added to the target collection");
            Assert.IsTrue(testTargetFoundation.Count == 3, "the FaceUp == false cards were added to the collection");
            //2. that the cards were removed from their source pile.
            Assert.IsFalse(testTargetFoundation.Any(testPile.Contains), "the collection was not removed from its source");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MovePile_ThrowsAnException_IfTheSelectedCardsContainAFaceDownCard()
        {
            //Arrange
            var testTargetPile = new List<Card>();

            var startingIndex = 0;

            var testPile = new List<Card>()
            {
                new(CardRank.Four, CardSuit.Hearts),
                new(CardRank.Six, CardSuit.Diamonds),
                new(CardRank.King, CardSuit.Spades) { FaceUp = true },
                new(CardRank.Queen, CardSuit.Hearts) { FaceUp = true },
                new(CardRank.Jack, CardSuit.Clubs) { FaceUp = true },
            };

            //Act
            _testGame.MovePile(testTargetPile, testPile, startingIndex, _parentOfPiles);

            //Assert
            //1. that MovePile() will throw an exception if the startingIndex or endingIndex selects a FaceUp == false card as part of its selection.
        }
        #endregion

        #region GameOver()
        [TestMethod]
        public void GameOver_PlayerCanWin_InvalidPiles()
        {
            //Arrange
            var testDeck = new List<Card>();

            var testFlipped = new Stack<Card>();

            List<Card>[] testPiles = [ [], [], [], [], [], [], [] ];
            testPiles[0].Add(new Card(CardRank.Ace, CardSuit.Hearts));

            //Act
            var result = _testGame.GameOver(testPiles, testDeck, testFlipped);

            //Assert
            //1. that GameOver() is false because one of the Piles has one or more elements.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GameOver_PlayerCanWin_InvalidDeck()
        {
            //Arrange
            var testDeck = new List<Card>()
            {
                new Card(CardRank.Eight, CardSuit.Diamonds)
            };
            var testFlipped = new Stack<Card>();

            //Act
            var result = _testGame.GameOver(_parentOfPiles, testDeck, testFlipped);

            //Assert
            //1. that GameOver() is false because testDeck has one or more elements.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GameOver_PlayerCanWin_InvalidFlipped()
        {
            //Arrange
            var testDeck = new List<Card>();

            var testFlipped = new Stack<Card>();
            testFlipped.Push(new Card(CardRank.Seven, CardSuit.Spades));

            //Act
            var result = _testGame.GameOver(_parentOfPiles, testDeck, testFlipped);

            //Assert
            //1. that GameOver() is false because testFlipped has one or more elements.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GameOver_PlayerCanWin_ValidCollections()
        {
            //Arrange
            var testDeck = new List<Card>();
            var testFlipped = new Stack<Card>();

            //Act
            var result = _testGame.GameOver(_parentOfPiles, testDeck, testFlipped);

            //Assert
            //1. that GameOver() is true if the Piles, Deck, and Flipped are empty.
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GameOver_PlayerCanGiveUp()
        {
            //Arrange
            var testFlipped = new Stack<Card>();

            var giveUpSelected = true;

            //Act
            var result = _testGame.GameOver(_parentOfPiles, _testGame.Deck.Cards, testFlipped, giveUpSelected);

            //Assert
            //1. that the result is true because giveUp is true;
            Assert.IsTrue(result);
            
        }

        [TestMethod]
        public void GameOver_PlayerCanGiveUp_OptionalVarDefaultsToFalse()
        {
            //Arrange
            var testFlipped = new Stack<Card>();

            //Act
            var result = _testGame.GameOver(_parentOfPiles, _testGame.Deck.Cards, testFlipped);

            //Assert
            //1. that the result is false, because if it were true, that means the optional giveUp var is set to true.
            Assert.IsFalse(result);
        }
        #endregion
    }
}
