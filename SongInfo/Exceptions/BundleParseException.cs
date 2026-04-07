namespace Bnfour.MuseDashMods.SongInfo.Exceptions;

/// <summary>
/// Thrown when the bundle file does not match the expectations.
/// </summary>
/// <param name="bundleName">Name of the bundle file that cannot be processed.</param>
public class BundleParseException(string bundleName) : System.Exception
{
    public string BundleName => bundleName;
}
