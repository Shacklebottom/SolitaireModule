
using static SolitaireDomain.EnumCardRank;
using static SolitaireDomain.EnumCardSuit;
using SolitaireDomain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ModuleTesting
{
    [TestClass]
    public class GameTests
    {
        //Global Test Variables
        private Game _testGame;
        private Mock<IPlayer> _mockPlayer;
        private Mock<IDeckUnwrapper> _mockDeckUnwrapper;
        private Mock<ICardCollection>[] _mockFoundations;
        private Mock<ICardCollection>[] _mockPiles;

        [TestInitialize]
        public void GameTestsInitialize()
        {
            //player setup
            _mockPlayer = new Mock<IPlayer>();

            _mockPlayer.Setup(p => p.Name).Returns("Player 1");

            //deck setup
            _mockDeckUnwrapper = new Mock<IDeckUnwrapper>();

            List<Card> cards = [];
            foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
            {
                foreach (CardRank rank in Enum.GetValues(typeof(CardRank)))
                {
                    cards.Add(new Card(rank, suit));
                }
            }

            _mockDeckUnwrapper.Setup(d => d.Cards).Returns(new List<Card>(cards));

            _mockDeckUnwrapper.Setup(d => d.Draw(It.IsAny<int>())).Returns((int count) =>
            {
                var cardsDrawn = cards.Take(count).ToList();
                cards = cards.Skip(count).ToList();
                return cardsDrawn;
            });

            //foundations setup
            _mockFoundations = new Mock<ICardCollection>[4];

            for (var i = 0; i < _mockFoundations.Length; i++)
            {
                _mockFoundations[i] = new Mock<ICardCollection>();
                _mockFoundations[i].Setup(f => f.Cards).Returns(new List<Card>());
            }

            //piles setup
            _mockPiles = new Mock<ICardCollection>[7];

            for (var i = 0; i < _mockPiles.Length; i++)
            {
                _mockPiles[i] = new Mock<ICardCollection>();
                _mockPiles[i].Setup(p => p.Cards).Returns(new List<Card>());
            }

            //game setup
            _testGame = new Game(_mockDeckUnwrapper.Object,
                _mockFoundations.Select(f => f.Object).ToArray(),
                _mockPiles.Select(p => p.Object).ToArray(),
                _mockPlayer.Object);



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
            var name = _testGame.Player.Name;

            Assert.AreEqual("Player 1", name);
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
        public void Deck_WasShuffled_Once()
        {
            //Arrange
            _mockDeckUnwrapper.Setup(d => d.Shuffle());

            //Act
            //1. In the Game() Constructor, Shuffle() is called, which shuffles the deck.

            //Assert
            //1. that deck.Shuffle() was called exactly once by the Game() constructor.
            _mockDeckUnwrapper.Verify(d => d.Shuffle(), Times.Once);
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
            Assert.IsTrue(_testGame.Piles.Select((pile, index) => new { pile, index }).All(obj => obj.pile.Cards.Count == obj.index + 1),
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
            Assert.IsTrue(_testGame.Piles.All(p => p.Cards.Count(c => c.FaceUp) == 1));
        }

        [TestMethod]
        public void Piles_TheLastCardIsTheFaceUpCard_Operation()
        {
            //Arrange
            //1. using _testGame

            //Act
            //1. In the Game() Constructor, SetupPiles() is called, which turns the last card in each Pile FaceUp.

            //Assert
            Assert.IsTrue(_testGame.Piles.All(p => p.Cards.Last().FaceUp == true));

        }
        #endregion

        //#region FlipPileCard()
        //[TestMethod]
        //public void FlipPileCard_CanFlip()
        //{
        //    //Arrange
        //    //1. using _testGame
        //    var pileIndex = 0;
        //    _testGame.Piles[pileIndex].Cards.Last().FaceUp = false;

        //    //Act
        //    _testGame.FlipPileCard(pileIndex);

        //    //Assert
        //    Assert.AreEqual(true, _testGame.Piles[pileIndex].Cards.Last().FaceUp);
        //}
        //#endregion

        //#region FlipFromDeck()
        //[TestMethod]
        //public void FlipFromDeck_CanFlip()
        //{
        //    //Arrange
        //    //1. using _testGame
        //    var drawCount = 3;
        //    //store how many cards are in flipped and then flip more and compare the two values.

        //    //Act
        //    _testGame.FlipFromDeck(drawCount);

        //    //Assert
        //    //1. The correct number of cards are present.
        //    Assert.AreEqual(drawCount, _testGame.FlippedCards.Count);
        //    //2. and, each card revealed is FaceUp.
        //    Assert.IsTrue(_testGame.FlippedCards.ToList().TrueForAll(c => c.FaceUp == true));
        //}
        //#endregion


        //#region PlayFromFlipped()
        //[TestMethod]
        //public void PlayFromFlipped_WillValidatePlay_InvalidPlay()
        //{
        //    //Arrange
        //    var testFlipped = new Stack<Card>();
        //    testFlipped.Push(new Card(CardRank.Five, CardSuit.Clubs) { FaceUp = true });
        //    testFlipped.Push(new Card(CardRank.Jack, CardSuit.Hearts) { FaceUp = true });
        //    testFlipped.Push(new Card(CardRank.Ace, CardSuit.Diamonds) { FaceUp = true });

        //    var testPile = new List<Card>();

        //    //Act
        //    _testGame.PlayFromFlipped(testPile, testFlipped, _parentOfPiles);

        //    //Assert
        //    //1. that this test will be false because ValidatePlay says the play is invalid.
        //    Assert.IsFalse(testPile.Count > 0);
        //}

        //[TestMethod]
        //public void PlayFromFlipped_CanPlay()
        //{
        //    //Arrange
        //    var testFlipped = new Stack<Card>();
        //    testFlipped.Push(new Card(CardRank.Five, CardSuit.Clubs) { FaceUp = true });
        //    testFlipped.Push(new Card(CardRank.Jack, CardSuit.Hearts) { FaceUp = true });
        //    testFlipped.Push(new Card(CardRank.Ace, CardSuit.Diamonds) { FaceUp = true });

        //    //Act
        //    _testGame.PlayFromFlipped(_testPile, testFlipped, _parentOfPiles);

        //    //Assert
        //    //1. The card was added to the collection.
        //    Assert.IsTrue(_testPile.Last().Rank == CardRank.Ace, "The card was not added to the collection");
        //    //2. The card was removed from the stack.
        //    Assert.IsTrue(testFlipped.Peek().Rank != CardRank.Ace, "The card was not Pop()'d off the Stack");
        //}
        //#endregion

        //#region MovePile()
        //[TestMethod]
        //public void MovePile_WillValidateMove_InvalidMove()
        //{
        //    //Arrange
        //    var testTargetPile = new List<Card>();

        //    var startingIndex = 2;

        //    //Act
        //    _testGame.MovePile(testTargetPile, _testPile, startingIndex, _parentOfPiles);

        //    //Assert
        //    //1. that this test will be false because ValidatePlay() says the move is invalid.
        //    Assert.IsFalse(testTargetPile.Count > 0);
        //}

        //[TestMethod]
        //public void MovePile_CanMove_ToAPile_BasedOnStartingIndex()
        //{
        //    //NOTE!: this test could be better constructed to look at the actual contents of each list to ensure that the rules were properly addressed.

        //    //Arrange
        //    var testTargetPile = new List<Card>();

        //    var startingIndex = 2;

        //    var testPile = new List<Card>()
        //    {
        //        new(CardRank.Four, CardSuit.Hearts),
        //        new(CardRank.Six, CardSuit.Diamonds),
        //        new(CardRank.King, CardSuit.Spades) { FaceUp = true },
        //        new(CardRank.Queen, CardSuit.Hearts) { FaceUp = true },
        //        new(CardRank.Jack, CardSuit.Clubs) { FaceUp = true },
        //    };

        //    //Act
        //    _testGame.MovePile(testTargetPile, testPile, startingIndex, _parentOfPiles);

        //    //Assert
        //    //1. That the cards were added to the collection, and
        //    Assert.IsTrue(testTargetPile.First().Rank == CardRank.King, "the collection was not properly added to the target collection");
        //    Assert.IsTrue(testTargetPile.Count == 3, "the FaceUp == false cards were added to the collection");
        //    //2. that the cards were removed from their source pile.
        //    Assert.IsFalse(testTargetPile.Any(testPile.Contains), "the collection was not removed from its source");
        //}

        //[TestMethod]
        //public void MovePile_CanMove_ToAFoundation_BasedOnEndingIndex()
        //{
        //    //NOTE!: this test could be better constructed to look at the actual contents of each list to ensure that the rules were properly addressed.

        //    //Arrange
        //    var testTargetFoundation = new List<Card>();

        //    var endingIndex = 2;

        //    var testPile = new List<Card>()
        //    {
        //        new(CardRank.Four, CardSuit.Hearts),
        //        new(CardRank.Six, CardSuit.Diamonds),
        //        new(CardRank.Three, CardSuit.Spades) { FaceUp = true },
        //        new(CardRank.Two, CardSuit.Hearts) { FaceUp = true },
        //        new(CardRank.Ace, CardSuit.Clubs) { FaceUp = true },
        //    };

        //    //Act
        //    _testGame.MovePile(testTargetFoundation, testPile, endingIndex, _parentOfFoundations);

        //    //Assert
        //    //1. That the cards were added to the collection, and
        //    Assert.IsTrue(testTargetFoundation.First().Rank == CardRank.Ace, "the collection was not properly added to the target collection");
        //    Assert.IsTrue(testTargetFoundation.Count == 3, "the FaceUp == false cards were added to the collection");
        //    //2. that the cards were removed from their source pile.
        //    Assert.IsFalse(testTargetFoundation.Any(testPile.Contains), "the collection was not removed from its source");
        //}

        //[TestMethod]
        //[ExpectedException(typeof(ArgumentException))]
        //public void MovePile_ThrowsAnException_IfTheSelectedCardsContainAFaceDownCard()
        //{
        //    //Arrange
        //    var testTargetPile = new List<Card>();

        //    var startingIndex = 0;

        //    var testPile = new List<Card>()
        //    {
        //        new(CardRank.Four, CardSuit.Hearts),
        //        new(CardRank.Six, CardSuit.Diamonds),
        //        new(CardRank.King, CardSuit.Spades) { FaceUp = true },
        //        new(CardRank.Queen, CardSuit.Hearts) { FaceUp = true },
        //        new(CardRank.Jack, CardSuit.Clubs) { FaceUp = true },
        //    };

        //    //Act
        //    _testGame.MovePile(testTargetPile, testPile, startingIndex, _parentOfPiles);

        //    //Assert
        //    //1. that MovePile() will throw an exception if the startingIndex or endingIndex selects a FaceUp == false card as part of its selection.
        //}
        //#endregion

        //#region GameOver()
        //[TestMethod]
        //public void GameOver_PlayerCanWin_InvalidPiles()
        //{
        //    //Arrange
        //    var testDeck = new List<Card>();

        //    var testFlipped = new Stack<Card>();

        //    List<Card>[] testPiles = [[], [], [], [], [], [], []];
        //    testPiles[0].Add(new Card(CardRank.Ace, CardSuit.Hearts));

        //    //Act
        //    var result = _testGame.GameOver(testPiles, testDeck, testFlipped);

        //    //Assert
        //    //1. that GameOver() is false because one of the Piles has one or more elements.
        //    Assert.IsFalse(result);
        //}

        //[TestMethod]
        //public void GameOver_PlayerCanWin_InvalidDeck()
        //{
        //    //Arrange
        //    var testDeck = new List<Card>()
        //    {
        //        new Card(CardRank.Eight, CardSuit.Diamonds)
        //    };
        //    var testFlipped = new Stack<Card>();

        //    //Act
        //    var result = _testGame.GameOver(_parentOfPiles, testDeck, testFlipped);

        //    //Assert
        //    //1. that GameOver() is false because testDeck has one or more elements.
        //    Assert.IsFalse(result);
        //}

        //[TestMethod]
        //public void GameOver_PlayerCanWin_InvalidFlipped()
        //{
        //    //Arrange
        //    var testDeck = new List<Card>();

        //    var testFlipped = new Stack<Card>();
        //    testFlipped.Push(new Card(CardRank.Seven, CardSuit.Spades));

        //    //Act
        //    var result = _testGame.GameOver(_parentOfPiles, testDeck, testFlipped);

        //    //Assert
        //    //1. that GameOver() is false because testFlipped has one or more elements.
        //    Assert.IsFalse(result);
        //}

        //[TestMethod]
        //public void GameOver_PlayerCanWin_ValidCollections()
        //{
        //    //Arrange
        //    var testDeck = new List<Card>();
        //    var testFlipped = new Stack<Card>();

        //    //Act
        //    var result = _testGame.GameOver(_parentOfPiles, testDeck, testFlipped);

        //    //Assert
        //    //1. that GameOver() is true if the Piles, Deck, and Flipped are empty.
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void GameOver_PlayerCanGiveUp()
        //{
        //    //Arrange
        //    var testFlipped = new Stack<Card>();

        //    var giveUpSelected = true;

        //    //Act
        //    var result = _testGame.GameOver(_parentOfPiles, _testGame.Deck.Cards, testFlipped, giveUpSelected);

        //    //Assert
        //    //1. that the result is true because giveUp is true;
        //    Assert.IsTrue(result);

        //}

        //[TestMethod]
        //public void GameOver_PlayerCanGiveUp_OptionalVarDefaultsToFalse()
        //{
        //    //Arrange
        //    var testFlipped = new Stack<Card>();

        //    //Act
        //    var result = _testGame.GameOver(_parentOfPiles, _testGame.Deck.Cards, testFlipped);

        //    //Assert
        //    //1. that the result is false, because if it were true, that means the optional giveUp var is set to true.
        //    Assert.IsFalse(result);
        //}
        //#endregion
    }
}
