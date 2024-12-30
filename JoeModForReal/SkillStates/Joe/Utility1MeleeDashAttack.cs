using EntityStates;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace ModdedEntityStates.Joe {

    public class Utility1MeleeDashAttack : UtilityBaseDash {

        public static float BaseOverlaps = 3f;

        private float _overlapInterval;
        private int _overlapResets;

        public float chargedDamage = 1;

		private OverlapAttack _attack;

        private Transform _modelTransform;
        
        public override void OnEnter() {

            speedCoefficient = chargedDamage * 2;
            duration = 1f;
            _travelEndPercentTime = 17/40f;//todo testvaluemanager
            
            base.OnEnter();

            characterMotor.Motor.ForceUnground();

            _overlapInterval = (duration * _travelEndPercentTime / (BaseOverlaps)) / attackSpeedStat;

            _modelTransform =  modelLocator.modelTransform;
            modelLocator.enabled = false;

            skillLocator.utility.DeductStock(1);

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
            }
            else {
                modelLocator.enabled = true;
            }
        }

        private void FireAttacks() {

            //yes this scales past framerate cause I'm a maniac
            while(stopwatch > _overlapInterval * _overlapResets) {
                ResetOverlap();
                Util.PlaySound("play_joe_whoosh", gameObject);
                _overlapResets++;

                if (isAuthority) {
                    _attack.Fire();
                }
            }

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
            
            this._attack = base.InitMeleeOverlap(chargedDamage, Modules.Asset.MercImpactEffect, modelLocator.modelTransform, "dash");
            _attack.impactSound = Modules.Asset.FleshSliceSound.index;
            _attack.damageType = DamageTypeCombo.GenericUtility;
            R2API.DamageAPI.AddModdedDamageType(_attack, Modules.DamageTypes.TenticleLifeStealing);
        }

        public override void OnExit() {
            base.OnExit();

            modelLocator.enabled = true;
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            if(fixedAge > duration * _travelEndPercentTime) {
                return InterruptPriority.Any;
            }
            return InterruptPriority.PrioritySkill;
        }
    }
}