using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using MelonLoader;
using Newtonsoft.Json;
using UnityEngine;

using Il2Cpp;
using Il2CppAssets.Scripts.Database;

using Bnfour.MuseDashMods.SongInfo.Data;
using Bnfour.MuseDashMods.SongInfo.Utilities;

namespace Bnfour.MuseDashMods.SongInfo;

public class SongInfoMod : MelonMod
{
    public readonly SongDurationProvider DurationProvider = new();

    internal SongInfoLayout Layout => _layoutToUse.Value;

    private MelonPreferences_Category _preferencesCategory;

    private MelonPreferences_Entry<SongInfoLayout> _layoutToUse;

    public override void OnInitializeMelon()
    {
        _preferencesCategory = MelonPreferences.CreateCategory("Bnfour_SongInfo");
        _layoutToUse = _preferencesCategory.CreateEntry("Layout", SongInfoLayout.OneLine,
            "Display layout", "Sets the layout to use. \"OneLine\" or \"TwoLines\". Classic two line layout requires Scoreboard characters to move the overlapping vanilla UI.");
    }

    public override void OnLateInitializeMelon()
    {
        base.OnLateInitializeMelon();
        // TODO read up on lifecycle, both mod and our code, and consider moving to OnInitializeMelon without the late part
        if (DurationProvider.ErrorLoadingOverride)
        {
            LoggerInstance.Warning("Unable to load the overrides file (bad JSON?). It will be overwritten if this mod encounters any song not present in its database. To avoid data loss, please quit now and fix the file.");
        }
    }

    public override void OnApplicationQuit()
    {
        base.OnApplicationQuit();

        DurationProvider.Shutdown();
    }

    /*
        tl;dr: nothing to see here

        The following section is used to generate the internal cache of song lengths file,
        included as "Resources/duration_data.json". This should be done by developer
        after the game update to include new data in the new mod version.

        While the end-users could also use this mechanism and use the resulting file
        as an override, the mod also automatically creates and saves overrides for
        the unknown songs, which are also automatically removed when the mod is updated.
        As such, this mode is not available at all at the release build, and the
        steps to the super-seecret way to start the process are officially undocumented.
        (protip: check the code below)

        Due to the quick-and-dirty nature of this feature, it runs extremely ugly:
        the screen just freezes for a few minutes while the BGM continues to play.
    */
#if DEBUG

    private const string OutputFilename = "duration_data.json";
    private readonly int BestGirl = Il2CppPeroPeroGames.GlobalDefines.CharacterDefine.yume;

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();

        if (DataHelper.selectedRoleIndex != BestGirl)
        {
            return;
        }
        var thePanel = GameObject.FindObjectOfType<PnlFeverUISelect>();
        if (thePanel == null || !thePanel.isActiveAndEnabled)
        {
            return;
        }
        // "B" was the random letter i typed while prototyping,
        // now it stands for "Build the cache, bretty blease"
        if (Input.GetKeyDown(KeyCode.B))
        {
            LoggerInstance.Msg("Super secret developer data generator mode engaged!");
            LoggerInstance.Msg("Hang tight, this will take a while...");

            uint counter = 0;
            var total = GlobalDataBase.s_DbMusicTag.allMusicCount;
            // the sorting is used to maintain order in the file
            var result = new SortedList<string, string>(new MusicInfoUidComparer());

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            foreach (var kvp in GlobalDataBase.s_DbMusicTag.m_AllMusicInfo)
            {
                counter++;
                if (counter % 25 == 0)
                {
                    LoggerInstance.Msg($"Processing data {counter}/{total}...");
                }
                // there is that one entry with unfilled data (for random music?)
                if (string.IsNullOrEmpty(kvp?.Key) || string.IsNullOrEmpty(kvp.Value?.music))
                {
                    continue;
                }
                result[kvp.Value.uid] = SongDurationProvider.FormatDuration(SongDurationProvider.GetDurationDirectly(kvp.Value));
            }

            var path = Path.Combine(Application.dataPath, OutputFilename);
            using (var writer = new StreamWriter(path, false))
            {
                writer.Write(JsonConvert.SerializeObject(result, Formatting.Indented));
            }

            stopwatch.Stop();
            LoggerInstance.Msg($"Done in {stopwatch.Elapsed.TotalSeconds:0.00}s <3");
        }
    }

#endif

}
