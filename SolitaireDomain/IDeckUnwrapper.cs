
namespace SolitaireDomain
{
    public interface IDeckUnwrapper
    {
        List<Card> Cards { get; }

        void UnwrapDeck();

        void Shuffle();

        List<Card> Draw(int count);
    }
}
