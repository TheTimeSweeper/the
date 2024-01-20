using EntityStates;
using PlagueMod.Survivors.Plague.Components;
using PlagueMod.Survivors.Plague.SkillDefs;

namespace PlagueMod.Survivors.Plague.SkillStates
{
    public class SelectBomb : BaseSkillState, PlagueBombSelectionSkillDef.IPlagueBombSelector
    {
        public PlagueBombSelectorController selectorComponent { get; set; }

        public override void OnEnter()
        {
            base.OnEnter();
            selectorComponent.ShowUI();
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
            selectorComponent.ShowUI(false);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}