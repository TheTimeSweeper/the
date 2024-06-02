using UnityEngine;
using RoR2.Projectile;

namespace AliemMod.Components.Bundled {
    public class AddColliderToProjectileController: MonoBehaviour {

        [SerializeField]
        private ProjectileController projectileController;

        [SerializeField]
        private Collider[] colliders;
    }
}
