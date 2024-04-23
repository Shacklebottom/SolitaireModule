
namespace SolitaireDomain
{
    public class Pile : IHeap
    {
        public List<Card> Cards { get; set; } = [];

        public bool ValidatePlay(IEnumerable<Card> targetCollection, Card card)
        {
            return true;
        }
    }
}
