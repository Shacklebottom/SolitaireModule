using static SolitaireDomain.EnumCardColor;

namespace SolitaireDomain
{
    public static class EnumCardSuit
    {
        public enum CardSuit
        {
            Hearts,
            Diamonds,
            Spades,
            Clubs
        }

        public static CardColor GetSuitColor(this CardSuit suit)
        {
            return suit switch
            {
                CardSuit.Hearts or CardSuit.Diamonds => CardColor.Red,
                CardSuit.Spades or CardSuit.Clubs => CardColor.Black,
                _ => throw new Exception("oh no..."),
            };
        }
    }
}
