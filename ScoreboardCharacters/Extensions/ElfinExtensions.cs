using System;
using Bnfour.MuseDashMods.ScoreboardCharacters.Data;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Extensions
{
    public static class ElfinExtensions
    {
        private const Elfin LastImplemented = Elfin.Silencer;

        public static bool IsActuallyAnElfin(this Elfin elfin)
        {
            return elfin > Elfin.NoElfin;
        }

        // TODO scuffed naming
        public static bool IsMystery(this Elfin elfin)
        {
            return elfin > LastImplemented;
        }

        // elfin menu order matches id order
        // method is here to save me 5 minutes when it will not
        // (so never?)
        public static int GetMenuOrder(this Elfin elfin)
        {
            // return something for elfins when order is N/A or unknown
            //  just in case
            // TODO consider throwing and catching with a log warning?
            if (!elfin.IsActuallyAnElfin() || elfin.IsMystery())
            {
                return 0;
            }
            return (int)elfin;
        }
    }
}
