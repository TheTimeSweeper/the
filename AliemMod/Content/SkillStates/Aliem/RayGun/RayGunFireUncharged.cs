using EntityStates;
using RoR2;
using System.Collections.Generic;

namespace ModdedEntityStates.Aliem
{
    public class RayGunFireUncharged : RayGunFire
    {
        private bool cancelCancelCancel;

        public override void OnEnter()
        {
            OverlapAttack overlap = InitMeleeOverlap(0, null, GetModelTransform(), "KnifeDetection");
            List<HurtBox> hitResults = new List<HurtBox>();
            
            overlap.Fire(hitResults);
            if(hitResults.Count > 0 && isAuthority)
            {
                outer.SetNextState(new CloseRangeKnife());
                cancelCancelCancel = true;
                return;
            }

            base.OnEnter();
        }

        public override void FixedUpdate()
        {
            if (cancelCancelCancel)
                return;

            base.FixedUpdate();
        }
    }
}