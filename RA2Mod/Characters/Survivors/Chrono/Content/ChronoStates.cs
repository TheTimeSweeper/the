using RA2Mod.Survivors.Chrono.SkillStates;

namespace RA2Mod.Survivors.Chrono
{
    public static class ChronoStates
    {
        public static void Init()
        {
            Modules.Content.AddEntityState(typeof(ChronoCharacterMain));
            Modules.Content.AddEntityState(typeof(ChronoSprintState));
            Modules.Content.AddEntityState(typeof(Modules.BaseStates.WindDownState));
            Modules.Content.AddEntityState(typeof(PhaseState));

            Modules.Content.AddEntityState(typeof(Shoot));

            Modules.Content.AddEntityState(typeof(ChronoBomb));

            Modules.Content.AddEntityState(typeof(AimChronosphere1));
            Modules.Content.AddEntityState(typeof(AimChronosphere2));
            Modules.Content.AddEntityState(typeof(PlaceChronosphere2));

            Modules.Content.AddEntityState(typeof(Vanish));
        }
    }
}
