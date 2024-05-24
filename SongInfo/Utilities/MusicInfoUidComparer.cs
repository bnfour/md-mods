using System.Collections.Generic;

namespace Bnfour.MuseDashMods.SongInfo.Utilities;

/// <summary>
/// A comparer for MusicInfo.uid to sort them by album, and by song order within one album.
/// Does not care about strings not formatted as UIDs.
/// </summary>
public class MusicInfoUidComparer : IComparer<string>
{
    // the UIDs are formatted "{album id}-{song id}", where both ids are integers starting from 0,
    // like "0-8", "27-0", or "37-5"
    
    // the default string comparer puts "10-0" before "2-0" (and "0-10" before "0-2")
    // because it sorts entire strings alphabetically and does not know nor care
    // about inner structure we'd like to sort by

    public int Compare(string x, string y)
    {
        // this method returns 0 ("the inputs are equal" meaning "i don't care")
        // if either input string does not match the expected format

        var xParts = x.Split('-');
        var yParts = y.Split('-');

        if (xParts.Length != 2 || xParts.Length != yParts.Length)
        {
            return 0;
        }

        for (int i = 0; i < 2; i++)
        {
            if (!int.TryParse(xParts[i], out int xPart)
                || !int.TryParse(yParts[i], out int yPart))
            {
                return 0;
            }

            if (xPart != yPart)
            {
                return xPart - yPart;
            }
        }
        return 0;
    }
}
