using EntityStates;
using ModdedEntityStates.BaseStates;
using Modules.Survivors;

namespace ModdedEntityStates.Desolator {
    public class DeployCancel : BaseState {

        public static float BaseDuration = 0.3f;
        public static float StartTime = 1f;

        public override void OnEnter() {
            base.OnEnter();

            //todo deso anim legs
            PlayCrossfade("FullBody, Override", "UnDeploy", "Deploy.playbackRate", 0.3f, 0.05f);

            PlayCrossfade("RadCannonBar", "DesolatorIdlePose", 0.1f);
            PlayCrossfade("RadCannonSpin", "DesolatorIdlePose", 0.1f);

            skillLocator.special.UnsetSkillOverride(gameObject, DesolatorSurvivor.cancelDeploySkillDef, RoR2.GenericSkill.SkillOverridePriority.Contextual);

            base.outer.SetNextStateToMain();
        }

        //public override void FixedUpdate() {
        //    base.FixedUpdate();

        //    if (base.isAuthority && base.inputBank.sprint.down) {
        //        base.characterBody.isSprinting = true;
        //    }
        //}

        //public override void OnExit() {
        //    base.outer.SetNextStateToMain();
        //}
    }
}
