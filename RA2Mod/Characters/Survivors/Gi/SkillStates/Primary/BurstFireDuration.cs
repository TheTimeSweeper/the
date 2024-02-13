using UnityEngine;

namespace RA2Mod.Survivors.GI.SkillStates
{
    public abstract class BurstFireDuration : BurstFire
    {
        public abstract float baseDuration { get; }
        public override int shurikens => totalShurikens;
        private int totalShurikens;

        public override void OnEnter()
        {
            base.OnEnter();

            totalShurikens = Mathf.RoundToInt(baseDuration / interval);
        }
    }
}