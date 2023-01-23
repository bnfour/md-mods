using System;

namespace Bnfour.MuseDashMods.ScoreboardCharacters.Data
{
    public class AdditionalScoreboardDataEntry
    {
        public string CharacterId { get; set; }
        public string ElfinId { get; set; }

        public override string ToString()
        {
            // human-readable proved to be too long for a name label copy we have now
            return $"{CharacterId}/{ElfinId}";
        }

        // TODO check the id format, ints as strings are silly
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
                case "8": return "Violin Marija";
                case "9": return "Maid Maria";
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
    }
}
