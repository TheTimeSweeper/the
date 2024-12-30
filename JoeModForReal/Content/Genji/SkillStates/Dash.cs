using EntityStates;
using JoeModForReal.Content.Survivors;
using RoR2;
using RoR2.Projectile;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace ModdedEntityStates.Genji {

    public class Dash : ModdedEntityStates.Joe.UtilityBaseDash {

        public float damageCoefficient => GenjiConfig.dashDamage.Value;
        public float speed => GenjiConfig.dashSpeed.Value;
        public float dashDuration => GenjiConfig.dashDuration.Value;
        public float minimumDashDuration = 0.1f;

        private OverlapAttack _attack;

        private Transform _modelTransform;
        private bool _startedGrounded;

        public override void OnEnter() {

            speedCoefficient = speed;
            ignoreMoveSpeed = true;
            duration = dashDuration;
            _travelEndPercentTime = 1;//todo testvaluemanager

            base.OnEnter();
            _startedGrounded = characterMotor.isGrounded;
            characterMotor.Motor.ForceUnground();
            _modelTransform = modelLocator.modelTransform;
            modelLocator.enabled = false;

            ResetOverlap();

            if (NetworkServer.active) {
                characterBody.AddTimedBuff(Modules.Buffs.DashArmorBuff, duration + 0.1f);
            }
        }

        // Token: 0x06000E36 RID: 3638 RVA: 0x00011909 File Offset: 0x0000FB09
        protected override Vector3 GetBlinkVector() {
            return base.inputBank.aimDirection;
        }

        protected override void PlayAnimation() {
            PlayAnimation("Fullbody, overried", "meleeDashAttack", "dash.playbackRate", duration);
        }

        public override void FixedUpdate() {
            base.FixedUpdate();

            if (dashing) {

                FireAttacks();

                _modelTransform.rotation = Util.QuaternionSafeLookRotation(blinkVector);
                _modelTransform.position = transform.position;
            } else {
                modelLocator.enabled = true;
            }
            //if (!_startedGrounded && characterMotor.isGrounded && fixedAge > minimumDashDuration) {
            //    base.outer.SetNextStateToMain();
            //}
            if (characterMotor.isGrounded) {
                float dot = Vector3.Dot(characterMotor.estimatedGroundNormal, blinkVector.normalized);
                speedCoefficient += speedCoefficient * dot * GenjiConfig.dashGroundFriction.Value;
            }
        }

        private void FireAttacks() {

            if (isAuthority) {
                _attack.Fire();
            }
        }

        public override void Update() {
            base.Update();
            if (dashing) {
                _modelTransform.position = transform.position;
            }
        }

        private void ResetOverlap() {

            this._attack = base.InitMeleeOverlap(damageCoefficient, Modules.Asset.MercImpactEffect, modelLocator.modelTransform, "dash");
            _attack.impactSound = Modules.Asset.FleshSliceSound.index;
            _attack.damageType = DamageTypeCombo.GenericUtility;
            //R2API.DamageAPI.AddModdedDamageType(_attack, Modules.DamageTypes.TenticleLifeStealing);
        }

        public override void OnExit() {
            base.OnExit();

            modelLocator.enabled = true;
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            if (fixedAge > duration * _travelEndPercentTime) {
                return InterruptPriority.Any;
            }
            return InterruptPriority.PrioritySkill;
        }
    }
}