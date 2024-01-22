using EntityStates;
using RA2Mod.Modules.BaseStates;
using RA2Mod.Survivors.Chrono.Components;
using RA2Mod.Survivors.Chrono.SkillDefs;
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
            
            //marker.transform.position = transform.position;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            marker.SimpleMove(inputBank.moveVector + (inputBank.jump.down ? Vector3.up : inputBank.skill3.down ? Vector3.down : Vector3.zero));

            base.characterBody.isSprinting = true;
            
            if (!inputBank.sprint.down)
            {
                PhaseState state = new PhaseState();
                float dist = Vector3.Distance(marker.viewPosition, transform.position)/Mathf.Max(characterBody.moveSpeed, 0.3f) * ChronoConfig.M0SprintBlinkTimeMulti.Value;
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

    //meh
    public class ChronoSprintState2 : GenericCharacterMain {
        private ICharacterGravityParameterProvider characterGravityParameterProvider;
        private ICharacterFlightParameterProvider characterFlightParameterProvider;

        public override void OnEnter() {
            base.OnEnter();
            modelLocator.autoUpdateModelTransform = false;

            this.characterGravityParameterProvider = base.gameObject.GetComponent<ICharacterGravityParameterProvider>();
            this.characterFlightParameterProvider = base.gameObject.GetComponent<ICharacterFlightParameterProvider>();

            if (this.characterFlightParameterProvider != null) {
                CharacterFlightParameters flightParameters = this.characterFlightParameterProvider.flightParameters;
                flightParameters.channeledFlightGranterCount++;
                this.characterFlightParameterProvider.flightParameters = flightParameters;
            }
            if (this.characterGravityParameterProvider != null) {
                CharacterGravityParameters gravityParameters = this.characterGravityParameterProvider.gravityParameters;
                gravityParameters.channeledAntiGravityGranterCount++;
                this.characterGravityParameterProvider.gravityParameters = gravityParameters;
            }
        }

        public override void FixedUpdate() {
            base.FixedUpdate();

            //marker.SimpleMove(inputBank.moveVector + (inputBank.jump.down ? Vector3.up : inputBank.skill3.down ? Vector3.down : Vector3.zero));

            base.characterBody.isSprinting = true;

            if (!inputBank.sprint.down) {
                //characterMotor.Motor.SetPosition(marker.viewPosition);
                characterBody.isSprinting = false;
                outer.SetNextStateToMain();
            }
        }

        public override void HandleMovements() {
            base.HandleMovements();
        }

        public override void OnExit() {
            base.OnExit();

            modelLocator.autoUpdateModelTransform = true;

            if (this.characterFlightParameterProvider != null) {
                CharacterFlightParameters flightParameters = this.characterFlightParameterProvider.flightParameters;
                flightParameters.channeledFlightGranterCount--;
                this.characterFlightParameterProvider.flightParameters = flightParameters;
            }
            if (this.characterGravityParameterProvider != null) {
                CharacterGravityParameters gravityParameters = this.characterGravityParameterProvider.gravityParameters;
                gravityParameters.channeledAntiGravityGranterCount--;
                this.characterGravityParameterProvider.gravityParameters = gravityParameters;
            }
        }
    }
}