using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace RA2Mod.General.Components
{
    public class DotZoneChild : MonoBehaviour
    {
        [SerializeField]
        public HitBox hitbox;

        [SerializeField]
        public ProjectileController projectileController;

        private void Reset()
        {
            hitbox = GetComponent<HitBox>();
            projectileController = GetComponent<ProjectileController>();
        }
    }
}