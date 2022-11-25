using EntityStates;
using ModdedEntityStates.BaseStates;
using Modules.Survivors;
using RoR2;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ModdedEntityStates.Desolator {

    public class DeployIrradiate : BaseTimedSkillState {

        #region gameplay Values
        public static float DamageCoefficient = 1f;
        public static float BarrierPercentPerEnemy = 0.05f;
        public static float MaxBarrierPercent = 0.5f;
        public static float Range = 60;
        public const float SqrRange = 1600;

        public static float BaseDuration = 3f;
        public static float StartTime = 1f;
        #endregion

        public RoR2.CameraTargetParams.AimRequest aimRequest;
        protected bool _complete;
        private Animator _animator;
        private float _cannonSpin;
        private bool _heldTooLongYaDoofus;
        private bool _inputDown;

        protected virtual GameObject deployProjectilePrefab => Modules.Assets.DesolatorDeployProjectile;

        public override void OnEnter() {
            base.OnEnter();

            InitDurationValues(BaseDuration, StartTime);
            _animator = base.GetModelAnimator();

            Util.PlaySound("Play_Desolator_Deploy", base.gameObject);
            PlayCrossfade("FullBody, Override", "DeployPump",/* "DeployPump.playbackRate", duration,*/ 0.05f);

            if (base.isAuthority) {
                DropRadiationProjectile();
                GiveBarrierPerEnemy();
            }

            if (NetworkServer.active) {
                characterBody.AddBuff(Modules.Buffs.desolatorArmorMiniBuff);
            }

            if (isAuthority && base.inputBank.skill4.down) {
                _heldTooLongYaDoofus = true;
            }

            characterBody.hideCrosshair = true;
        }
        
        public override void FixedUpdate() {
            base.FixedUpdate();

            if (_heldTooLongYaDoofus && isAuthority && base.inputBank.skill4.justReleased) {
                _heldTooLongYaDoofus = false;
            }
            if (!_heldTooLongYaDoofus && isAuthority && base.inputBank.skill4.justPressed) {
                _inputDown = true;
            }

            //bit of a hack to get around Body ESM not being in GenericCharacterMain
            if (isAuthority && base.inputBank.skill4.justReleased && _inputDown) {
                skillLocator.special.ExecuteIfReady();
            }
            
            if (isAuthority && inputBank.skill3.down) {
                skillLocator.utility.ExecuteIfReady();
            }
        }
        
        public override void Update() {
            base.Update();
            _cannonSpin = Mathf.Lerp(0, 2.2f, fixedAge/duration);
            _animator.SetFloat("CannonSpin", _cannonSpin, 0.1f, Time.deltaTime);  //bit of a magic number because animation for some reason resets at 1.0
            _animator.SetFloat("CannonBarCharge", Mathf.Min(fixedAge / duration, 0.99f), 0.1f, Time.deltaTime);
        }

        private void GiveBarrierPerEnemy() {
             
            List<TeamComponent> enemies = Helpers.GatherEnemies(teamComponent.teamIndex);
            int nearbyEnemies = 0;
            for (int i = 0; i < enemies.Count; i++) {

                if (Vector3.SqrMagnitude(enemies[i].transform.position - transform.position) < SqrRange) {
                    nearbyEnemies++;
                }
            }

            float barrierGained = nearbyEnemies * BarrierPercentPerEnemy * healthComponent.fullBarrier;
            float targetBarrier = Mathf.Min(barrierGained + healthComponent.barrier, healthComponent.fullBarrier * MaxBarrierPercent);

            healthComponent.AddBarrierAuthority(Mathf.Max(targetBarrier - healthComponent.barrier, 0));
        }

        protected void DropRadiationProjectile() {
            FireProjectileInfo fireProjectileInfo = new FireProjectileInfo {
                projectilePrefab = deployProjectilePrefab,
                crit = base.RollCrit(),
                force = 0f,
                damage = this.damageStat * DamageCoefficient,
                owner = base.gameObject,
                rotation = Quaternion.identity,
                position = base.characterBody.corePosition,
                //damageTypeOverride = DamageType.WeakOnHit
            };

            ModifyProjectile(ref fireProjectileInfo);

            ProjectileManager.instance.FireProjectile(fireProjectileInfo);
        }

        protected virtual void ModifyProjectile(ref FireProjectileInfo fireProjectileInfo) { }

        protected override EntityState ChooseNextState() {
            _complete = true;
            return new DeployIrradiate { aimRequest = this.aimRequest };
        }

        public override void OnExit() {
            base.OnExit();

            characterBody.hideCrosshair = false;

            if (NetworkServer.active) {

                characterBody.RemoveBuff(Modules.Buffs.desolatorArmorMiniBuff);
            }

            if (!_complete) {
                aimRequest?.Dispose();

                skillLocator.special.UnsetSkillOverride(gameObject, DesolatorSurvivor.cancelDeploySkillDef, RoR2.GenericSkill.SkillOverridePriority.Contextual);

                PlayCrossfade("FullBody, Override", "BufferEmpty", 0.5f);

                PlayCrossfade("RadCannonBar", "DesolatorIdlePose", 0.1f);
                PlayCrossfade("RadCannonSpin", "DesolatorIdlePose", 0.1f);
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.PrioritySkill;
        }
    }
}
