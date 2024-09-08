using JetBrains.Annotations;
using RoR2;
using RoR2.Skills;
using UnityEngine;

namespace RA2Mod.Survivors.Tesla.SkillDefs
{
    //todo teslamove really want my hascomponentskilldef huh
    public class TeslaTrackingResettingSkillDef : SkillDef
    {
        protected class InstanceData : BaseSkillInstanceData
        {
            public TeslaTrackerComponentDash teslaTracker;

            public float timeoutTimer;
            public bool hasExtraStock;
        }

        public float timeoutDuration = 3;
        public Sprite refreshedIcon;

        public override BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            return new InstanceData
            {
                teslaTracker = skillSlot.GetComponent<TeslaTrackerComponentDash>()
            };
        }

        private static bool HasTarget([NotNull] GenericSkill skillSlot)
        {

            TeslaTrackerComponentDash teslaTracker = ((InstanceData)skillSlot.skillInstanceData).teslaTracker;
            HurtBox trackingTarget = teslaTracker?.GetTrackingTarget();

            return trackingTarget != null;
        }

        public override bool IsReady([NotNull] GenericSkill skillSlot)
        {
            return base.IsReady(skillSlot) && HasTarget(skillSlot);
        }

        public override bool CanExecute([NotNull] GenericSkill skillSlot)
        {
            return HasTarget(skillSlot) /*&& !IsTargetCooldown(skillSlot)*/ && base.CanExecute(skillSlot);
        }

        //handled in the tracking component
        //private static bool IsTargetCooldown([NotNull] GenericSkill skillSlot) {

        //    TeslaTrackerComponentDash teslaTracker = ((InstanceData)skillSlot.skillInstanceData).teslaTracker;
        //    HurtBox trackingTarget = teslaTracker?.GetTrackingTarget();

        //    return trackingTarget != null && trackingTarget.healthComponent.body.HasBuff(Modules.Buffs.blinkCooldownBuff)
        //}

        public override void OnExecute([NotNull] GenericSkill skillSlot)
        {
            base.OnExecute(skillSlot);
            InstanceData instanceData = (InstanceData)skillSlot.skillInstanceData;

            //if (!IsTargetCooldown(skillSlot)) {
            skillSlot.stock++;
            instanceData.hasExtraStock = true;
            instanceData.timeoutTimer = timeoutDuration;
            //} else {
            //    instanceData.hasExtraStock = false;
            //}
        }

        // Token: 0x060045D6 RID: 17878 RVA: 0x00121E18 File Offset: 0x00120018
        public override void OnFixedUpdate([NotNull] GenericSkill skillSlot, float deltaTime)
        {
            base.OnFixedUpdate(skillSlot, deltaTime);
            InstanceData instanceData = (InstanceData)skillSlot.skillInstanceData;

            ((InstanceData)skillSlot.skillInstanceData).teslaTracker.SetIsReady(IsReady(skillSlot));

            instanceData.timeoutTimer -= deltaTime;
            if (instanceData.timeoutTimer <= 0f && instanceData.hasExtraStock)
            {
                skillSlot.stock--;
                instanceData.hasExtraStock = false;
            }
        }

        public override Sprite GetCurrentIcon([NotNull] GenericSkill skillSlot)
        {
            return ((InstanceData)skillSlot.skillInstanceData).hasExtraStock ? refreshedIcon : icon;
        }
    }
}
