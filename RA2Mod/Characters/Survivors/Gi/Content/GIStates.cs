using RA2Mod.Survivors.GI.SkillStates;

namespace RA2Mod.Survivors.GI
{
    public static class GIStates
    {
        public static void Init()
        {
            Modules.Content.AddEntityState(typeof(BaseShoot));

            Modules.Content.AddEntityState(typeof(Roll));

            Modules.Content.AddEntityState(typeof(FireHeavyMissile));
        }
    }
}
