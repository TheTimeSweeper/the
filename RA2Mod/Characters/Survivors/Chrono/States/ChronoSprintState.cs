using EntityStates;
using RA2Mod.General.SkillDefs;
using RA2Mod.Modules;
using RA2Mod.Modules.BaseStates;
using RA2Mod.Survivors.Chrono.Components;
using Rewired;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RA2Mod.Survivors.Chrono.States
{
    public class ChronoSprintState : BaseSkillState, IHasSkillDefComponent<PhaseIndicatorController, ChronoSprintProjectionSpawner, InteractionDriver>
    {
        private ChronoProjectionMotor marker;

        private bool inCamera;

        private Transform origPivot;
        private Transform origOrigin;
        private CameraTargetParams.CameraParamsOverrideHandle cameraOverride;
        private float heightLimit = 50;
        private float timeSpent = -1;
        protected virtual float timeSpentMultiplier => ChronoConfig.M0_SprintTeleport_TimeTimeMulti.Value;
        protected virtual float distMultiplier => ChronoConfig.M0_SprintTeleport_DistTimeMulti.Value;

        public PhaseIndicatorController componentFromSkillDef1 { get; set; }
        private PhaseIndicatorController phaseIndicator => componentFromSkillDef1;
        public ChronoSprintProjectionSpawner componentFromSkillDef2 { get; set; }
        private ChronoSprintProjectionSpawner projectionSpawner => componentFromSkillDef2;
        public InteractionDriver componentFromSkillDef3 { get; set; }
        private InteractionDriver interactor => componentFromSkillDef3;

        private bool _hasInputBank;
        private int _fixedFrames;

        public override void OnEnter()
        {
            base.OnEnter();

            heightLimit = characterBody.jumpPower * characterBody.maxJumpCount * ChronoConfig.M0_JumpMultiplier.Value;
            if (characterBody.inventory)
            {
                heightLimit += characterBody.inventory.GetItemCount(RoR2Content.Items.JumpBoost) * characterBody.jumpPower * ChronoConfig.M0_JumpMultiplier.Value;
            }

            _hasInputBank = inputBank != null;
            //well turns out the skilldef method only sets on authority
            if(projectionSpawner == null)
            {
                componentFromSkillDef2 = GetComponent<ChronoSprintProjectionSpawner>();
            }
            if (NetworkServer.active && projectionSpawner != null)
            {
                projectionSpawner.SpawnProjectionServer(modelLocator.modelBaseTransform.position, transform.rotation);
                characterBody.AddTimedBuff(RoR2Content.Buffs.ArmorBoost, 0.5f);
            }
            if (interactor != null)
            {
                interactor.enabled = false;
            }

            cameraOverride = CameraParams.OverrideCameraParams(base.cameraTargetParams, ChronoCameraParams.sprintCamera, 0.6f);
        }

        private void InitCameraToMarker()
        {            
            origPivot = cameraTargetParams.cameraPivotTransform;
            cameraTargetParams.cameraPivotTransform = marker.cameraPivot;
            cameraTargetParams.dontRaycastToPivot = true;
            origOrigin = characterBody.aimOriginTransform;
            characterBody.aimOriginTransform = marker.cameraPivot;
            inCamera = true;

            if (!isAuthority)
                marker.InitNonAuthority();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            base.characterBody.isSprinting = true;

            marker = projectionSpawner.marker;

            if (marker == null)
                return;

            if (_fixedFrames > 0)//wait a frame
            {
                InitCameraToMarker();
            }
            
            timeSpent += Time.deltaTime;

            if (_hasInputBank)
            {
                Vector3 moveDeltaVector = inputBank.moveVector * Mathf.Clamp(moveSpeedStat, 0, 10) * ChronoConfig.M0_SprintTeleport_ProjectionSpeed.Value;

                if (inputBank.jump.down)
                {
                    if (marker.transform.position.y - transform.position.y < heightLimit)
                    {
                        moveDeltaVector.y = 100;
                    }
                }
                else if(inputBank.skill3.down)
                {
                    moveDeltaVector.y = -100;
                }

                if (isAuthority && _fixedFrames > 0)//wait a frame
                {
                  projectionSpawner.MoveMarkerAuthority(moveDeltaVector * Time.deltaTime);
                }
                marker.UpdateAim(GetAimRay().direction);
            }
            marker.UpdateHeightBeam(transform.position.y, heightLimit);

            if (GetSprintReleased() && isAuthority)
            {
                PhaseState state = new PhaseState();
                float dist = Vector3.Distance(marker.viewPosition, transform.position) / Mathf.Max(characterBody.moveSpeed, 0.3f) * distMultiplier;
                float distTime = 0.01f * dist * dist + 1 * dist; //fucking around in desmos
                float timeTime = timeSpent * timeSpentMultiplier;
                state.windDownTime = Mathf.Max(distTime, timeTime);

                //Log.Warning($"dist {Vector3.Distance(marker.viewPosition, transform.position)} time {state.windDownTime}");
                state.controller = phaseIndicator;
                StopCamera();
                
                characterMotor.Motor.SetPosition(marker.viewPosition);
                
                outer.SetNextState(state);
            }
            _fixedFrames += 1;
        }

        private bool GetSprintReleased()
        {
            if (ChronoConfig.M0_SprintTeleport_OnRelease.Value || ChronoCompat.AutoSprintInstalled)
            {
                PlayerCharacterMasterController playerMaster;
                if (playerMaster = characterBody.master.playerCharacterMasterController)
                {
                    //LocalUser localUser;
                    Player player;
                    //CameraRigController cameraRigController;

                    if (PlayerCharacterMasterController.CanSendBodyInput(playerMaster.networkUser, out _, out player, out _, out _))
                    {
                        return !player.GetButton(18);
                    }
                }
            }

            return !inputBank.sprint.down;
        }

        public override void OnExit()
        {
            base.OnExit();
            
            StopCamera();
            if (NetworkServer.active)
            {
                projectionSpawner.DisposeMarkerServer();
            }
            cameraTargetParams.RemoveParamsOverride(cameraOverride, 0.6f);

            if (interactor)
            {
                interactor.enabled = true;
            }
        }

        private void StopCamera()
        {
            if (!inCamera)
                return;

            cameraTargetParams.cameraPivotTransform = origPivot;
            cameraTargetParams.dontRaycastToPivot = false;
            characterBody.aimOriginTransform = origOrigin;

            inCamera = false;
        }
    }
}