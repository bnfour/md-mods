using MelonLoader;

namespace Bnfour.MuseDashMods.Experimental
{
    /// <summary>
    /// Experimental mod to test things before actually implementing them,
    /// the code here will be local only before moving to an actual mod or deleting.
    /// </summary>
    public class ExperimentalMod : MelonMod
    {
        public override void OnInitializeMelon()
        {
            base.OnInitializeMelon();

            LoggerInstance.Msg("experimental mod online");
        }
    }
}
