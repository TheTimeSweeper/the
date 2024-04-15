using RA2Mod.Survivors.GI.SkillStates;
using RA2Mod.Survivors.GI.SkillStates.Mine;

namespace RA2Mod.Survivors.Conscript
{
    public static class ConscriptStates
    {
        public static void Init()
        {
            Modules.Content.AddEntityState(typeof(ArmMutiny));
        }
    }
}
