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

        public override void OnEnter()
        {
            base.OnEnter();
            _selectorComponent.ShowUI();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!inputBank.skill4.down)
            {
                base.outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            _selectorComponent.ShowUI(false);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}