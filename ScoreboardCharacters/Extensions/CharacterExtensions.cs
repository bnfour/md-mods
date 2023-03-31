using System;
using System.Collections.Generic;
using Bnfour.MuseDashMods.ScoreboardCharacters.Data;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Extensions
{
    public static class CharacterExtensions
    {
        private const Character LastKnown = Character.OlaBoxer;

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
            Character.OlaBoxer,
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

        public static bool IsPlaceholderForFuture(this Character character)
        {
            return character > LastKnown;
        }

        public static int GetMenuOrder(this Character character)
        {
            if (character.IsPlaceholderForFuture())
            {
                throw new NotImplementedException("Menu order not known");
            }
            return KnownOrder.IndexOf(character);
        }
    }
}
