using RA2Mod.General.Components;
using RoR2;
using RoR2.Skills;

namespace RA2Mod.General
{
    public static class GeneralHooks
    {
        public static void Init()
        {

            On.RoR2.ModelSkinController.ApplySkin += ModelSkinController_ApplySkin;
        }

        private static void ModelSkinController_ApplySkin(On.RoR2.ModelSkinController.orig_ApplySkin orig, ModelSkinController self, int skinIndex)
        {
            orig(self, skinIndex);
            
            SkinRecolorController skinRecolorController = self.GetComponent<SkinRecolorController>();
            if (skinRecolorController)
            {
                SkillDef color = self.characterModel.body?.skillLocator?.FindSkill("LOADOUT_SKILL_COLOR")?.skillDef;
                if (color)
                    skinRecolorController.SetRecolor(color.skillName.ToLowerInvariant());
            }
        }
    }
}
