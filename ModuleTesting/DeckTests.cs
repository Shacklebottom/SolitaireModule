using static SolitaireDomain.EnumCardRank;
using static SolitaireDomain.EnumCardSuit;
using SolitaireDomain;

namespace ModuleTesting
{
    [TestClass]
    public class DeckTests
    {
        //Global Test Variables
        private Deck _deck = new();

        [TestInitialize]
        public void DeckTestsInitialize()
        {
            _deck = new Deck();
        }

        [TestMethod]
        public void CardsNumberToFiftyTwo()
        {
            //Arrange
            //1. Global Initalizer

            //Act

            //Assert
            Assert.AreEqual(52, _deck.Cards.Count);
        }

        [TestMethod]
        public void CardsAreAllUnique()
        {
            //Arrange
            //1. Global Initalizer

            //Act
            var uniqueCardCount = _deck.Cards.GroupBy(c => new { c.Rank, c.Suit }).Count();

            //Assert
            Assert.AreEqual(52, uniqueCardCount);
        }

        [TestMethod]
        public void DeckWasShuffled()
        {
            //Arrange
            //1. Global Initalizer

            //Act
            _deck.Shuffle();

            //Assert
            var firstFiveCards = _deck.Cards.Take(5);
            var areAllHearts = firstFiveCards.All(c => c.Suit == CardSuit.Hearts);
            var sumFirstFive = firstFiveCards.Sum(c => (int)c.Rank);

            //1. If the first five cards are NOT Hearts _and_ the first five cards DON'T sum to 15. If both are true, the deck hasn't been shuffled.
            Assert.IsFalse(areAllHearts && sumFirstFive == 15, "First five cards were Ace - Five of Hearts");
        }

        [TestMethod]
        public void DeckCanDraw()
        {
            //Arrange
            //1. Global Initalizer
            var cardsToDraw = 3;
            var firstThreeCards = _deck.Cards.Take(cardsToDraw).ToList();

            //Act
            var cards = _deck.Draw(cardsToDraw);

            //Assert
            //1. the drawn cards were the first three cards.
            Assert.IsTrue(firstThreeCards.SequenceEqual(cards), $"The first {cardsToDraw} cards were not the cards drawn");
            //2. the first three cards are no longer in the deck.
            Assert.IsFalse(cards.Any(_deck.Cards.Contains), $"The first {cardsToDraw} cards weren't removed from the deck");
        }
    }
}
