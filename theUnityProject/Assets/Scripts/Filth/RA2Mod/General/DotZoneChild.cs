using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace RA2Mod.General.Components {
    public class DotZoneChild : MonoBehaviour {

        [SerializeField]
        private HitBox hitbox;

        [SerializeField]
        private ProjectileController projectileController;

        private void Reset() {
            hitbox = GetComponent<HitBox>();
            projectileController = GetComponent<ProjectileController>();
        }
    }
}
