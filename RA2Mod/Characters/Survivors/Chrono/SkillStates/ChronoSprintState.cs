using EntityStates;
using RA2Mod.Modules.BaseStates;
using RA2Mod.Survivors.Chrono.Components;
using RA2Mod.Survivors.Chrono.SkillDefs;
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

        public PhaseIndicatorController componentFromSkillDef { get; set; }

        public override void OnEnter()
        {
            base.OnEnter();

            marker = Object.Instantiate(ChronoAssets.markerPrefab, modelLocator.modelBaseTransform.position, transform.rotation, null);

            origPivot = cameraTargetParams.cameraPivotTransform;
            cameraTargetParams.cameraPivotTransform = marker.cameraPivot;
            cameraTargetParams.dontRaycastToPivot = true;
            characterBody.aimOriginTransform = marker.cameraPivot;
            inCamera = true;
            if (NetworkServer.active)
            {
                characterBody.AddTimedBuff(RoR2Content.Buffs.ArmorBoost, 0.5f);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            marker.SimpleMove(inputBank.moveVector + (inputBank.jump.down ? Vector3.up : inputBank.skill3.down ? Vector3.down : Vector3.zero));

            base.characterBody.isSprinting = true;
            if (GetSprintReleased())
            {
                PhaseState state = new PhaseState();
                float dist = Vector3.Distance(marker.viewPosition, transform.position) / Mathf.Max(characterBody.moveSpeed, 0.3f) * ChronoConfig.M0SprintBlinkTimeMulti.Value;
                state.windDownTime = 0.01f * dist * dist + 1 * dist; //fucking around in desmos

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
            PlayerCharacterMasterController playerMaster;
            if (playerMaster = characterBody.master.playerCharacterMasterController)
            {
                LocalUser localUser;
                Player player;
                CameraRigController cameraRigController;
                if (PlayerCharacterMasterController.CanSendBodyInput(playerMaster.networkUser, out localUser, out player, out cameraRigController))
                {
                    return !player.GetButton(18);
                }
            }

            return !inputBank.sprint.down;
        }

        public override void OnExit()
        {
            base.OnExit();

            StopCamera();

            UnityEngine.Object.DestroyImmediate(marker.gameObject);
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