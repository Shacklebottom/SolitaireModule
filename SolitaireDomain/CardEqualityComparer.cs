
namespace SolitaireDomain
{
    public class CardEqualityComparer : IEqualityComparer<Card>
    {
        public bool Equals(Card? x, Card? y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x is null || y is null)
            {
                return false;
            }
            return x.Rank == y.Rank
                && x.Suit == y.Suit
                && x.FaceUp == y.FaceUp;
        }
        //we're not implementing a HashCode, whatever that is :P
        public int GetHashCode(Card? obj) { return obj != null ? obj.GetHashCode() : 0; }
    }
}
