
namespace SolitaireDomain
{
    public class Player(string name = "") : IPlayer
    {
        public string Name { get; set; } = name;
    }
}
