
namespace SolitaireDomain
{
    public class Foundation : IHeap
    {
        public List<Card> Cards { get; set; } = [];

        public bool ValidatePlay()
        {
            return true;
        }
    }
}
