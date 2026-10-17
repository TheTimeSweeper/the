
using LfPMod.Survivors.Mamluk.SkillStates;

namespace LfPMod.Survivors.Mamluk.MamlukContent
{
    public static class CharacterStates
    {
        public static void Init()
        {
            Modules.Content.AddEntityState(typeof(Axe_M1));
            Modules.Content.AddEntityState(typeof(Axe_M2Enter));
            Modules.Content.AddEntityState(typeof(Shield_M1));
            Modules.Content.AddEntityState(typeof(Shield_M3Enter));
            Modules.Content.AddEntityState(typeof(Spear_M1));
            Modules.Content.AddEntityState(typeof(Spear_M4Enter));
        }
    }
}
