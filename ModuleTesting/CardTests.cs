using static SolitaireDomain.Enums.EnumCardRank;
using static SolitaireDomain.Enums.EnumCardSuit;
using SolitaireDomain.Objects;

namespace ModuleTesting
{
    [TestClass]
    public class CardTests
    {
        [TestMethod]

        public void CardToString()
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