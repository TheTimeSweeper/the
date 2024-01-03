using EntityStates;
using PlagueMod.Survivors.Plague.Components;
using PlagueMod.Survivors.Plague.SkillDefs;

namespace PlagueMod.Survivors.Plague.SkillStates
{
    public class SelectBomb : BaseSkillState, PlagueBombSelectionSkillDef.IPlagueBombSetSelector
    {
        private PlagueBombSelectorController _selectorComponent;

        public void SetPlagueComponent(PlagueBombSelectorController component)
        {
            _selectorComponent = component;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (inputBank.skill1.justPressed)
            {
                _selectorComponent.ChangeSelectedCasing();
            }
            if (inputBank.skill2.justPressed)
            {
                _selectorComponent.ChangeSelectedPowder();
            }

            if (!inputBank.skill4.down)
            {
                base.outer.SetNextStateToMain();
            }
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}