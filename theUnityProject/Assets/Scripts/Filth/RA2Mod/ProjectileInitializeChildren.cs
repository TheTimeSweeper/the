using RoR2.Projectile;
using UnityEngine;

namespace RA2Mod.General.Components {
    public class ProjectileInitializeChildren : MonoBehaviour {

        [SerializeField]
        private ProjectileController parentProjectile;
        [SerializeField]
        private ProjectileController[] childProjectiles;

        private void Start() {
            for (int i = 0; i < childProjectiles.Length; i++) {
                childProjectiles[i].owner = parentProjectile.owner;
                childProjectiles[i].procChainMask = parentProjectile.procChainMask;
                childProjectiles[i].procCoefficient = parentProjectile.procCoefficient;
                childProjectiles[i].teamFilter.teamIndex = parentProjectile.teamFilter.teamIndex;
            }
        }
    }
}
