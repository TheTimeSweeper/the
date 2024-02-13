using UnityEngine;

namespace RA2Mod.Survivors.GI.SkillStates
{
    public class BurstFireDuration : BurstFire
    {
        public virtual float baseDuration => 1;
        public override int shurikens => totalShurikens;
        private int totalShurikens;

        public override void OnEnter()
        {
            base.OnEnter();

            totalShurikens = Mathf.RoundToInt(baseDuration / interval);
        }
    }
}