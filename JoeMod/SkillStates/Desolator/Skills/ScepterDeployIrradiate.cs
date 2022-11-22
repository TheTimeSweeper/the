using EntityStates;
using RoR2.Projectile;
using UnityEngine;

namespace ModdedEntityStates.Desolator {
    public class ScepterDeployIrradiate : DeployIrradiate {

        public static float ScepterRange = 120f;

        protected override EntityState ChooseNextState() {
            _complete = true;
            return new ScepterDeployIrradiate { aimRequest = this.aimRequest };
        }

        protected override void ModifyProjectile(ref FireProjectileInfo fireProjectileInfo) {
            fireProjectileInfo.projectilePrefab = Modules.Assets.DesolatorDeployProjectileScepter;
        }
    }
}
