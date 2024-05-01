using SolitaireDomain.Objects;

namespace SolitaireDomain.Interfaces
{
    public interface IDeckUnwrapper
    {
        List<Card> Cards { get; }

        Stack<Card> Flipped { get; }

        void Shuffle();

        List<Card> Draw(int count);

        void Flip(int count);
    }
}
