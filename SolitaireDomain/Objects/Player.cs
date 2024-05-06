using SolitaireDomain.Interfaces;

namespace SolitaireDomain.Objects
{
    public class Player : IPlayer
    {
        public string Name { get; set; }

        public int Score { get; set; } = 0;

        public Player(string name = "")
        {
            Name = name;
        }
    }
}