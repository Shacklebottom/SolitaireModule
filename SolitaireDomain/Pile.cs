
namespace SolitaireDomain
{
    public class Pile : IHeap
    {
        public List<Card> Cards { get; set; } = [];

        public bool ValidatePlay()
        {
            return true;
        }
    }
}
