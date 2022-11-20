using EntityStates;
using ModdedEntityStates.BaseStates;
using Modules.Survivors;
using RoR2;

namespace ModdedEntityStates.Desolator {

    public class DeployCancel : BaseTimedSkillState {

        public override void OnEnter() {
            base.OnEnter();

            //todo deso anim legs
            PlayCrossfade("FullBody, Override", "UnDeploy", "Deploy.playbackRate", 0.3f, 0.05f);

            PlayCrossfade("RadCannonBar", "DesolatorIdlePose", 0.1f);
            PlayCrossfade("RadCannonSpin", "DesolatorIdlePose", 0.1f);

            base.outer.SetNextStateToMain();

            skillLocator.special.UnsetSkillOverride(gameObject, DesolatorSurvivor.cancelDeploySkillDef, RoR2.GenericSkill.SkillOverridePriority.Contextual);

        }

        //public override void FixedUpdate() {
        //    base.FixedUpdate();

        //    if (base.isAuthority && base.inputBank.sprint.down) {
        //        base.characterBody.isSprinting = true;
        //    }

        //}
    }
}
