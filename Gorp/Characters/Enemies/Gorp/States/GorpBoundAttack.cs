using EntityStates.DefectiveUnit;
using GorpMod.Enemies.Gorp.Content;
using RoR2;
using UnityEngine;

namespace GorpMod.Characters.Enemies.Gorp.States
{
    public class GorpBoundAttack : GorpMortarAttack
    {
        protected override int firings => 1;

        protected override void FireProjectile()
        {

            Vector3 vector = base.trackedTarget.position - transform.position;
            vector.y = 0f;
            float magnitude = vector.magnitude;
            float timetoland = Mathf.Max(minimumTime, magnitude / CharacterConfig.Bound_HorizontalSpeedMultiplier);
            Vector3 position;
            if (!base.getTargetComponent.GetPredictionPosition(timetoland * CharacterConfig.Bound_predictionTime, out position))
            {
                position = base.trackedTarget.position;
            }
            Vector3 initialVelocity = Trajectory.CalculateInitialVelocityFromTime(transform.position, position, timetoland, Physics.gravity.y, DenialProjectile.minimumDistance, DenialProjectile.maximumDistance);

            characterMotor.Motor.ForceUnground();
            characterMotor.velocity = initialVelocity;
        }
    }
}
