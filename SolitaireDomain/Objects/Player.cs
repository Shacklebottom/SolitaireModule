using SolitaireDomain.Interfaces;

namespace SolitaireDomain.Objects
{
    public class Player : IPlayer
    {
        public string Name { get; set; }

        public Player(string name = "")
        {
            Name = name;
        }
    }
}