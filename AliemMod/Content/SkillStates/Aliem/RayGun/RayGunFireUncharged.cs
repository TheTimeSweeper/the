using EntityStates;
using RoR2;
using System.Collections.Generic;

namespace ModdedEntityStates.Aliem
{
    public class RayGunFireUncharged : RayGunFire
    {
        private bool cancelCancel;

        public override void OnEnter()
        {
            OverlapAttack overlap = InitMeleeOverlap(0, null, GetModelTransform(), "Knife");
            List<HurtBox> hitResults = new List<HurtBox>();
            
            overlap.Fire(hitResults);
            if(hitResults.Count > 0)
            {
                outer.SetNextState(new CloseRangeKnife());
                cancelCancel = true;
                return;
            }

            base.OnEnter();
        }

        public override void FixedUpdate()
        {
            if (cancelCancel)
                return;

            base.FixedUpdate();
        }
    }
}