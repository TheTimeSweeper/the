using EntityStates;
using MatcherMod.Survivors.Matcher.Components;

namespace MatcherMod.Survivors.Matcher.SkillStates
{
    internal class MatchMenuAI : BaseSkillState
    {
        public MatcherGridController componentFromSkillDef1 { get; set; }

        private float interval = 0.6f;
        private float _tim;

        public override void OnEnter()
        {
            base.OnEnter();

            activatorSkillSlot.DeductStock(1);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (isAuthority)
            {
                _tim -= GetDeltaTime();

                while (_tim < 0)
                {
                    _tim += interval;
                    componentFromSkillDef1.AwardRandomMatchForAI(UnityEngine.Random.Range(1, 3));
                }

                if (fixedAge > 4)
                {
                    outer.SetNextStateToMain();
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}