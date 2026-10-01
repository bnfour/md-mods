namespace Bnfour.MuseDashMods.ScoreboardCharacters.Data;

/// <summary>
/// Represents data for a single custom button.
/// </summary>
public record AdditionalScoreboardDataEntry
{
    public Character Character { get; init; }
    public Elfin Elfin { get; init; }

    public AdditionalScoreboardDataEntry(Api.PlayInfo detail)
    {
        var character = (Character)int.Parse(detail.CharacterId);
        // Phantom Thief Rin is transparently replaced with the actual character
        // if the data to do so if available
        if (character == Character.RinPhantomThief && !string.IsNullOrEmpty(detail.KaitouActualId))
        {
            Character = (Character)int.Parse(detail.KaitouActualId);
        }
        else
        {
            Character = character;
        }

        Elfin = string.IsNullOrEmpty(detail.ElfinId) ? Elfin.NoElfin : (Elfin)int.Parse(detail.ElfinId);
    }
}
