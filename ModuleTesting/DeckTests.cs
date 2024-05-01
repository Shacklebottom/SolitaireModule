using static SolitaireDomain.Enums.EnumCardRank;
using static SolitaireDomain.Enums.EnumCardSuit;
using SolitaireDomain.Objects;
using SolitaireDomain.Comparers;

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
        public void Cards_NumberToFiftyTwo()
        {
            //Arrange

            //Act

            //Assert
            Assert.AreEqual(52, _deck.Cards.Count, "the collection doesn't contain 52 cards");
        }

        [TestMethod]
        public void Cards_AreAllUnique()
        {
            //Arrange

            //Act

            //Assert
            var uniqueCardCount = _deck.Cards.GroupBy(c => new { c.Rank, c.Suit }).Count();
            Assert.AreEqual(52, uniqueCardCount, "the collection doesn't contain 52 unique cards");
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
            //1. the drawn cards were the first three cards.
            Assert.IsTrue(firstThreeCards.SequenceEqual(cards, new CardEqualityComparer()), $"The first {cardsToDraw} cards were not the cards drawn");
            //2. the first three cards are no longer in the deck.
            Assert.IsFalse(cards.Any(_deck.Cards.Contains), $"The first {cardsToDraw} cards weren't removed from the deck");
        }


    }
}
