using EntityStates;
using ModdedEntityStates.BaseStates;
using Modules.Survivors;
using UnityEngine;
using UnityEngine.Networking;

namespace ModdedEntityStates.Desolator {
    public class DeployEnter : BaseTimedSkillState {

        #region gameplay Values
        public static float BaseDuration = 0.3f;
        public static float StartTime = 1f;
        #endregion

        public RoR2.CameraTargetParams.AimRequest aimRequest;
        private bool _complete;

        public override void OnEnter() {
            base.OnEnter();

            InitDurationValues(BaseDuration, StartTime);

            aimRequest = cameraTargetParams.RequestAimType(RoR2.CameraTargetParams.AimType.Aura);

            PlayCrossfade("FullBody, Override", "Deploy", "Deploy.playbackRate", duration, 0.05f);

            //addbuff, something

            if (NetworkServer.active) {
                characterBody.AddTimedBuff(RoR2.RoR2Content.Buffs.HiddenInvincibility, BaseDuration);
            }

            skillLocator.special.SetSkillOverride(gameObject, DesolatorSurvivor.cancelDeploySkillDef, RoR2.GenericSkill.SkillOverridePriority.Contextual);
        }

        public override void FixedUpdate() {
            base.FixedUpdate();

            if (base.characterMotor) {
                base.characterMotor.moveDirection = Vector3.zero;
            }
        }

        protected override EntityState ChooseNextState() {
            _complete = true;
            return new DeployIrradiate { aimRequest = this.aimRequest };
        }

        public override void OnExit() {
            base.OnExit();
            if (!_complete) {
                aimRequest.Dispose();

                skillLocator.special.UnsetSkillOverride(gameObject, DesolatorSurvivor.cancelDeploySkillDef, RoR2.GenericSkill.SkillOverridePriority.Contextual);
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Frozen;
        }
    }
}
