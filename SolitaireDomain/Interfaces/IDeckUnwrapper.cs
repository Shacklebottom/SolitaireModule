using SolitaireDomain.Objects;

namespace SolitaireDomain.Interfaces
{
    public interface IDeckUnwrapper
    {
        List<Card> Cards { get; }

        void Shuffle();

        List<Card> Draw(int count);
    }
}
