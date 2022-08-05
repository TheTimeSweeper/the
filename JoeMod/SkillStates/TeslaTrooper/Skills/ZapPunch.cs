using EntityStates;
using ModdedEntityStates.BaseStates; //todo just take make them in root moddedentitystates
using RoR2;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ModdedEntityStates.TeslaTrooper {
    public class ZapPunch : BaseMeleeAttackButEpic {

        #region Gameplay Values
        public static float DamageCoefficient = 2.5f;
        public static float ProcCoefficient = 1f;

        public static float OrbDamageCoefficient = 1.5f;
        public static float OrbProcCoefficient = 0.7f;
        public static int OrbCasts = 20;
        public static float OrbDistance = 20;

        public static float DeflectRadius = 6f;
        #endregion

        public static NetworkSoundEventDef loaderZapFistSoundEvent = RoR2.LegacyResourcesAPI.Load<NetworkSoundEventDef>("EntityStateConfigurations/EntityStates.Loader.SwingZapFist");

        private Transform deflectMuzzleTransform;
        private float deflectEndTime;

        public override void OnEnter() {
            
            base.hitboxName = "PunchHitbox";
            base.damageCoefficient = DamageCoefficient;
            base.procCoefficient = ProcCoefficient;

            base.baseDuration = 1.0f;
            base.attackStartTime = 0.42f;
            base.attackEndTime = 0.71f;
            base.baseEarlyExitTime = 0.8f;
            this.deflectEndTime = 0.55f;

            base.hitStopDuration = 0.12f;
            base.swingSoundString = "";
            base.hitSoundString = "";
            base.muzzleString = "PunchHitboxAnchor"; // swingIndex % 2 == 0 ? "SwingLeft" : "SwingRight";
            base.hitstopAnimationParameter = "Shock.playbackRate";
            base.swingEffectPrefab = Modules.Assets.TeslaZapConeEffect;
            base.hitEffectPrefab = null; // Modules.Assets.swordHitImpactEffect;

            deflectMuzzleTransform = FindModelChild("PunchHitbox");

            //base.impactSound = loaderZapFistSoundEvent.index;
            base.OnEnter();
        }
        
        protected override void PlayAttackAnimation() {
            base.PlayAnimation("Gesture, Override", "ShockPunch", "Punch.playbackRate", duration);
        }

        protected override void OnFireAttackEnter() {

            Vector3 direction = GetAimRay().direction;
            direction.y = Mathf.Max(direction.y, 0);
            FindModelChild("PunchHitboxAnchor").rotation = Util.QuaternionSafeLookRotation(direction);

            if (base.FindModelChild(base.muzzleString)) {
                FireProjectileInfo fireProjectileInfo = default(FireProjectileInfo);
                fireProjectileInfo.position = base.FindModelChild(this.muzzleString).position;
                fireProjectileInfo.rotation = Quaternion.LookRotation(GetAimRay().direction);
                fireProjectileInfo.crit = rolledCrit;
                fireProjectileInfo.damage = 1f * this.damageStat;
                fireProjectileInfo.owner = base.gameObject;
                fireProjectileInfo.projectilePrefab = Modules.Assets.TeslaLoaderZapConeProjectile;
                ProjectileManager.instance.FireProjectile(fireProjectileInfo);
            };
        }

        protected override void FireAttack() {
            base.FireAttack();

            if (NetworkServer.active && base.fixedAge < deflectEndTime * duration) {
                DeflectProjectiles();
            }
        }

        private void DeflectProjectiles() {

            List<ProjectileController> instancesList = InstanceTracker.GetInstancesList<ProjectileController>();

            float deflectRadiusSquared = DeflectRadius * DeflectRadius;

            for (int i = 0; i < instancesList.Count; i++) {
                ProjectileController projectileController = instancesList[i];

                if (!projectileController.cannotBeDeleted && projectileController.teamFilter.teamIndex != teamComponent.teamIndex && (projectileController.transform.position - deflectMuzzleTransform.position).sqrMagnitude < deflectRadiusSquared) {

                    projectileController.owner = gameObject;

                    Vector3 dist = projectileController.gameObject.transform.position - deflectMuzzleTransform.position;

                    FireProjectileInfo info = new FireProjectileInfo() {
                        projectilePrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/MageLightningboltBasic"),
                        position = deflectMuzzleTransform.position + dist * 0.5f,
                        rotation = deflectMuzzleTransform.rotation,
                        owner = base.characterBody.gameObject,
                        damage = base.characterBody.damage * 10f,
                        force = 200f,
                        crit = rolledCrit,
                        damageColorIndex = DamageColorIndex.Default,
                        target = null,
                        speedOverride = 120f,
                        fuseOverride = -1f
                    };
                    ProjectileManager.instance.FireProjectile(info);

                    EntityState.Destroy(projectileController.gameObject);
                }

            }
        }

        protected override void OnHitEnemyAuthority(List<HurtBox> hits) {
            base.OnHitEnemyAuthority(hits);

        }
    }
}