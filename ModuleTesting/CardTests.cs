using static SolitaireDomain.EnumCardRank;
using static SolitaireDomain.EnumCardSuit;
using SolitaireDomain;

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