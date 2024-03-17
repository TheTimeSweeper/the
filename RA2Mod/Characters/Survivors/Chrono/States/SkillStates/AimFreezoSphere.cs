using EntityStates;
using UnityEngine;

namespace RA2Mod.Survivors.Chrono.States
{
    public class AimFreezoSphere : AimChronosphere1
    {

        public override void OnEnter()
        {
            base.OnEnter(); 

            viewRadius = ChronoConfig.M3_Freezosphere_Radius.Value;
        }
        protected override EntityState ActuallyPickNextState(Vector3 point)
        {
            return new PlaceFreezoSphere
            {
                originalPoint = point,
                cameraOverride = cameraOverride,
                origOrigin = origOrigin
            };
        }
    }
}