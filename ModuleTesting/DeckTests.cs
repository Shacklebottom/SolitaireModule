using SolitaireDomain;
using static SolitaireDomain.CardEnum;

namespace ModuleTesting
{
    [TestClass]
    public class DeckTests
    {
        [TestMethod]
        public void NumberOfCardsInDeck()
        {
            //Arrange
            var deck = new Deck();

            //Act

            //Assert
            Assert.AreEqual(52, deck.Cards.Count);
        }

        [TestMethod]
        public void AreAllCardsUnique()
        {
            //Arrange
            var deck = new Deck();

            //Act
            int uniqueCardCount = deck.Cards.GroupBy(c => new { c.Rank, c.Suit }).Count();

            //Assert
            Assert.AreEqual(52, uniqueCardCount);
        }

        [TestMethod]
        public void WasDeckShuffled()
        {
            //Arrange
            var deck = new Deck();

            //Act
            deck.Shuffle();

            //Assert
            var firstFiveCards = deck.Cards.Take(5);
            var areAllHearts = firstFiveCards.All(c => c.Suit == Suit.Hearts);
            var sumFirstFive = firstFiveCards.Sum(c => (int)c.Rank);
            Assert.IsFalse(areAllHearts && sumFirstFive == 15, "First five cards were Ace - Five of Hearts");
        }

        [TestMethod]
        public void CanDeckDraw()
        {
            //Arrange
            var deck = new Deck();
            List<Card> firstThreeCards = deck.Cards.Take(3).ToList();

            //Act
            List<Card> cards = deck.Draw(3);

            //Assert
            //1. the drawn cards were the first three cards.
            Assert.IsTrue(firstThreeCards.SequenceEqual(cards));
            //2. the first three cards are no longer in the deck.
            Assert.IsFalse(cards.Any(deck.Cards.Contains));
        }
    }
}
