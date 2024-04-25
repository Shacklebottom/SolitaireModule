
namespace SolitaireDomain
{
    public interface ICardCollection
    {
        List<Card> Cards { get; }

        bool ValidatePlay(IEnumerable<Card> targetCollection, Card card);
    }
}
