using EntityStates;
using RoR2;
using UnityEngine;

namespace RA2Mod.Survivors.Tesla.States
{
    public class ChargeZapPunch : BaseSkillState
    {

        public static float MinCharge = 0.1f;
        public static float MaxCharge = 1;
        public static float BaseChargeTime = 2f;

        private float _maxChargeTime;
        private bool _reachedMax;
        private bool _success;

        private uint _chargeSoundID;
        private bool _playingLoop;
        private bool _playingLoop2;

        public override void OnEnter()
        {
            base.OnEnter();

            _maxChargeTime = BaseChargeTime / attackSpeedStat;

            PlayAnimation("Gesture, Override", "ShockPunchBeefCharge", "PunchCharge.playbackRate", _maxChargeTime);

            _chargeSoundID = Util.PlaySound("Play_TeslaChargingUp", gameObject);

            StartAimMode(_maxChargeTime);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            ManageSound();

            if (!_reachedMax && fixedAge > _maxChargeTime)
            {
                _reachedMax = true;

                EffectManager.SimpleMuzzleFlash(LegacyResourcesAPI.Load<GameObject>("prefabs/effects/omnieffect/omniimpactvfxlightning"),
                                                gameObject,
                                                "MuzzleGauntlet",
                                                true);

                Util.PlaySound("Play_TeslaChargingComplete", gameObject);
            }

            characterBody.SetSpreadBloom(Mathf.Lerp(0, 0.6f, fixedAge / _maxChargeTime), true);

            if (inputBank.skill2.justReleased)
            {

                float charge = Mathf.Lerp(MinCharge, MaxCharge, fixedAge / _maxChargeTime);

                FireChargedZapPunch newNextState = new FireChargedZapPunch();
                newNextState.chargeMultiplier = charge;

                _success = true;
                outer.SetNextState(newNextState);
            }
        }


        private void ManageSound()
        {
            //magic number for length of RayGunChargeUp sound
            if (!_playingLoop && !_playingLoop2 && fixedAge > 5.6f)
            {
                _playingLoop = true;

                AkSoundEngine.StopPlayingID(_chargeSoundID);
                _chargeSoundID = Util.PlaySound("Play_TeslaChargingLoop", gameObject);
            }

            if (!_playingLoop2 && fixedAge > _maxChargeTime)
            {
                _playingLoop2 = true;
                AkSoundEngine.StopPlayingID(_chargeSoundID);

                _chargeSoundID = Util.PlaySound("Play_TeslaChargingLoopHigh", gameObject);
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public override void OnExit()
        {
            base.OnExit();

            AkSoundEngine.StopPlayingID(_chargeSoundID);

            //refund if interrupted
            if (!_success)
            {
                activatorSkillSlot.AddOneStock();
            }
        }
    }
}