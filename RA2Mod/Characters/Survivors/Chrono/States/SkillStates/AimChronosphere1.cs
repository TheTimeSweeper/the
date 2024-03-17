using EntityStates;
using RA2Mod.Modules;
using RoR2;
using UnityEngine;

namespace RA2Mod.Survivors.Chrono.States
{
    public class AimChronosphere1 : AimChronosphereBase
    {
        protected CameraTargetParams.CameraParamsOverrideHandle cameraOverride;
        protected Transform origOrigin;

        protected override EntityState ActuallyPickNextState(Vector3 point)
        {
            return new AimChronosphere2 {
                originalPoint = point,
                cameraOverride = cameraOverride,
                origOrigin = origOrigin
            };
        }
        
        public override void OnEnter()
        {
            origOrigin = characterBody.aimOriginTransform;
            characterBody.aimOriginTransform = FindModelChild("ChronosphereAimOrigin");
            characterBody.aimOriginTransform.position = new Vector3(characterBody.aimOriginTransform.position.x, transform.position.y + 10, characterBody.aimOriginTransform.position.z);
            
            base.OnEnter();

            cameraOverride = CameraParams.OverrideCameraParams(base.cameraTargetParams, ChronoCameraParams.chronosphereCamera, ChronoConfig.M3ChronosphereCameraLerpTime.Value);
        }

        public override void UpdateTrajectoryInfo(out TrajectoryInfo dest)
        {
            base.UpdateTrajectoryInfo(out dest);

            if(origOrigin == null)
            {
                origOrigin = transform;
            }

            Vector3 vector = dest.hitPoint - origOrigin.position;
            dest.finalRay.origin = origOrigin.position;
            dest.finalRay.direction = vector.normalized;
            dest.speedOverride = this.projectileBaseSpeed;
            dest.travelTime = vector.magnitude / this.projectileBaseSpeed;
        }

        public override void OnExit()
        {
            base.OnExit();
            if (!castSuccessful)
            {
                characterBody.aimOriginTransform = origOrigin;
                cameraTargetParams.RemoveParamsOverride(cameraOverride, ChronoConfig.M3ChronosphereCameraLerpTime.Value);
            }
        }
    }
}