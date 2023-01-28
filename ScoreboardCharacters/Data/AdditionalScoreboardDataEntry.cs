using System;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Data
{
    public class AdditionalScoreboardDataEntry
    {
        public string CharacterId { get; set; }
        public string ElfinId { get; set; }

        // just in case, probably not necessary
        public AdditionalScoreboardDataEntry() { }

        public AdditionalScoreboardDataEntry(Api.PlayInfo detail)
        {
            CharacterId = detail.CharacterId;
            ElfinId = detail.ElfinId;
        }

        public override string ToString()
        {
            // human-readable proved to be too long for a name label copy we have now
            return $"{CharacterId}/{ElfinId}";
        }

        // TODO check the id format, ints as strings are silly (but it's the same way in the api)
        // TODO move to extensions?
        private string CharacterIdToReadableForm()
        {
            switch (CharacterId)
            {
                case "0": return "Bassist Rin";
                case "1": return "Bad Girl Rin";
                case "2": return "Sleepwalker Rin";
                case "3": return "Bunny Girl Rin";
                case "4": return "Pilot Buro";
                case "5": return "Idol Buro";
                case "6": return "Zombie Buro";
                case "7": return "Joker Buro";
                case "8": return "Violinist Marija";
                case "9": return "Maid Marija";
                case "10": return "Mahou Shoujo Marija";
                case "11": return "Akuma Marija";
                case "12": return "Girl in Black Marija";
                case "13": return "Santa Rin";
                case "14": return "JK Buro";
                case "15": return "Yume";
                case "16": return "Neko";
                case "17": return "Worker Rin";
                case "18": return "Reimu";
                case "19": return "El Clear";
                case "20": return "Sister Marija";
                case "21": return "Marisa";
                case "22": return "Amiya";
                default: return "Unknown Girl";
            }
        }

        private string ElfinIdToReadableForm()
        {
            switch (ElfinId)
            {
                case "0": return "Mio Sir";
                case "1": return "Angela";
                case "2": return "Thanatos";
                case "3": return "Rabbot";
                case "4": return "Robot Nurse";
                case "5": return "Little Witch";
                case "6": return "Dragon Girl";
                case "7": return "Lilith";
                case "8": return "Rhythm Doctor Bird";
                case "9": return "Silencer";
                default: return "Unknown Elfin";
            }
        }

        // TODO just floating here, check and move somewhere relevant
        // used to scroll the menus on switch
        private int GetCharacterMenuOrder()
        {
            switch (CharacterId)
            {
                case "0": return 0;
                case "1": return 1;
                case "2": return 2;
                case "3": return 3;
                // Santa (13) and Worker (17) go here
                case "4": return 6;
                case "5": return 7;
                case "6": return 8;
                case "7": return 9;
                // JK (14) here
                case "8": return 11;
                case "9": return 12;
                case "10": return 13;
                case "11": return 14;
                case "12": return 15;
                // Sister (20) here
                case "13": return 4;
                case "14": return 10;
                case "15": return 17;
                case "16": return 18;
                case "17": return 5;
                case "18": return 19;
                case "19": return 20;
                case "20": return 16;
                case "21": return 21;
                case "22": return 22;
                default: return 0;
            }
        }
        // elfin menu order matches id order
    }
}
