using RoR2;

namespace PlagueMod.Survivors.Plague.Components
{
    public class CasingSkillGrid : PlagueSkillGrid
    {
        public override void OnPointerDown(GenericSkill targetSkill)
        {
            bombSelectUI.plagueBombSelectorController.SetCasingSkillDef(targetSkill);
        }
    }
}
