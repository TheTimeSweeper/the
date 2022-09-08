using ModdedEntityStates.BaseStates;
using Modules.Survivors;

namespace ModdedEntityStates.Desolator {
    public class DeployCancel : BaseTimedSkillState {

        public static float BaseDuration = 0.3f;
        public static float StartTime = 1f;

        public override void OnEnter() {
            base.OnEnter();

            InitDurationValues(BaseDuration, StartTime);

            PlayCrossfade("FullBody, Override", "UnDeploy", "Deploy.playbackRate", duration, 0.05f);

            skillLocator.special.UnsetSkillOverride(gameObject, DesolatorSurvivor.cancelDeploySkillDef, RoR2.GenericSkill.SkillOverridePriority.Contextual);

        }

        protected override void OnCastEnter() {
            base.outer.SetNextStateToMain();
        }
    }
}
