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
        protected bool _complete;

        public override void OnEnter() {
            base.OnEnter();

            InitDurationValues(BaseDuration, StartTime);

            aimRequest = cameraTargetParams.RequestAimType(RoR2.CameraTargetParams.AimType.Aura);

            PlayCrossfade("FullBody, Override", "Deploy", "Deploy.playbackRate", duration, 0.05f);

            Animator animator = GetModelAnimator();
            animator.SetFloat("CannonBarCharge", 1);
            PlayAnimation("RadCannonBar", "CannonCharge");

            animator.SetFloat("CannonSpin", 0.99f);
            PlayCrossfade("RadCannonSpin", "CannonSpin", 0.1f);

            animator.SetFloat("aimYawCycle", 0.5f);
            animator.SetFloat("aimPitchCycle", 0.5f);

            if (NetworkServer.active) {
                characterBody.AddTimedBuff(RoR2.RoR2Content.Buffs.HiddenInvincibility, BaseDuration);
            }
        }

        protected override EntityState ChooseNextState() {
            _complete = true;
            return new DeployIrradiate { aimRequest = this.aimRequest, fromEnter = true };
        }
        
        public override void OnExit() {
            base.OnExit();

            if (_complete)
                return;
            
            aimRequest.Dispose();


            PlayCrossfade("RadCannonBar", "DesolatorIdlePose", 0.1f);
            PlayCrossfade("RadCannonSpin", "DesolatorIdlePose", 0.1f);
        }
        
        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Frozen;
        }
    }
}
