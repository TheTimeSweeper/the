using EntityStates;
using RA2Mod.Modules.BaseStates;

namespace RA2Mod.Survivors.Conscript.States
{
    public class Reload : BaseTimedSkillState
    {
        public override float TimedBaseDuration => 3;
        public override float TimedBaseCastStartPercentTime => 0.5f;

        public override void OnEnter()
        {
            base.OnEnter();

            PlayAnimation("Arms, Override", "swing1 v2", "swing.playbackRate", duration * 2);
        }

        protected override void OnCastEnter()
        {
            base.OnCastEnter();

            while(activatorSkillSlot.stock < activatorSkillSlot.maxStock)
            {
                activatorSkillSlot.AddOneStock();
            }

        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return fixedAge > castStartTime ? InterruptPriority.Any : InterruptPriority.Skill;
        }
    }
}
