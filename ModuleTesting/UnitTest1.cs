using static SolitaireDomain.CardEnum;
using SolitaireDomain;

namespace ModuleTesting
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]

        public void CardToString()
        {
            //Arrange
            var mockCard = new Card(Rank.Ace, Suit.Diamonds);

            //Act
            var cardString = mockCard.ToString();

            //Assert
            Assert.AreEqual("Ace of Diamonds", cardString);
        }
    }
}