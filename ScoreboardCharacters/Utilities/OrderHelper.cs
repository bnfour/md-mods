using System;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Utilities
{
    // TODO think how to work with character/elfin ids internally
    // currently everything is all over the place, here it's ints as in DataHelper,
    // in other places it's strings pulled straight from API response

    public static class OrderHelper
    {
        public static int GetCharacterMenuOrder(int characterId)
        {
            switch (characterId)
            {
                case 0: return 0;
                case 1: return 1;
                case 2: return 2;
                case 3: return 3;
                // Santa (13) and Worker (17) go here
                case 4: return 6;
                case 5: return 7;
                case 6: return 8;
                case 7: return 9;
                // JK (14) here
                case 8: return 11;
                case 9: return 12;
                case 10: return 13;
                case 11: return 14;
                case 12: return 15;
                // Sister (20) here
                case 13: return 4;
                case 14: return 10;
                case 15: return 17;
                case 16: return 18;
                case 17: return 5;
                case 18: return 19;
                case 19: return 20;
                case 20: return 16;
                case 21: return 21;
                case 22: return 22;
                default: return 0;
            }
        }

        // elfin menu order matches id order
        // method is here to save me 5 minutes when it will not
        // (so never?)
        public static int GetElfinMenuOrder(int elfinId)
        {
            // id -1 of no elfin is handled before this is even called,
            // but just in case, return an existing value here as well
            return Math.Max(elfinId, 0);
        }
    }
}
