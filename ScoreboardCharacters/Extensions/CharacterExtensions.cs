using System;
using System.Collections.Generic;
using Bnfour.MuseDashMods.ScoreboardCharacters.Data;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Extensions
{
    public static class CharacterExtensions
    {
        private const Character LastImplemented = Character.Amiya;

        private static readonly List<Character> KnownOrder = new List<Character>
        {
            Character.RinDefault,
            Character.RinBadGirl,
            Character.RinSleepwalker,
            Character.RinBunny,
            Character.RinSanta,
            Character.RinWorker,
            Character.BuroDefault,
            Character.BuroIdol,
            Character.BuroZombie,
            Character.BuroJoker,
            Character.BuroJK,
            Character.MarijaDefault,
            Character.MarijaMaid,
            Character.MarijaMagic,
            Character.MarijaEvil,
            Character.MarijaInBlack,
            Character.MarijaSister,
            Character.Yume,
            Character.Neko,
            Character.Reimu,
            Character.ElClear,
            Character.Marisa,
            Character.Amiya
        };

        // TODO scuffed naming
        public static bool IsMystery(this Character character)
        {
            return character > LastImplemented;
        }

        public static int GetMenuOrder(this Character character)
        {
            // TODO consider throwing and catching with a log warning?
            if (character.IsMystery())
            {
                return 0;
            }
            return KnownOrder.IndexOf(character);
        }
    }
}
