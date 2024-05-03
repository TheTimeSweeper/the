using RoR2.Projectile;
using UnityEngine;

namespace RA2Mod.Survivors.Desolator.States
{
    public class ScepterThrowIrradiator : ThrowIrradiator {
        public static float finalExplosionDamageCoefficient = 16f;
        public static float explosionDamageCoefficient = finalExplosionDamageCoefficient / DamageCoefficient;

        protected override void ModifyProjectile(ref FireProjectileInfo fireProjectileInfo) {
            fireProjectileInfo.projectilePrefab = DesolatorAssets.DesolatorIrradiatorProjectileScepter;
        }
    }
}
