using EntityStates;
using RA2Mod.Modules.BaseStates;
using RA2Mod.Survivors.Chrono.Components;
using RoR2;
using UnityEngine;

namespace RA2Mod.Survivors.Chrono.SkillStates {

    public class ChronoSprintState : BaseSkillState
    {
        private ChronoProjectionMotor marker;

        private bool inCamera;

        private Transform origPivot;

        public override void OnEnter()
        {
            base.OnEnter();

            marker = Object.Instantiate(ChronoAssets.markerPrefab, modelLocator.modelBaseTransform.position, transform.rotation, null);

            //foreach (CameraRigController cameraRigController in CameraRigController.readOnlyInstancesList)
            //{
            //    if (cameraRigController.target == gameObject)
            //    {
            //        cameraRigController.SetOverrideCam(marker, 0f);
            //    }
            //}
            origPivot = cameraTargetParams.cameraPivotTransform;
            cameraTargetParams.cameraPivotTransform = marker.cameraPivot;
            cameraTargetParams.dontRaycastToPivot = true;
            inCamera = true;
            
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
                state.windDownTime = Vector3.Distance(marker.viewPosition, transform.position) / (6 * Mathf.Max(characterBody.moveSpeed, 0.5f));
                
                StopCamera();

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

            //foreach (CameraRigController cameraRigController in CameraRigController.readOnlyInstancesList)
            //{
            //    if (cameraRigController.target == gameObject)
            //    {
            //        cameraRigController.SetOverrideCam(null, 0.5f);
            //    }
            //}
            cameraTargetParams.cameraPivotTransform = origPivot;
            cameraTargetParams.dontRaycastToPivot = false;
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