﻿using EntityStates;
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
        public static float DamageCoefficient = 2f;
        public static float BarrierPercentPerEnemy = 0.1f;
        public static float MaxBarrierPercent = 0.5f;
        public static float Range = 60;
        public const float SqrRange = 1600;

        public static float BaseDuration = 4f;
        public static float StartTime = 1f;
        #endregion

        public RoR2.CameraTargetParams.AimRequest aimRequest;
        private bool _complete;

        public override void OnEnter() {
            base.OnEnter();

            InitDurationValues(BaseDuration, StartTime);

            Util.PlaySound("Play_Desolator_Deploy", base.gameObject);
            PlayCrossfade("FullBody, Override", "DeployPump",/* "DeployPump.playbackRate", duration,*/ 0.05f);

            if (base.isAuthority) {
                DropRadiationProjectile();
                GiveBarrierPerEnemy();
            }

            if (NetworkServer.active) {
                characterBody.AddBuff(Modules.Buffs.desolatorArmorMiniBuff);
            }
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
                projectilePrefab = Modules.Assets.DesolatorDeployProjectile,
                crit = base.RollCrit(),
                force = 0f,
                damage = this.damageStat * DamageCoefficient,
                owner = base.gameObject,
                rotation = Quaternion.identity,
                position = base.characterBody.corePosition,
                damageTypeOverride = DamageType.WeakOnHit
            };
            ProjectileManager.instance.FireProjectile(fireProjectileInfo);
        }

        protected override EntityState ChooseNextState() {
            _complete = true;
            return new DeployIrradiate { aimRequest = this.aimRequest };
        }

        public override void FixedUpdate() {
            base.FixedUpdate();

            if (base.characterMotor) {
                base.characterMotor.moveDirection = Vector3.zero;
            }
        }

        public override void OnExit() {
            base.OnExit();
            if (!_complete) {
                aimRequest.Dispose();

                skillLocator.special.UnsetSkillOverride(gameObject, DesolatorSurvivor.cancelDeploySkillDef, RoR2.GenericSkill.SkillOverridePriority.Contextual);

                if (NetworkServer.active) {

                    characterBody.RemoveBuff(Modules.Buffs.desolatorArmorMiniBuff);
                }

                PlayCrossfade("FullBody, Override", "BufferEmpty", 0.5f);
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.PrioritySkill;
        }
    }
}