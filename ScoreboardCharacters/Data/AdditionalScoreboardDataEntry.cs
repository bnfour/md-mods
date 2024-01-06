namespace Bnfour.MuseDashMods.ScoreboardCharacters.Data;

/// <summary>
/// Represents data for a single custom button.
/// </summary>
public class AdditionalScoreboardDataEntry
{
    public Character Character { get; private set; }
    public Elfin Elfin { get; private set; }

    public AdditionalScoreboardDataEntry(Api.PlayInfo detail)
    {
        Character = (Character)int.Parse(detail.CharacterId);
        Elfin = string.IsNullOrEmpty(detail.ElfinId) ? Elfin.NoElfin : (Elfin)int.Parse(detail.ElfinId);
    }
}
