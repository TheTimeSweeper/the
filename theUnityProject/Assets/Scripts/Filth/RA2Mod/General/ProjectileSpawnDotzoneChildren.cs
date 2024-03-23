using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace RA2Mod.General.Components {
    public class ProjectileSpawnDotzoneChildren : MonoBehaviour {

        [SerializeField]
        private ProjectileController parentProjectile;

        [SerializeField]
        private HitBoxGroup parenthitboxGroup;

        [SerializeField]
        private DotZoneChild childPrefab;

        [SerializeField]
        private Transform[] childProjectiles;

        [ContextMenu("spown")]
        public void Spown() {
            for (int i = 0; i < childProjectiles.Length; i++) {
                Instantiate(childPrefab, childProjectiles[i]);
            }
        }
    }
}
