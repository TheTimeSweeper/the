using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using RoR2;

namespace RA2Mod.Survivors.Chrono.Components {

    public class ChronoProjectionMotor : BaseCharacterController, ICameraStateProvider {

        [SerializeField]
        private Transform cameraPivot;

        [SerializeField]
        private Transform positionProjection;

        [SerializeField]
        private Animator projectionAnimator;

        [SerializeField]
        private Transform truePositionView;

        [SerializeField]
        private Transform heightBeam;

        #region motor
        public override void AfterCharacterUpdate(float deltaTime) { }
        public override void BeforeCharacterUpdate(float deltaTime) { }

        public override bool IsColliderValidForCollisions(Collider coll) {
            return !coll.isTrigger && coll != base.Motor.Capsule;
        }

        public override void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport) { }

        public override void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport) { }

        public override void PostGroundingUpdate(float deltaTime) { }

        public override void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport) { }

        public override void UpdateRotation(ref Quaternion currentRotation, float deltaTime) { }

        public override void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) { }

        public void SimpleMove(Vector3 deltaPosition) {
            Motor.MoveCharacter(transform.position + deltaPosition);
        }
        #endregion motor

        #region camera
        public void GetCameraState(CameraRigController cameraRigController, ref CameraState cameraState) {
            cameraState.position = cameraPivot.position;
        }

        public bool IsHudAllowed(CameraRigController cameraRigController) {
            return true;
        }

        public bool IsUserControlAllowed(CameraRigController cameraRigController) {
            return true;
        }

        public bool IsUserLookAllowed(CameraRigController cameraRigController) {
            return true;
        }
        #endregion camera
    }
}