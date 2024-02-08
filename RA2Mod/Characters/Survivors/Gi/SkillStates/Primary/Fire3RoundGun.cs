using UnityEngine;

namespace RA2Mod.Survivors.GI.SkillStates
{
    public class Fire3RoundGun : BaseFireGun
    {
        public static float baseInterval => GIConfig.M1PistolInterval.Value;
        public static float baseFinalInterval => GIConfig.M1PistolFinalInterval.Value;
        public static int shurikens => GIConfig.M1PistolShots.Value;

        private float interval;
        private float finalInterval;
        private int thrownShurikens;
        private float intervalTim;

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

            Vector3 direction = GetAimRay().direction;
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
    }
}