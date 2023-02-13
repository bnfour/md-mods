using System;
using Bnfour.MuseDashMods.ScoreboardCharacters.Data;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Extensions
{
    public static class ElfinExtensions
    {
        private const Elfin LastKnown = Elfin.Silencer;

        public static bool IsActuallyAnElfin(this Elfin elfin)
        {
            return elfin > Elfin.NoElfin;
        }

        public static bool IsPlaceholderForFuture(this Elfin elfin)
        {
            return elfin > LastKnown;
        }

        // elfin menu order matches id order
        // method is here to save me 5 minutes when it will not
        // (so never?)
        public static int GetMenuOrder(this Elfin elfin)
        {
            if (!elfin.IsActuallyAnElfin() || elfin.IsPlaceholderForFuture())
            {
                throw new NotImplementedException("Menu order not known");
            }
            return (int)elfin;
        }
    }
}
