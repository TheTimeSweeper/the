using JetBrains.Annotations;
using RoR2;
using RoR2.Skills;
using UnityEngine;

namespace JoeMod {
    public class TeslaTrackingResettingSkillDef : SkillDef {

        protected class InstanceData : SkillDef.BaseSkillInstanceData {

            public TeslaTrackerComponent teslaTracker;

            public float timeoutTimer;
            public bool hasExtraStock;
        }

        public float timeoutDuration = 3;
        public Sprite refreshedIcon;

        public override SkillDef.BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot) {
            return new TeslaTrackingResettingSkillDef.InstanceData {
                teslaTracker = skillSlot.GetComponent<TeslaTrackerComponent>()
            };
        }

        private static bool HasTarget([NotNull] GenericSkill skillSlot) {

            TeslaTrackerComponent teslaTracker = ((InstanceData)skillSlot.skillInstanceData).teslaTracker;
            HurtBox trackingTarget = teslaTracker?.GetTrackingTarget();

            return trackingTarget != null;
        }
        
        public override bool CanExecute([NotNull] GenericSkill skillSlot) {
            return HasTarget(skillSlot) && !IsTargetCooldown(skillSlot) && base.CanExecute(skillSlot);
        }

        public override bool IsReady([NotNull] GenericSkill skillSlot) {
            return base.IsReady(skillSlot) && HasTarget(skillSlot);
        }

        private static bool IsTargetCooldown([NotNull] GenericSkill skillSlot) {

            TeslaTrackerComponent teslaTracker = ((InstanceData)skillSlot.skillInstanceData).teslaTracker;
            HurtBox trackingTarget = teslaTracker?.GetTrackingTarget();

            return trackingTarget != null && trackingTarget.healthComponent.body.HasBuff(Modules.Buffs.blinkCooldownBuff);
        }

        public override void OnExecute([NotNull] GenericSkill skillSlot) {
            base.OnExecute(skillSlot);
            InstanceData instanceData = (InstanceData)skillSlot.skillInstanceData;

            if (!IsTargetCooldown(skillSlot)) {
                skillSlot.stock++;
                instanceData.hasExtraStock = true;
                instanceData.timeoutTimer = timeoutDuration;
            } else {
                instanceData.hasExtraStock = false;
            }
        }

        // Token: 0x060045D6 RID: 17878 RVA: 0x00121E18 File Offset: 0x00120018
        public override void OnFixedUpdate([NotNull] GenericSkill skillSlot) {
            base.OnFixedUpdate(skillSlot);
            InstanceData instanceData = (InstanceData)skillSlot.skillInstanceData;

            instanceData.timeoutTimer -= Time.fixedDeltaTime;
            if (instanceData.timeoutTimer <= 0f && instanceData.hasExtraStock) {
                skillSlot.stock--;
                instanceData.hasExtraStock = false;
            }
        }

        public override Sprite GetCurrentIcon([NotNull] GenericSkill skillSlot) {
            return ((InstanceData)skillSlot.skillInstanceData).hasExtraStock ? refreshedIcon : icon;
        }
    }
}
