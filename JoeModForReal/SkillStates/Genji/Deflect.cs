using EntityStates;
using JoeModForReal.Content.Survivors;
using RoR2;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace ModdedEntityStates.Genji {
    public class Deflect : BaseSkillState {
        public static float Duration = 1;
        public static float MaxDeflectDistance = 2;

        private DeflectBulletAttackReceiver _bulletAttackReceiver;
        private GameObject deflectHitbox;

        public override void OnEnter() {
            base.OnEnter();

            if (NetworkServer.active) {
                characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, Duration);
            }
            _bulletAttackReceiver = characterBody.GetComponent<DeflectBulletAttackReceiver>();
            if (_bulletAttackReceiver != null) {
                _bulletAttackReceiver.OnBulletAttackReceived += DeflectBullet;
            }
            deflectHitbox = GetModelChildLocator().FindChildGameObject("DeflectHitbox");
            if (deflectHitbox) {
                deflectHitbox.SetActive(true);
            }
        }

        public override void FixedUpdate() {
            base.FixedUpdate();

            DeflectProjectiles();

            if(fixedAge > Duration) {
                outer.SetNextStateToMain();
            }
        }

        public override void OnExit() {
            base.OnExit();

            if (_bulletAttackReceiver != null) {
                _bulletAttackReceiver.OnBulletAttackReceived -= DeflectBullet;
            }

            if (deflectHitbox) {
                deflectHitbox.SetActive(false);
            }
        }

        private void DeflectProjectiles() {

            List<ProjectileController> projectileInstances = InstanceTracker.GetInstancesList<ProjectileController>();
            float num = MaxDeflectDistance * MaxDeflectDistance;
            for (int i = 0; i < projectileInstances.Count; i++) {

                ProjectileController projectileController = projectileInstances[i];
                if ((projectileController.transform.position - base.characterBody.corePosition).sqrMagnitude < num) {
                    DeflectProjectile(projectileController);
                }
            }
        }

        private void DeflectProjectile(ProjectileController projectileController) {

            if (projectileController.owner == gameObject)
                return;

            PlayDeflectEffect();

            projectileController.owner = gameObject;

            Ray aimRay = base.GetAimRay();

            FireProjectileInfo info = new FireProjectileInfo() {
                projectilePrefab = projectileController.gameObject,
                position = projectileController.gameObject.transform.position,
                rotation = Util.QuaternionSafeLookRotation(aimRay.direction),
                owner = base.characterBody.gameObject,
                damage = 1,
                force = 200f,
                crit = base.RollCrit(),
                damageColorIndex = DamageColorIndex.Default,
                target = null,
                speedOverride = 120f,
                fuseOverride = -1f
            };
            if (isAuthority) {
                ProjectileManager.instance.FireProjectile(info);
            }
            Destroy(projectileController.gameObject);
        }

        private void DeflectBullet(BulletAttack bulletAttack_, BulletAttack.BulletHit hitInfo_) {

            PlayDeflectEffect();

            Ray aimRay = GetAimRay();
            BulletAttack deflectedBullet = new BulletAttack {
                aimVector = aimRay.direction,
                origin = aimRay.origin,
                owner = gameObject,
                weapon = gameObject,
                bulletCount = 1,
                damageColorIndex = DamageColorIndex.Default,
                isCrit = base.RollCrit(),
                muzzleName = "notMercSlash",
                minSpread = 0,
                maxSpread = 0,
                spreadPitchScale = 1,
                spreadYawScale = 1,

                damage = bulletAttack_.damage,
                damageType = bulletAttack_.damageType,
                falloffModel = bulletAttack_.falloffModel,
                force = bulletAttack_.force,
                HitEffectNormal = bulletAttack_.HitEffectNormal,
                procChainMask = default(ProcChainMask),
                procCoefficient = bulletAttack_.procCoefficient,
                maxDistance = bulletAttack_.maxDistance,
                radius = bulletAttack_.radius,
                hitEffectPrefab = bulletAttack_.hitEffectPrefab,
                smartCollision = bulletAttack_.smartCollision,
                sniper = bulletAttack_.sniper,
                tracerEffectPrefab = bulletAttack_.tracerEffectPrefab,
            };
            if(deflectedBullet.tracerEffectPrefab == null) {
                //todo default tracer here
            }

            deflectedBullet.Fire();
        }
        

        private void PlayDeflectEffect() {

            Util.PlaySound("GenjiDeflect", gameObject);

            base.PlayAnimation("Arms, Override", "cast 2", "cast.playbackRate", 0.2f);
        }
    }
}