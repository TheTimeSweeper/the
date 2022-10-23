using EntityStates;
using UnityEngine;

namespace ModdedEntityStates.Aliem {
    public class ChargeRayGunBig : BaseSkillState {

        public static float BaseMaxChargeDuration = 3;
        public static float MinDamageCoefficient = 1;
        public static float MaxDamageCoefficient = 10;

        private float _maxChargeDuration;
        private float _chargeTimer;

        public override void OnEnter() {
            base.OnEnter();
            _maxChargeDuration = BaseMaxChargeDuration / attackSpeedStat;

            PlayAnimation("LeftArm, Override", "ChargeGun");
        }

        public override void FixedUpdate() {
            base.FixedUpdate();

            _chargeTimer += Time.fixedDeltaTime;

            if (!inputBank.skill1.down) {
                float dam = Mathf.Lerp(MinDamageCoefficient, MaxDamageCoefficient, _chargeTimer / _maxChargeDuration);
                outer.SetNextState(new RayGunBig(dam));
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.PrioritySkill;
        }
    }
}
