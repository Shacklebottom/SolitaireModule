
namespace SolitaireDomain
{
    public interface IHeap
    {
        List<Card> Cards { get; }

        bool ValidatePlay(IEnumerable<Card> targetCollection, Card card);
    }
}
