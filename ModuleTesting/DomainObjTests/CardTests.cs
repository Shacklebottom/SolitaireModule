using static SolitaireDomain.CardEnum;
using SolitaireDomain;

namespace ModuleTesting.DomainObjTests
{
    [TestClass]
    public class CardTests
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