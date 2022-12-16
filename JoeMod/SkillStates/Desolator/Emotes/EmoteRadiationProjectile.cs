using EntityStates;
using RoR2.Projectile;
using UnityEngine;

namespace ModdedEntityStates.Desolator {
    public class EmoteRadiationProjectile : BaseSkillState {

        public GameObject deployProjectilePrefab => Modules.Assets.DesolatorDeployProjectileEmote;
        public static float DamageCoefficient = 0.2f;
        public static float Range = 10;

        public override void OnEnter() {
            base.OnEnter();

            DropRadiationProjectile();

            base.outer.SetNextStateToMain();
        }

        protected void DropRadiationProjectile() {
            FireProjectileInfo fireProjectileInfo = new FireProjectileInfo {
                projectilePrefab = deployProjectilePrefab,
                crit = base.RollCrit(),
                force = 0f,
                damage = this.damageStat * DamageCoefficient,
                owner = base.gameObject,
                rotation = Quaternion.identity,
                position = base.FindModelChild("MuzzleGauntlet").position,
                //damageTypeOverride = DamageType.WeakOnHit
            };

            ProjectileManager.instance.FireProjectile(fireProjectileInfo);
        }
    }
}
