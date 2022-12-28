using EntityStates;
using System;

namespace ModdedEntityStates.BaseStates
{
    public class BaseTimedSkillState : BaseSkillState
    {
        public float TimedBaseDuration;
        public float TimedBaseCastStartTime;
        public float TimedBaseCastEndTime;

        protected float duration;
        protected float castStartTime;
        protected float castEndTime;
        protected bool hasFired;
        protected bool isFiring;
        protected bool hasExited;

        protected virtual void InitDurationValues(float baseDuration, float baseCastStartTime, float baseCastEndTime = 1)
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

            bool fireStarted = fixedAge >= castStartTime;
            bool fireEnded = fixedAge >= castEndTime;
            isFiring = false;

            //to guarantee attack comes out if at high attack speed the fixedage skips past the endtime
            if ((fireStarted && !fireEnded) || (fireStarted && fireEnded && !this.hasFired))
            {
                isFiring = true;
                OnCastFixedUpdate();
                if (!hasFired) {
                    OnCastEnter();
                    hasFired = true;
                }
            }

            if (fireEnded && !hasExited)
            {
                hasExited = true;
                OnCastExit();
            }

            if (fixedAge > duration)
            {
                EntityState state = ChooseNextState();
                if (state == null) {
                    outer.SetNextStateToMain();
                } else {
                    outer.SetNextState(state);
                }
                return;
            }
        }

        protected virtual EntityState ChooseNextState() {
            return null;
        }

        public override void Update()
        {
            base.Update();
            if (isFiring)
            {
                OnCastUpdate();
            }
        }

        protected virtual void OnCastEnter() { }
        protected virtual void OnCastFixedUpdate() { }
        protected virtual void OnCastUpdate() { }
        protected virtual void OnCastExit() { }
    }
}