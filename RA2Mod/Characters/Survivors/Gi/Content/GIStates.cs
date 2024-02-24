using RA2Mod.Survivors.GI.SkillStates;
using RA2Mod.Survivors.GI.SkillStates.Mine;

namespace RA2Mod.Survivors.GI
{
    public static class GIStates
    {
        public static void Init()
        {
            Modules.Content.AddEntityState(typeof(ArmMutiny));
            Modules.Content.AddEntityState(typeof(MineArmingMutiny));
            Modules.Content.AddEntityState(typeof(WaitForStickMutiny));
            Modules.Content.AddEntityState(typeof(WaitForTargetMutiny));

            Modules.Content.AddEntityState(typeof(Fire3RoundPistol));
            Modules.Content.AddEntityState(typeof(FireGunHeavy));
            Modules.Content.AddEntityState(typeof(FireMissile));
            Modules.Content.AddEntityState(typeof(FireMissileHeavy));

            Modules.Content.AddEntityState(typeof(BarricadeMain));
            Modules.Content.AddEntityState(typeof(BarricadeTransform));
            Modules.Content.AddEntityState(typeof(EnterBarricade));
            Modules.Content.AddEntityState(typeof(EnterBarricadeMissile));
            Modules.Content.AddEntityState(typeof(UnBarricade));

            Modules.Content.AddEntityState(typeof(LiterallyCommandoSlide));
            Modules.Content.AddEntityState(typeof(Roll));
            Modules.Content.AddEntityState(typeof(ThrowCaltrops));
            Modules.Content.AddEntityState(typeof(ThrowMine));
        }
    }
}
