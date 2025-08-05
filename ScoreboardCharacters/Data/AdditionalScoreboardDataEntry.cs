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
        Character = (Character)int.Parse(detail.CharacterId);
        Elfin = string.IsNullOrEmpty(detail.ElfinId) ? Elfin.NoElfin : (Elfin)int.Parse(detail.ElfinId);
    }
}
