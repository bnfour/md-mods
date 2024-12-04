using System.Linq;

using Il2CppAssets.Scripts.Database;
using Il2CppAssets.Scripts.PeroTools.Nice.Components;

using Bnfour.MuseDashMods.AlbumScroll.Data;

namespace Bnfour.MuseDashMods.AlbumScroll.Utilities;

/// <summary>
/// Contains the album-scrolling logic.
/// </summary>
public static class AlbumScroller
{
    public static void ScrollToDifferentAlbum(FancyScrollView scrollView, Direction direction)
    {
        var index = GetIndex(direction);
        scrollView.ScrollTo(index, scrollView.switchDuration);
    }

    private static int GetIndex(Direction direction)
    {
        // this contains ids of songs included in the current view
        // random song entry is always included last with "?" instead of id
        var list = GlobalDataBase.dbMusicTag.m_StageShowMusicUids;
        var currentIndex = GlobalDataBase.dbMusicTag.curSelectedMusicIdx;
        
        var currentAlbum = GetAlbumId(list[currentIndex]);

        var delta = direction == Direction.Forward ? 1 : -1;
        var listCount = list.Count;

        for (int i = currentIndex + delta; i < listCount && i >= 0; i += delta)
        {
            if (GetAlbumId(list[i]) != currentAlbum)
            {
                return i;
            }
        }
        // if index was not found during the for loop, we have to loop through the list:
        // if moving forward, we're looping from random song to the first one,
        // if moving backward, we're looping from the first album to random song
        return direction == Direction.Forward ? 0 : listCount - 1;
    }

    private static string GetAlbumId(string songId)
    {
        // the list contains song ids like "56-1", where the first number is album id,
        // and the second number is zero-based order of a song in the album

        // we're only comparing album ids here
        return songId.Split('-').First();
    }
}
