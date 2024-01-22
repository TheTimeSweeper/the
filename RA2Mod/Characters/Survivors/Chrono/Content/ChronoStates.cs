using RA2Mod.Survivors.Chrono.SkillStates;

namespace RA2Mod.Survivors.Chrono
{
    public static class ChronoStates
    {
        public static void Init()
        {
            Modules.Content.AddEntityState(typeof(Shoot));

            Modules.Content.AddEntityState(typeof(ChronoBomb));
        }
    }
}
