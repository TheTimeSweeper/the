using EntityStates;
using RoR2;
using RoR2.Projectile;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ModdedEntityStates.TeslaTrooper {
    public class ZapPunchWithDeflect : ZapPunch {

        public static float DeflectDamageCoefficient = 3f;
        public static float DeflectRadius = 6f;

        private Transform deflectMuzzleTransform;
        private float deflectEndTime;

        public override void OnEnter() {

            base.OnEnter();

            this.deflectEndTime = 0.55f * animationDuration;
            deflectMuzzleTransform = FindModelChild("PunchHitbox");
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
                ProjectileController deflectedProjectile = instancesList[i];

                if (!deflectedProjectile.cannotBeDeleted && deflectedProjectile.teamFilter.teamIndex != teamComponent.teamIndex && (deflectedProjectile.transform.position - deflectMuzzleTransform.position).sqrMagnitude < deflectRadiusSquared) {

                    Vector3 dist = deflectedProjectile.gameObject.transform.position - deflectMuzzleTransform.position;

                    FireProjectileInfo info = new FireProjectileInfo() {
                        projectilePrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/MageLightningboltBasic"),
                        position = deflectMuzzleTransform.position + dist * 0.3f,
                        rotation = Util.QuaternionSafeLookRotation(GetAimRay().direction, Vector3.up),
                        owner = base.characterBody.gameObject,
                        damage = base.characterBody.damage * DeflectDamageCoefficient,
                        force = 200f,
                        crit = rolledCrit,
                        damageColorIndex = DamageColorIndex.Default,
                        target = null,
                        speedOverride = 120f,
                        fuseOverride = -1f
                    };
                    ProjectileManager.instance.FireProjectile(info);

                    EffectManager.SimpleEffect(RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/effects/omnieffect/omniimpactvfxlightning"),
                                               deflectedProjectile.gameObject.transform.position,
                                               Quaternion.identity,
                                               true);
                    ApplyHitstop();

                    EntityState.Destroy(deflectedProjectile.gameObject);
                }

            }
        }
    }
}