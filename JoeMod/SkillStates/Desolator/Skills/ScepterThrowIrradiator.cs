using RoR2.Projectile;
using UnityEngine;

namespace ModdedEntityStates.Desolator {
    public class ScepterThrowIrradiator : ThrowIrradiator {
        
        public static float explosionDamageCoefficient = 16f / DamageCoefficient;

        protected override void ModifyProjectile(ref FireProjectileInfo fireProjectileInfo) {
            fireProjectileInfo.projectilePrefab = Modules.Assets.DesolatorIrradiatorProjectileScepter;
        }
    }
}
