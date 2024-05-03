﻿using RA2Mod.Modules.BaseStates;

namespace RA2Mod.Survivors.Desolator.States
{
    public class DeployCancel : BaseTimedSkillState {

        public override float TimedBaseDuration => 0;

        public override float TimedBaseCastStartPercentTime => 0;

        public override void OnEnter() {
            base.OnEnter();

            //todo deso anim legs
            PlayCrossfade("FullBody, Override", "UnDeploy", "Deploy.playbackRate", 0.3f, 0.05f);

            PlayCrossfade("RadCannonBar", "DesolatorIdlePose", 0.1f);
            PlayCrossfade("RadCannonSpin", "DesolatorIdlePose", 0.1f);

            base.outer.SetNextStateToMain();
            
            if (activatorSkillSlot.skillDef == DesolatorSurvivor.cancelDeploySkillDef) {
                activatorSkillSlot.UnsetSkillOverride(gameObject, DesolatorSurvivor.cancelDeploySkillDef, RoR2.GenericSkill.SkillOverridePriority.Contextual);
            }
        }

        //public override void FixedUpdate() {
        //    base.FixedUpdate();

        //    if (base.isAuthority && base.inputBank.sprint.down) {
        //        base.characterBody.isSprinting = true;
        //    }

        //}
    }
}