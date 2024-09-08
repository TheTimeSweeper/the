using RoR2.Projectile;
using UnityEngine;

namespace RA2Mod.General.Components {

    [RequireComponent(typeof(QuaternionPID))]
    [RequireComponent(typeof(ProjectileSteerTowardTarget))]
    public class ProjectileOverrideMissilePID : MonoBehaviour {
        [SerializeField]
        private ProjectileSteerTowardTarget steerComponent;
        [SerializeField]
        private QuaternionPID pidComopnent;
        [SerializeField]
        private ProjectileTargetComponent targetComponent;
        [SerializeField]
        private Collider colliderToActivate;

        [SerializeField]
        private float overrideSqrDistance = 16f;

        void Reset() {
            steerComponent = GetComponent<ProjectileSteerTowardTarget>();
            pidComopnent = GetComponent<QuaternionPID>();
            targetComponent = GetComponent<ProjectileTargetComponent>();
        }

        void FixedUpdate() {
            float distance = (transform.position - targetComponent.target.position).sqrMagnitude;
            bool overriding = targetComponent.target && distance < overrideSqrDistance;
            steerComponent.enabled = overriding;
            pidComopnent.enabled = !overriding;
            if (colliderToActivate)
            {
                colliderToActivate.enabled = overriding;
            }
        }
    }
}
