using System.Collections.Generic;
using System.Linq;

using Il2CppAssets.Scripts.Database;
using Il2CppPeroPeroGames.GlobalDefines;

namespace Bnfour.MuseDashMods.UITweaks.Utilities;

/// <summary>
/// Checks the current level uid against known list of songs
/// that require special treatment due to their "special" UI.
/// </summary>
internal static class MusicUidChecker
{
    private static readonly IReadOnlyList<string> _memeLevels =
    [
        // april fools 2024
        MusicUidDefine.peropero_aniki_ranbu,
        // april fools 2025
        MusicUidDefine.museyao
    ];

    public static bool IsMemeLevel()
        => _memeLevels.Contains(GlobalDataBase.dbBattleStage.musicUid);
}
