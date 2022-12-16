using RoR2;
using UnityEngine;
using System;
using static RoR2.CameraTargetParams;
using EntityStates;

namespace ModdedEntityStates.TeslaTrooper {

    public class BaseEmote : BaseState {
        public string soundString;
        public string animString;
        public float duration;
        //public float animDuration;

        private bool hasExploded;
        private uint activePlayID;
        private Animator animator;
        private ChildLocator childLocator;

        public LocalUser localUser;

        private CharacterCameraParamsData emoteCameraParams = new CharacterCameraParamsData() {
            maxPitch = 70,
            minPitch = -70,
            pivotVerticalOffset = 1f,
            idealLocalCameraPos = emoteCameraPosition,
            wallCushion = 0.1f,
        };

        public static Vector3 emoteCameraPosition = new Vector3(0, -1.0f, -7.9f);

        private CameraParamsOverrideHandle camOverrideHandle;

        public override void OnEnter() {
            base.OnEnter();
            this.animator = base.GetModelAnimator();
            this.childLocator = base.GetModelChildLocator();
            this.localUser = LocalUserManager.readOnlyLocalUsersList[0];

            base.characterBody.hideCrosshair = true;

            if (base.GetAimAnimator()) base.GetAimAnimator().enabled = false;
            this.animator.SetLayerWeight(animator.GetLayerIndex("AimPitch"), 0);
            this.animator.SetLayerWeight(animator.GetLayerIndex("AimYaw"), 0);

            //if (this.animDuration == 0 && this.duration != 0) this.animDuration = this.duration;

            //if (this.duration > 0) base.PlayAnimation("FullBody, Override", this.animString, "Emote.playbackRate", this.duration);
            //else base.PlayAnimation("FullBody, Override", this.animString, "Emote.playbackRate", this.animDuration);

            base.PlayAnimation("FullBody, Override", this.animString);

            this.activePlayID = Util.PlaySound(soundString, base.gameObject);

            CameraParamsOverrideRequest request = new CameraParamsOverrideRequest {
                cameraParamsData = emoteCameraParams,
                priority = 0,
            };

            camOverrideHandle = base.cameraTargetParams.AddParamsOverride(request, 0.5f);

        }

        public override void OnExit() {
            base.OnExit();

            base.characterBody.hideCrosshair = false;

            if (base.GetAimAnimator()) base.GetAimAnimator().enabled = true;
            this.animator.SetLayerWeight(animator.GetLayerIndex("AimPitch"), 1);
            this.animator.SetLayerWeight(animator.GetLayerIndex("AimYaw"), 1);

            base.PlayAnimation("FullBody, Override", "BufferEmpty");
            if (this.activePlayID != 0) AkSoundEngine.StopPlayingID(this.activePlayID);

            base.cameraTargetParams.RemoveParamsOverride(camOverrideHandle, 0.5f);
        }

        public override void FixedUpdate() {
            base.FixedUpdate();

            bool flag = false;

            if (base.characterMotor) {
                if (!base.characterMotor.isGrounded) flag = true;
                if (base.characterMotor.velocity != Vector3.zero) flag = true;
            }

            if (base.inputBank) {
                if (base.inputBank.skill1.down) flag = true;
                if (base.inputBank.skill2.down) flag = true;
                if (base.inputBank.skill3.down) flag = true;
                if (base.inputBank.skill4.down) flag = true;
                if (base.inputBank.jump.down) flag = true;

                if (base.inputBank.moveVector != Vector3.zero) flag = true;
            }

            //emote cancels
            if (base.isAuthority && base.characterMotor.isGrounded) {

                CheckEmote(Modules.Config.restKeybind.Value, new Rest());
            }

            if (this.duration > 0 && base.fixedAge >= this.duration) flag = true;
            
            if (flag) {
                this.outer.SetNextStateToMain();
            }
        }

        private void CheckEmote(KeyCode keybind, EntityState state) {
            if (Input.GetKeyDown(keybind)) {
                if (!localUser.isUIFocused) {
                    outer.SetInterruptState(state, InterruptPriority.Any);
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Any;
        }
    }

    public class Rest : BaseEmote {
        public override void OnEnter() {
            this.animString = "Sit";
            this.duration = 0;
            base.OnEnter();
            PlayAnimation("RadCannonSpin", "CannonSpin");
        }

        public override void OnExit() {
            base.OnExit();
            PlayAnimation("RadCannonSpin", "DesolatorIdlePose");
        }
    }
}
