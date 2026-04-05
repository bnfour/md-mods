namespace Bnfour.MuseDashMods.SongInfo.Exceptions;

/// <summary>
/// Exception to throw when (if) the bundle data we search for is split between
/// two 128k blocks Unity compresses separately.
/// </summary>
/// <param name="message">Specifies what improbable thing happened.</param>
public class VeryUnluckyException(string message, string bundleName) : System.Exception(message)
{
    /// <summary>
    /// Name of the file bundle that is generated that unlucky.
    /// </summary>
    public string BundleName => bundleName;
}
