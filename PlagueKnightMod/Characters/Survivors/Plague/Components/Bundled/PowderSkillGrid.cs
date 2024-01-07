using RoR2;

namespace PlagueMod.Survivors.Plague.Components
{
    public class PowderSkillGrid : PlagueSkillGrid
    {
        public override void OnPointerDown(GenericSkill targetSkill)
        {
            bombSelectUI.plagueBombSelectorController.SetPrimaryGenericSkill(targetSkill);
        }
    }
}
