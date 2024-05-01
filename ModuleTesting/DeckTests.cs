using static SolitaireDomain.Enums.EnumCardRank;
using static SolitaireDomain.Enums.EnumCardSuit;
using SolitaireDomain.Objects;
using SolitaireDomain.Comparers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ModuleTesting
{
    [TestClass]
    public class DeckTests
    {
        //Global Test Variables
        private StandardDeck _deck;

        [TestInitialize]
        public void Initialize_DeckTest()
        {
            _deck = new StandardDeck();
        }

        [TestMethod]
        public void Cards_NumberFiftyTwo()
        {
            //Arrange
            var expectedCount = 52;

            //Act

            //Assert
            Assert.AreEqual(expectedCount, _deck.Cards.Count, "the collection doesn't contain 52 cards");
        }

        [TestMethod]
        public void Cards_AreAllUnique()
        {
            //Arrange
            var expectedCount = 52;

            //Act

            //Assert
            var uniqueCardCount = _deck.Cards.GroupBy(c => new { c.Rank, c.Suit }).Count();
            Assert.AreEqual(expectedCount, uniqueCardCount, "the collection doesn't contain 52 unique cards");
        }

        [TestMethod]
        public void Deck_CanShuffle()
        {
            //Arrange

            //Act
            _deck.Shuffle();

            //Assert
            var firstFiveCards = _deck.Cards.Take(5);
            var areAllHearts = firstFiveCards.All(c => c.Suit == CardSuit.Hearts);
            var sumFirstFive = firstFiveCards.Sum(c => (int)c.Rank);

            //1. that the first five cards are NOT Hearts _and_ the first five cards DON'T sum to 15. If both are true, the deck hasn't been shuffled.
            Assert.IsFalse(areAllHearts && sumFirstFive == 15, "First five cards were Ace - Five of Hearts");
        }

        [TestMethod]
        public void Deck_CanDraw()
        {
            //Arrange
            var cardsToDraw = 3;
            var firstThreeCards = _deck.Cards.Take(cardsToDraw).ToList();

            //Act
            var cards = _deck.Draw(cardsToDraw);

            //Assert
            //1. the drawn cards were the first three cards, and
            Assert.IsTrue(firstThreeCards.SequenceEqual(cards, new CardEqualityComparer()), $"the first {cardsToDraw} cards were not the cards drawn");
            //2. the first three cards are no longer in the deck.
            Assert.IsFalse(cards.Any(_deck.Cards.Contains), $"the first {cardsToDraw} cards weren't removed from the deck");
        }

        [TestMethod]
        public void Deck_CanFlip()
        {
            //Arrange
            var cardsToFlip = 3;
            var firstThreeCards = _deck.Cards.Take(cardsToFlip).ToList();

            //Act
            _deck.Flip(cardsToFlip);

            //Assert
            //1. the flipped cards were the first three cards, and
            Assert.IsTrue(firstThreeCards.SequenceEqual(_deck.Flipped.Reverse(), new CardEqualityComparer()), $"the first {cardsToFlip} cards were not the flipped cards");
            //2. the first three cards are no longer in the deck.
            Assert.IsFalse(_deck.Flipped.Any(_deck.Cards.Contains), $"the first {cardsToFlip} cards weren't removed from the deck");
        }

        [TestMethod]
        public void Flipped_ShouldFlipItselfOntoTheDeck_WhenTheDeckIsEmpty()
        {
            //Arrange
            var requestedCount = 3;

            var requestedCards = _deck.Cards.Take(requestedCount).ToList();

            _deck.Cards.ForEach(_deck.Flipped.Push);

            _deck.Cards.Clear();

            //Act
            _deck.Flip(requestedCount);

            //Assert
            //1. that the flipped cards were the first three cards, and
            Assert.IsTrue(requestedCards.SequenceEqual(_deck.Flipped.Reverse(), new CardEqualityComparer()), $"the first {requestedCount} cards were not the flipped cards");
            //2. that the flipped cards only contain three cards.
            Assert.AreEqual(requestedCount, _deck.Flipped.Count, "the flipped cards weren't flipped back onto the deck");
        }
    }
}
