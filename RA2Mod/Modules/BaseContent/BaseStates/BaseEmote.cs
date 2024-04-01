using RoR2;
using UnityEngine;
using System;
using static RoR2.CameraTargetParams;
using EntityStates;
using BepInEx.Configuration;
using RA2Mod.General;
using RA2Mod.General.States;

namespace RA2Mod.Modules.BaseStates
{
    public class BaseEmote : BaseState
    {
        public string soundString;
        public string animString;
        public float duration;
        //public float animDuration;

        private bool hasExploded;
        private uint activePlayID;
        private Animator animator;
        private ChildLocator childLocator;

        public LocalUser localUser;

        private CharacterCameraParamsData emoteCameraParams = new CharacterCameraParamsData()
        {
            maxPitch = 70,
            minPitch = -70,
            pivotVerticalOffset = 0.2f,
            idealLocalCameraPos = emoteCameraPosition,
            wallCushion = 0.1f,
        };

        public static Vector3 emoteCameraPosition = new Vector3(0, -0.8f, -10.2f);

        private CameraParamsOverrideHandle camOverrideHandle;

        public override void OnEnter()
        {
            base.OnEnter();
            animator = GetModelAnimator();
            childLocator = GetModelChildLocator();
            FindLocalUser();

            characterBody.hideCrosshair = true;

            if (GetAimAnimator()) GetAimAnimator().enabled = false;
            animator.SetLayerWeight(animator.GetLayerIndex("AimPitch"), 0);
            animator.SetLayerWeight(animator.GetLayerIndex("AimYaw"), 0);

            //if (this.animDuration == 0 && this.duration != 0) this.animDuration = this.duration;

            //if (this.duration > 0) base.PlayAnimation("FullBody, Override", this.animString, "Emote.playbackRate", this.duration);
            //else base.PlayAnimation("FullBody, Override", this.animString, "Emote.playbackRate", this.animDuration);

            base.PlayAnimation("FullBody, Override", animString);

            activePlayID = Util.PlaySound(soundString, gameObject);

            CameraParamsOverrideRequest request = new CameraParamsOverrideRequest
            {
                cameraParamsData = emoteCameraParams,
                priority = 0,
            };

            camOverrideHandle = cameraTargetParams.AddParamsOverride(request, 0.5f);

        }

        public override void OnExit()
        {
            base.OnExit();

            characterBody.hideCrosshair = false;

            if (GetAimAnimator()) GetAimAnimator().enabled = true;
            animator.SetLayerWeight(animator.GetLayerIndex("AimPitch"), 1);
            animator.SetLayerWeight(animator.GetLayerIndex("AimYaw"), 1);

            base.PlayAnimation("FullBody, Override", "BufferEmpty");
            if (activePlayID != 0) AkSoundEngine.StopPlayingID(activePlayID);

            cameraTargetParams.RemoveParamsOverride(camOverrideHandle, 0.5f);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            bool flag = false;

            if (characterMotor)
            {
                if (!characterMotor.isGrounded) flag = true;
                if (characterMotor.velocity != Vector3.zero) flag = true;
            }

            if (inputBank)
            {
                if (inputBank.skill1.down) flag = true;
                if (inputBank.skill2.down) flag = true;
                if (inputBank.skill3.down) flag = true;
                if (inputBank.skill4.down) flag = true;
                if (inputBank.jump.down) flag = true;

                if (inputBank.moveVector != Vector3.zero) flag = true;
            }

            //emote cancels
            if (isAuthority && characterMotor.isGrounded)
            {
                //todo separate checking from the state
                CheckEmote<Rest>(GeneralConfig.RestKeybind.Value);
            }


            if (duration > 0 && fixedAge >= duration) flag = true;

            if (flag)
            {
                outer.SetNextStateToMain();
            }
        }

        private void CheckEmote(KeyCode keybind, EntityState state)
        {
            if (Input.GetKeyDown(keybind))
            {
                if (!localUser.isUIFocused)
                {
                    outer.SetInterruptState(state, InterruptPriority.Any);
                }
            }
        }

        private bool CheckEmote<T>(KeyboardShortcut keybind) where T : EntityState, new()
        {
            if (Config.GetKeyPressed(keybind))
            {

                FindLocalUser();

                if (localUser != null && !localUser.isUIFocused)
                {
                    outer.SetInterruptState(new T(), InterruptPriority.Any);
                    return true;
                }
            }
            return false;
        }

        private void FindLocalUser()
        {
            if (localUser == null)
            {
                if (characterBody)
                {
                    foreach (LocalUser lu in LocalUserManager.readOnlyLocalUsersList)
                    {
                        if (lu.cachedBody == characterBody)
                        {
                            localUser = lu;
                            break;
                        }
                    }
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Any;
        }
    }
}
