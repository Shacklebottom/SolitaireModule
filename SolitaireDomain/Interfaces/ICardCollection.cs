using SolitaireDomain.Objects;

namespace SolitaireDomain.Interfaces
{
    public interface ICardCollection
    {
        List<Card> Cards { get; set; }

        bool ValidatePlay(Card card);

        bool ValidateMove(IEnumerable<Card> sourceCollection, int startingIndex);

        void SetupCardCollection(List<Card> cards);
    }
}
