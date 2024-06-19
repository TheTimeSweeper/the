using AliemMod.Modules;
using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace ModdedEntityStates.Aliem
{
    public abstract class BaseChargingState : BaseSkillState, IOffHandable
    {
        public bool isOffHanded { get; set; }

        public abstract float BaseMaxChargeDuration { get; }

        protected virtual string maxChargeSound => "Play_AliemTeslaChargingComplete";
        protected virtual string chargeSound => "Play_RayGunChargeUp";
        protected virtual string chargeLoop1 => "Play_RayGunChargeLoop";
        protected virtual string chargeLoop2 => "Play_RayGunChargeLoopHigh";
        protected virtual string chargeAnimationLayer => "RightArm, Over";

        protected virtual GameObject chargeEffectPrefab => Assets.swirlCharge;
        protected virtual GameObject chargeEffectMaxPrefab => Assets.swirlChargeMax;

        private GameObject _chargeEffect;

        private float _maxChargeDuration;
        private float _chargeTimer;

        private bool _playedMaxChargeEffect;

        private uint _chargeSoundID;
        private bool _playingLoop;
        private bool _playingLoop2;

        protected virtual float GetChargedValue(float min, float max)
        {
            return Mathf.Lerp(min, max, _chargeTimer / _maxChargeDuration); 
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _maxChargeDuration = BaseMaxChargeDuration / attackSpeedStat;

            PlayCrossfade("Gesture, Override", "ChargeGun", 0.2f);
            PlayCrossfade(isOffHanded ? "RightArm, Over" : "LeftArm, Over", "ChargeGun", 0.2f);
            PlayCrossfade(isOffHanded ? "LeftArm, Under" : "RightArm, Under", "ChargeGun", 0.2f);

            _chargeSoundID = Util.PlaySound(chargeSound, gameObject);

            _chargeEffect = Object.Instantiate(chargeEffectPrefab, GetModelBaseTransform());
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            ManageSound();

            StartAimMode();

            _chargeTimer += Time.fixedDeltaTime;

            if (!_playedMaxChargeEffect && _chargeTimer > _maxChargeDuration)
            {
                _playedMaxChargeEffect = true;
                playMaxChargeEffect();
            }

            //base.characterBody.SetSpreadBloom(Mathf.Lerp(minBloomRadius, maxBloomRadius, _chargeTimer / _maxChargeDuration), true);

            if (!GetSkillButton().down && isAuthority)
            {
                float charge = _chargeTimer / _maxChargeDuration;
                characterBody.OnSkillActivated(activatorSkillSlot);
                StartNextState(charge);
            }
        }

        private InputBankTest.ButtonState GetSkillButton()
        {
            return isOffHanded ? inputBank.skill2 : inputBank.skill1;
        }

        protected abstract void StartNextState(float chargeCoefficient);

        private void playMaxChargeEffect()
        {
            if (!string.IsNullOrEmpty(maxChargeSound))
            {
                Util.PlaySound(maxChargeSound, gameObject);
            }

            EffectManager.SimpleMuzzleFlash(chargeEffectMaxPrefab, gameObject, "ModelTransform", false);
        }

        private void ManageSound()
        {
            //magic number for length of RayGunChargeUp sound
            if (!_playingLoop && !_playingLoop2 && fixedAge > 1.2f)
            {
                _playingLoop = true;
                _chargeSoundID = Util.PlaySound(chargeLoop1, gameObject);
            }

            if (!_playingLoop2 && _chargeTimer >= _maxChargeDuration)
            {
                _playingLoop2 = true;
                if (_playingLoop)
                {
                    AkSoundEngine.StopPlayingID(_chargeSoundID);
                }

                _chargeSoundID = Util.PlaySound(chargeLoop2, gameObject);
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

        public override void OnExit()
        {
            base.OnExit();
            AkSoundEngine.StopPlayingID(_chargeSoundID);

            Object.Destroy(_chargeEffect);

            //PlayAnimation("Gesture, Override", "BufferEmpty");
            //PlayAnimation(isOffHanded ? "RightArm, Over" : "LeftArm, Over", "BufferEmpty");
            //PlayAnimation(isOffHanded ? "LeftArm, Under" : "RightArm, Under", "BufferEmpty");
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(isOffHanded);
        }
        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            isOffHanded = reader.ReadBoolean();
        }
    }
}
