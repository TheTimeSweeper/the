using EntityStates;
using System;

namespace KatamariMod.SkillStates.BaseStates
{
    //see example skills below
    public abstract class BaseTimedSkillState : BaseSkillState
    {
        //total duration of the move
        public abstract float TimedBaseDuration { get; }

        //0-1 time relative to duration that the skill starts
        //for example, set 0.5 and the "cast" will happen halfway through the skill
        public abstract float TimedBaseCastStartPercentTime { get; }
        public virtual float TimedBaseCastEndPercentTime => 1;

        public virtual bool DontAttackSpeed => false;

        protected float duration;
        protected float castStartTime;
        protected float castEndTime;
        protected bool hasFired;
        protected bool isFiring;
        protected bool hasExited;

        public override void OnEnter()
        {
            InitDurationValues();
            base.OnEnter();
        }

        protected virtual void InitDurationValues()
        {
            duration = TimedBaseDuration / (DontAttackSpeed? 1 : attackSpeedStat);
            this.castStartTime = TimedBaseCastStartPercentTime * duration;
            this.castEndTime = TimedBaseCastEndPercentTime * duration;
        }

        protected virtual void OnCastEnter() { }
        protected virtual void OnCastFixedUpdate() { }
        protected virtual void OnCastUpdate() { }
        protected virtual void OnCastExit() { }

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
                if (!hasFired)
                {
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
                SetNextState();
                return;
            }
        }

        protected virtual void SetNextState()
        {
            outer.SetNextStateToMain();
        }

        public override void Update()
        {
            base.Update();
            if (isFiring)
            {
                OnCastUpdate();
            }
        }
    }
}