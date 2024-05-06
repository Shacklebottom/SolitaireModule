using SolitaireDomain.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ModuleTesting
{
    [TestClass]
    public class PlayerTests
    {
        [TestMethod]
        public void Constructor_ShouldSetPlayerName()
        {
            //Arrange
            var name = "Player 1";

            //Act
            var player = new Player(name);

            //Assert
            Assert.AreEqual(name, player.Name);
        }

        [TestMethod]
        public void Property_PlayerScoreShouldExist()
        {
            //Arrange
            var score = 1;

            var player = new Player();

            //Act
            player.Score = score;

            //Assert
            Assert.AreEqual(score, player.Score);
        }
    }
}
