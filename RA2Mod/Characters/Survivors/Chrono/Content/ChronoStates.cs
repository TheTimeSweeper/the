using RA2Mod.General.States;
using RA2Mod.Survivors.Chrono.States;

namespace RA2Mod.Survivors.Chrono
{
    public static class ChronoStates
    {
        public static void Init()
        {
            Modules.Content.AddEntityState(typeof(ChronoCharacterMain));
            Modules.Content.AddEntityState(typeof(ChronoSprintState));
            Modules.Content.AddEntityState(typeof(WindDownState));
            Modules.Content.AddEntityState(typeof(PhaseState));

            Modules.Content.AddEntityState(typeof(ChronoShoot));

            Modules.Content.AddEntityState(typeof(ChronoBomb));

            Modules.Content.AddEntityState(typeof(AimChronosphere1));
            Modules.Content.AddEntityState(typeof(AimChronosphere2));
            Modules.Content.AddEntityState(typeof(PlaceChronosphere2));

            Modules.Content.AddEntityState(typeof(AimFreezoSphere));
            Modules.Content.AddEntityState(typeof(PlaceFreezoSphere));

            Modules.Content.AddEntityState(typeof(Vanish));
        }
    }
}
