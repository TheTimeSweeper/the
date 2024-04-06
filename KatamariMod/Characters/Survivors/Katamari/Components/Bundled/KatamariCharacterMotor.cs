using KinematicCharacterController;
using RoR2;
using System;
using UnityEngine;

namespace KatamariMod.Survivors.Katamari.Components
{
    public class KatamariCharacterMotor : CharacterMotor
    {
        public float speed;
        //public override void OnStartAuthority()
        //{
        //    base.OnStartAuthority();
        //}

        public override void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
            //base.OnGroundHit(hitCollider, hitNormal, hitPoint, ref hitStabilityReport);
        }

        public override void BeforeCharacterUpdate(float deltaTime)
        {
            isAirControlForced = true;
            base.BeforeCharacterUpdate(deltaTime);
        }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            base.UpdateVelocity(ref currentVelocity, deltaTime);

            speed = currentVelocity.magnitude;
            airControl = Mathf.Clamp01(1 - speed * KatamariConfig.passive_speedAirControlMultiplier.Value);
        }
    }
}