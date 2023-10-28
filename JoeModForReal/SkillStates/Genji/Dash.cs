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

        public float damageCoefficient = 4;
        public float speed = 6.9f;

        private OverlapAttack _attack;

        private Transform _modelTransform;

        public override void OnEnter() {

            speedCoefficient = speed;
            duration = 0.5f;
            _travelEndPercentTime = 1;//todo testvaluemanager

            base.OnEnter();

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

            this._attack = base.InitMeleeOverlap(damageCoefficient, Modules.Assets.MercImpactEffect, modelLocator.modelTransform, "dash");
            _attack.impactSound = Modules.Assets.FleshSliceSound.index;
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