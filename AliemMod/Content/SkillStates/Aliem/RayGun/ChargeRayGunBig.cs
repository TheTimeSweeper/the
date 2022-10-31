using EntityStates;
using RoR2;
using System;
using UnityEngine;

namespace ModdedEntityStates.Aliem {
    public class ChargeRayGunBig : BaseSkillState {

        public static float BaseMaxChargeDuration = 3;
        public static float MinDamageCoefficient = 1;
        public static float MaxDamageCoefficient = 10;

        private float minBloomRadius = 0.2f;
        private float maxBloomRadius = 0.8f;

        private float _maxChargeDuration;
        private float _chargeTimer;

        private bool _playedMaxChargeEffect;

        private uint _chargeSoundID;
        private bool _playingLoop;
        private bool _playingLoop2;


        public override void OnEnter() {
            base.OnEnter();
            _maxChargeDuration = BaseMaxChargeDuration / attackSpeedStat;

            PlayAnimation("Gesture, Override", "ChargeGun");

            _chargeSoundID = Util.PlaySound("Play_RayGunChargeUp", gameObject);
        }

        public override void FixedUpdate() {
            base.FixedUpdate();

            ManageSound();

            StartAimMode();

            _chargeTimer += Time.fixedDeltaTime;

            if(!_playedMaxChargeEffect && _chargeTimer > _maxChargeDuration) {
                _playedMaxChargeEffect = true;
                playMaxChargeEffect();
            }

            base.characterBody.SetSpreadBloom(Mathf.Lerp(minBloomRadius, maxBloomRadius, _chargeTimer / _maxChargeDuration), true);
            
            if (!inputBank.skill1.down) {

                float dam = Mathf.Lerp(MinDamageCoefficient, MaxDamageCoefficient, _chargeTimer / _maxChargeDuration);
                string shootSound = dam >= MaxDamageCoefficient ? "Play_RayGunBigClassic" : "Play_RayGun";
                outer.SetNextState(new RayGunBig(dam, shootSound));
            }
        }

        private void playMaxChargeEffect() {
            //Util.PlaySound("Play_RayGunChargeMax", gameObject);

        }

        private void ManageSound() {
                          //magic number for length of RayGunChargeUp sound
            if(!_playingLoop && !_playingLoop2 && fixedAge > 1.2f) {
                _playingLoop = true;
                _chargeSoundID = Util.PlaySound("Play_RayGunChargeLoop", gameObject);
            }

            if(!_playingLoop2 && _chargeTimer >= _maxChargeDuration) {
                _playingLoop2 = true;
                if (_playingLoop) {
                    AkSoundEngine.StopPlayingID(_chargeSoundID);
                }

                _chargeSoundID = Util.PlaySound("Play_RayGunChargeLoopHigh", gameObject);
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.PrioritySkill;
        }

        public override void OnExit() {
            base.OnExit();
            AkSoundEngine.StopPlayingID(_chargeSoundID);
        }
    }
}
