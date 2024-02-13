using EntityStates;
using UnityEngine;

namespace RA2Mod.Survivors.GI.SkillStates
{
    public class BurstFire : BaseSkillState
    {
        public virtual float baseInterval => 0.1f;
        public virtual float baseFinalInterval => 0.4f;
        public virtual int shurikens => 3;

        protected float interval;
        protected float finalInterval;

        protected int thrownShurikens;
        protected float intervalTim;

        public override void OnEnter()
        {
            base.OnEnter();
            interval = baseInterval / attackSpeedStat;
            finalInterval = baseFinalInterval / attackSpeedStat;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            intervalTim -= Time.fixedDeltaTime;

            while (intervalTim <= 0)
            {
                //throw normally until the last one
                if (thrownShurikens < shurikens - 1)
                {

                    Fire();
                    thrownShurikens++;
                    intervalTim += interval;
                }
                //at the last one, set the final interval
                else if (thrownShurikens == shurikens - 1)
                {

                    Fire();
                    thrownShurikens++;
                    intervalTim += finalInterval;
                }
                //after the final interval, don't throw and end state
                else if (thrownShurikens == shurikens)
                {
                    thrownShurikens++;
                    base.outer.SetNextStateToMain();
                }
                else if (thrownShurikens > shurikens)
                {
                    return;
                }
            }
        }

        protected virtual void Fire()
        {

        }
    }
}