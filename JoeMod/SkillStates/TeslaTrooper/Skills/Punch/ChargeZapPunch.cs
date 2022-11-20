using EntityStates;
using RoR2;
using UnityEngine;

namespace ModdedEntityStates.TeslaTrooper {
    public class ChargeZapPunch : BaseSkillState {

        public static float MinCharge = 0.1f;
        public static float MaxCharge = 1;
        public static float BaseChargeTime = 2.5f;

        private float _maxChargeTime;
        private bool _reachedMax;
        private bool _success;

        public override void OnEnter() {
            base.OnEnter();

            _maxChargeTime = /*BaseChargeTime*/TestValueManager.value1 / attackSpeedStat;

            PlayAnimation("Gesture, Override", "ShockPunchBeefCharge", "PunchCharge.playbackRate", _maxChargeTime);
        }

        public override void FixedUpdate() {
            base.FixedUpdate();

            if(!_reachedMax && fixedAge > _maxChargeTime) {
                _reachedMax = true;

                EffectManager.SimpleMuzzleFlash(Modules.Assets.LoadAsset<GameObject>("prefabs/effects/omnieffect/omniimpactvfxlightning"),
                                                gameObject,
                                                "MuzzleGauntlet",
                                                true);
            }

            base.characterBody.SetSpreadBloom(Mathf.Lerp(0, 0.6f, base.fixedAge / _maxChargeTime), true);

            if (inputBank.skill2.justReleased) {

                float charge = Mathf.Lerp(MinCharge, MaxCharge, fixedAge / _maxChargeTime);

                FireChargedZapPunch newNextState = new FireChargedZapPunch();
                newNextState.chargeMultiplier = charge;

                _success = true;
                outer.SetNextState(newNextState);
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Skill;
        }

        public override void OnExit() {
            base.OnExit();
            //refund if interrupted
            if (!_success)
                base.activatorSkillSlot.AddOneStock();
        }
    }
}