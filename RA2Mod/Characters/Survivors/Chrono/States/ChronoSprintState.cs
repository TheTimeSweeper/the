using EntityStates;
using RA2Mod.General.SkillDefs;
using RA2Mod.Modules;
using RA2Mod.Modules.BaseStates;
using RA2Mod.Survivors.Chrono.Components;
using Rewired;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RA2Mod.Survivors.Chrono.SkillStates
{
    public class ChronoSprintState : BaseSkillState, IHasSkillDefComponent<PhaseIndicatorController>
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

        public PhaseIndicatorController componentFromSkillDef { get; set; }

        private InteractionDriver interactor;

        public override void OnEnter()
        {
            base.OnEnter();

            marker = Object.Instantiate(ChronoAssets.markerPrefab, modelLocator.modelBaseTransform.position, transform.rotation, null);

            origPivot = cameraTargetParams.cameraPivotTransform;
            cameraTargetParams.cameraPivotTransform = marker.cameraPivot;
            cameraTargetParams.dontRaycastToPivot = true;
            origOrigin = characterBody.aimOriginTransform;
            characterBody.aimOriginTransform = marker.cameraPivot;
            inCamera = true;

            if (NetworkServer.active)
            {
                characterBody.AddTimedBuff(RoR2Content.Buffs.ArmorBoost, 0.5f);
            }
            if(gameObject.TryGetComponent(out interactor))
            {
                interactor.enabled = false;                                                                              
            }

            cameraOverride = CameraParams.OverrideCameraParams(base.cameraTargetParams, ChronoCameraParams.sprintCamera, ChronoConfig.M0CameraLerpTime.Value);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!isAuthority)
                return;

            timeSpent += Time.fixedDeltaTime;

            Vector3 moveVector = inputBank.moveVector * (Mathf.Min(moveSpeedStat, 20) * 0.1f);
            moveVector.y = inputBank.jump.down && (marker.transform.position.y - transform.position.y < heightLimit) ? 1 : inputBank.skill3.down ? -1 : 0;

            marker.SimpleMove(moveVector);


            base.characterBody.isSprinting = true;
            if (GetSprintReleased())
            {
                PhaseState state = new PhaseState();
                float dist = Vector3.Distance(marker.viewPosition, transform.position) / Mathf.Max(characterBody.moveSpeed, 0.3f) * distMultiplier;
                float distTime = 0.01f * dist * dist + 1 * dist; //fucking around in desmos
                float timeTime = timeSpent * timeSpentMultiplier;
                state.windDownTime = Mathf.Max(distTime, timeTime);

                //Log.Warning($"dist {Vector3.Distance(marker.viewPosition, transform.position)} time {state.windDownTime}");
                state.controller = componentFromSkillDef;
                StopCamera();
                
                Util.PlaySound("Play_ChronoMove", gameObject);
                characterMotor.Motor.SetPosition(marker.viewPosition);
                characterBody.isSprinting = false;
                
                outer.SetNextState(state);
            }
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
                    if (PlayerCharacterMasterController.CanSendBodyInput(playerMaster.networkUser, out _, out player, out _))
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

            UnityEngine.Object.Destroy(marker.gameObject);

            cameraTargetParams.RemoveParamsOverride(cameraOverride, ChronoConfig.M0CameraLerpTime.Value);

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