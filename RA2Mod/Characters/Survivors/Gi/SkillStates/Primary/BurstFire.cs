using EntityStates;
using UnityEngine;

namespace RA2Mod.Survivors.GI.SkillStates
{
    public abstract class BurstFire : BaseSkillState
    {
        public abstract float baseInterval { get; }
        public abstract float baseFinalInterval { get; }
        public abstract int shurikens { get; }

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

            intervalTim -= Time.deltaTime;

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

        protected abstract void Fire();
    }
}