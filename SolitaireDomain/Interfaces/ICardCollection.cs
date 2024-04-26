using SolitaireDomain.Objects;

namespace SolitaireDomain.Interfaces
{
    public interface ICardCollection
    {
        List<Card> Cards { get; }

        bool ValidatePlay(Card card);

        void SetupCardCollection(List<Card> cards);
    }
}
