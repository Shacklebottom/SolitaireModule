using static SolitaireDomain.Enums.EnumCardRank;
using static SolitaireDomain.Enums.EnumCardSuit;
using static SolitaireDomain.Enums.EnumCardColor;
using SolitaireDomain.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ModuleTesting
{
    [TestClass]
    public class CardTests
    {
        [TestMethod]
        public void Constructor_ShouldSetSuitAndRank()
        {
            //Arrange
            var rank = CardRank.Queen;
            var suit = CardSuit.Hearts;

            //Act
            var card = new Card(rank, suit);

            //Assert
            //1. that Card.Rank is rank, and
            Assert.AreEqual(rank, card.Rank);
            //2. that Card.Suit is suit.
            Assert.AreEqual(suit, card.Suit);
        }

        [TestMethod]
        public void CardColor_ShouldBeBlack_WhenSuitIsSpades()
        {
            //Arrange
            var card = new Card(CardRank.Ace, CardSuit.Spades);

            //Act
            var color = card.Color;

            //Assert
            Assert.AreEqual(CardColor.Black, color);
        }

        [TestMethod]
        public void CardColor_ShouldBeBlack_WhenSuitIsClubs()
        {
            //Arrange
            var card = new Card(CardRank.Ace, CardSuit.Clubs);

            //Act
            var color = card.Color;

            //Assert
            Assert.AreEqual(CardColor.Black, color);
        }

        [TestMethod]
        public void CardColor_ShouldBeRed_WhenSuitIsDiamonds()
        {
            //Arrange
            var card = new Card(CardRank.Ace, CardSuit.Diamonds);

            //Act
            var color = card.Color;

            //Assert
            Assert.AreEqual(CardColor.Red, color);
        }

        [TestMethod]
        public void CardColor_ShouldBeRed_WhenSuitIsHearts()
        {
            //Arrange
            var card = new Card(CardRank.Ace, CardSuit.Hearts);
            
            //Act
            var color = card.Color;

            //Assert
            Assert.AreEqual(CardColor.Red, color);
        }

        [TestMethod]
        public void Card_CanToString()
        {
            //Arrange
            var testCard = new Card(CardRank.Ace, CardSuit.Diamonds);

            //Act
            var cardString = testCard.ToString();

            //Assert
            Assert.AreEqual("Ace of Diamonds", cardString);
        }
    }
}