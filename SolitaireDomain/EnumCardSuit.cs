
using System.Drawing;

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

        public static Color GetSuitColor(this CardSuit suit)
        {
            return suit switch
            {
                CardSuit.Hearts or CardSuit.Diamonds => Color.Red,
                CardSuit.Spades or CardSuit.Clubs => Color.Black,
                _ => throw new Exception("oh no..."),
            };
        }
    }
}
