using EntityStates;
using System;

namespace ModdedEntityStates.BaseStates
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
        protected bool isFiring;

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

            if(!hasFired && fixedAge > castStartTime)
            {
                OnCastEnter();
                hasFired = true;
            }

            bool fireStarted = fixedAge >= castStartTime;
            bool fireEnded = fixedAge >= castEndTime;
            isFiring = false;

            //to guarantee attack comes out if at high attack speed the fixedage skips past the endtime
            if ((fireStarted && !fireEnded) || (fireStarted && fireEnded && !this.hasFired))
            {
                isFiring = true;
                OnCastFixedUpdate();
            }

            if(fixedAge > duration)
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        public override void Update()
        {
            base.Update();
            if (isFiring)
            {
                OnCastUpdate();
            }
        }

        /// <summary>
        /// SOC design pattern gods pls calm down, I hate it too
        /// </summary>
        protected virtual void PlaySoundAuthority(string sound) {
            if (isAuthority)
                RoR2.Util.PlaySound(sound, gameObject);
        }

        protected virtual void OnCastEnter() { }
        protected virtual void OnCastFixedUpdate() { }
        protected virtual void OnCastUpdate() { }
    }
}