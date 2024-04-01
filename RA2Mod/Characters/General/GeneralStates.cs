using RA2Mod.General.States;
using RA2Mod.Modules;

namespace RA2Mod.General
{
    public static class GeneralStates
    {
        public static void Init()
        {
            Content.AddEntityState(typeof(WindDownState));
            Content.AddEntityState(typeof(Rest));
        }
    }
}
