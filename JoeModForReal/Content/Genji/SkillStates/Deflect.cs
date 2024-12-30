using EntityStates;
using EntityStates.GolemMonster;
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
        public static float Duration => GenjiConfig.deflectDuration.Value;
        public static float MaxProjectileDeflectDistance = 3;

        private DeflectAttackReceiver _deflectAttackReceiver;
        private GameObject _deflectHitbox;
        private ChildLocator _childLocator;
        TemporaryOverlayInstance _temporaryOverlay;
        
        public override void OnEnter() {
            base.OnEnter();

            if (NetworkServer.active) {
                characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);
            }
            _deflectAttackReceiver = characterBody.GetComponent<DeflectAttackReceiver>();
            if (_deflectAttackReceiver != null) {
                _deflectAttackReceiver.OnBulletAttackReceived += DeflectBullet;
                _deflectAttackReceiver.OnGolemLaserReceived += DeflectGolemLaser;

            }

            _childLocator = GetModelChildLocator();
            _deflectHitbox = _childLocator.FindChildGameObject("DeflectHitbox");
            if (_deflectHitbox) {
                _deflectHitbox.SetActive(true);
            }

            StartAimMode(Duration + 1);

            CharacterModel characterModel = base.GetModelTransform().GetComponent<CharacterModel>();

            _temporaryOverlay = TemporaryOverlayManager.AddOverlay(characterModel.gameObject);
            _temporaryOverlay.duration = Duration;
            _temporaryOverlay.originalMaterial = LegacyResourcesAPI.Load<Material>("Materials/matIsShocked");
            _temporaryOverlay.AddToCharacterModel(characterModel);
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
            //this can probably go wrong...
            if (NetworkServer.active) {
                characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);
            }

            if (_deflectAttackReceiver != null) {
                _deflectAttackReceiver.OnBulletAttackReceived -= DeflectBullet;
                _deflectAttackReceiver.OnGolemLaserReceived -= DeflectGolemLaser;
            }

            if (_deflectHitbox) {
                _deflectHitbox.SetActive(false);
            }

            if (_temporaryOverlay!= null) {
                _temporaryOverlay.Destroy();
            }
        }

        private void DeflectProjectiles() {

            List<ProjectileController> projectileInstances = InstanceTracker.GetInstancesList<ProjectileController>();
            float num = MaxProjectileDeflectDistance * MaxProjectileDeflectDistance;
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

            float damage = 1 * damageStat;
            DamageTypeCombo damageType = DamageType.Generic;
            float force = 200;
            if (projectileController.gameObject.TryGetComponent(out ProjectileDamage projectileDamage)) {
                damage = projectileDamage.damage * GenjiConfig.deflectProjectileDamageMultiplier.Value;
                damageType = projectileDamage.damageType;
                force = projectileDamage.force;
            }
            damageType.damageSource = DamageSource.Secondary;

            FireProjectileInfo info = new FireProjectileInfo() {
                projectilePrefab = projectileController.gameObject,
                position = projectileController.gameObject.transform.position,
                rotation = Util.QuaternionSafeLookRotation(aimRay.direction),
                owner = base.characterBody.gameObject,
                damage = damage,
                damageTypeOverride = damageType,
                force = force,
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

            DamageTypeCombo damageTypeCombo = bulletAttack_.damageType;
            damageTypeCombo.damageSource = DamageSource.Secondary;

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

                damage = bulletAttack_.damage * GenjiConfig.deflectBulletAttackDamageMultiplier.Value,
                damageType = damageTypeCombo,
                falloffModel = bulletAttack_.falloffModel,
                force = bulletAttack_.force,
                HitEffectNormal = bulletAttack_.HitEffectNormal,
                procChainMask = default(ProcChainMask),
                procCoefficient = bulletAttack_.procCoefficient,
                maxDistance = bulletAttack_.maxDistance * 2,
                radius = bulletAttack_.radius,
                hitEffectPrefab = bulletAttack_.hitEffectPrefab,
                smartCollision = true,// bulletAttack_.smartCollision,
                sniper = bulletAttack_.sniper,
                tracerEffectPrefab = bulletAttack_.tracerEffectPrefab,
            };
            if(deflectedBullet.tracerEffectPrefab == null) {
                //todo default tracer here
            }

            deflectedBullet.Fire();
        }

        private void DeflectGolemLaser() {

            PlayDeflectEffect();

            if (FireLaser.effectPrefab) {
                EffectManager.SimpleMuzzleFlash(FireLaser.effectPrefab, base.gameObject, "notMercSlash", false);
            }

            if (base.isAuthority) {

                Ray modifiedAimRay = base.GetAimRay();

                float num = 1000f;
                Vector3 vector = modifiedAimRay.origin + modifiedAimRay.direction * num;
                RaycastHit raycastHit;
                if (Physics.Raycast(modifiedAimRay, out raycastHit, num, LayerIndex.world.mask | LayerIndex.defaultLayer.mask | LayerIndex.entityPrecise.mask)) {
                    vector = raycastHit.point;
                }
                new BlastAttack {
                    attacker = base.gameObject,
                    inflictor = base.gameObject,
                    teamIndex = TeamComponent.GetObjectTeam(base.gameObject),
                                                  //multiply a survivor's base damage of 12 to get up to golem's base damage of 20
                    baseDamage = this.damageStat * 1.7f * FireLaser.damageCoefficient * GenjiConfig.deflectGolemLaserDamageMultiplier.Value,
                    baseForce = FireLaser.force * 0.2f,
                    position = vector,
                    radius = FireLaser.blastRadius,
                    falloffModel = BlastAttack.FalloffModel.SweetSpot,
                    bonusForce = FireLaser.force * modifiedAimRay.direction,
                    damageType = DamageTypeCombo.GenericSecondary,
                }.Fire();

                Vector3 origin = modifiedAimRay.origin;
                if (_childLocator) {
                    int childIndex = _childLocator.FindChildIndex("notMercSlash");
                  if (FireLaser.tracerEffectPrefab) {
                      EffectData effectData = new EffectData {
                          origin = vector,
                          start = modifiedAimRay.origin
                      };
                      effectData.SetChildLocatorTransformReference(base.gameObject, childIndex);
                      EffectManager.SpawnEffect(FireLaser.tracerEffectPrefab, effectData, true);
                      EffectManager.SpawnEffect(FireLaser.hitEffectPrefab, effectData, true);
                  }
                }
            }
        }        
        
        private void PlayDeflectEffect() {

            Util.PlaySound("Play_merc_sword_impact", gameObject);

            base.PlayAnimation("Arms, Override", "cast 2", "cast.playbackRate", 0.2f);
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return base.fixedAge > 0.2f? InterruptPriority.Any : InterruptPriority.Skill;
        }
    }
}