using EntityStates;
using RA2Mod.Modules.BaseStates;
using UnityEngine;
using UnityEngine.Networking;

namespace RA2Mod.Survivors.Desolator.States
{

    public class DeployEnter : BaseTimedSkillState {

        #region gameplay Values
        public static float BaseDuration = 0.3f;
        public static float StartTime = 1f;
        #endregion

        public RoR2.CameraTargetParams.AimRequest aimRequest;
        protected bool _complete;

        public override float TimedBaseDuration => BaseDuration;

        public override float TimedBaseCastStartPercentTime => StartTime;

        public override void OnEnter()
        {
            base.OnEnter();

            aimRequest = cameraTargetParams.RequestAimType(RoR2.CameraTargetParams.AimType.Aura);

            PlayCrossfade("Gesture, Override", "BufferEmpty", 0.05f);
            PlayCrossfade("FullBody, Override", "DesolatorDeploy", "Deploy.playbackRate", duration, 0.05f);

            Animator animator = GetModelAnimator();
            PlayCannonAnimations(animator);

            animator.SetFloat("aimYawCycle", 0.5f);
            animator.SetFloat("aimPitchCycle", 0.5f);
            
            if (NetworkServer.active)
            {
                characterBody.AddTimedBuff(RoR2.RoR2Content.Buffs.HiddenInvincibility, BaseDuration);
            }
        }

        protected virtual void PlayCannonAnimations(Animator animator)
        {
            animator.SetFloat("CannonBarCharge", 1);
            PlayAnimation("RadCannonBar", "CannonCharge");

            animator.SetFloat("CannonSpin", 0.99f);
            PlayCrossfade("RadCannonSpin", "CannonSpin", 0.1f);
        }

        protected override void SetNextState()
        {
            _complete = true;
            var state = new DeployIrradiate { aimRequest = this.aimRequest, fromEnter = true, activatorSkillSlot = activatorSkillSlot };
            outer.SetNextState(state);
        }

        public override void OnExit()
        {
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
