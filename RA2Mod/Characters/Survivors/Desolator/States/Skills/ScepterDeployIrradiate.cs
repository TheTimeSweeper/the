using EntityStates;
using RoR2.Projectile;
using UnityEngine;

namespace RA2Mod.Survivors.Desolator.States
{
    public class ScepterDeployIrradiate : DeployIrradiate {

        public static float ScepterRange = 120f;

        protected override void SetNextState()
        {
            _complete = true;
            ScepterDeployIrradiate state = new ScepterDeployIrradiate { aimRequest = this.aimRequest, activatorSkillSlot = activatorSkillSlot };
            outer.SetNextState(state);
        }

        protected override void ModifyProjectile(ref FireProjectileInfo fireProjectileInfo) {
            fireProjectileInfo.projectilePrefab = DesolatorAssets.DesolatorDeployProjectileScepter;
        }
    }
}
