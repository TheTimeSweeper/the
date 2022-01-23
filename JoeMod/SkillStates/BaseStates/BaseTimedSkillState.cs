using EntityStates;

namespace HenryMod.ModdedEntityStates.BaseStates
{
    public class BaseTimedSkillState : BaseSkillState
    {
        public static float TimedBaseDuration;
        public static float TimedBaseCastStartTime;
        public static float TimedBaseCastEndTime;

        protected float duration;
        protected float castStartTime;
        protected float castEndTime;
        protected bool hasFired;

        protected virtual void SetDurationValues(float baseDuration, float baseCastStartTime, float baseCastEndTime = 1)
        {
            TimedBaseDuration = baseDuration;
            TimedBaseCastStartTime = baseCastStartTime;
            TimedBaseCastEndTime = baseCastEndTime;

            duration = TimedBaseDuration / base.attackSpeedStat;
            castStartTime = baseCastStartTime * duration;
            castEndTime = baseCastEndTime * duration;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if(!hasFired && fixedAge > castStartTime)
            {
                OnCastEnter();
                hasFired = true;
            }

            bool fireStarted = fixedAge >= castStartTime;
            bool fireEnded = fixedAge >= castEndTime;

            //to guarantee attack comes out if at high attack speed the fixedage skips past the endtime
            if ((fireStarted && !fireEnded) || (fireStarted && fireEnded && !this.hasFired))
            {
                OnCastFixedUpdate();
            }

            if(fixedAge > duration)
            {
                outer.SetNextStateToMain();
            }
        }

        protected virtual void OnCastEnter() { }

        protected virtual void OnCastFixedUpdate() { }
    }
}