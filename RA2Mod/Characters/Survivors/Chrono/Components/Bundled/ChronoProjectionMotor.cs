using KinematicCharacterController;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace RA2Mod.Survivors.Chrono.Components
{
    public class ChronoProjectionMotor : BaseCharacterController {

        [SerializeField]
        public Transform cameraPivot;

        [SerializeField]
        private Transform view;
        public Vector3 viewPosition => lastPosition + Vector3.up * 1;

        private Vector3 lastPosition;
        private Vector3 lastLastPosition;

        private float inverseFixedDeltaTime;

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
            if(deltaPosition.y > 0)
            {
                Motor.ForceUnground();
            }
        }
        #endregion motor

        #region projection preview
        void Awake()
        {
            inverseFixedDeltaTime = 1 / Time.fixedDeltaTime;

            lastLastPosition = transform.position;
            lastPosition = transform.position;
        }

        void FixedUpdate()
        {
            lastLastPosition = lastPosition;
            Vector3 foundPosition = Find();
            if (foundPosition != default(Vector3))
            {
                lastPosition = foundPosition;
            }
            view.transform.position = lastPosition;
        }

        public Vector3 Find() 
        {
            RaycastHit hit;
            if (Physics.SphereCast(cameraPivot.position, 1, Vector3.down, out hit, 300f, LayerIndex.world.mask)) {
                return new Vector3(transform.position.x, hit.point.y, transform.position.z);
            }
            return default(Vector3);
        }
        
        void Update()
        {
            view.transform.position = Vector3.Lerp(lastLastPosition, lastPosition, (Run.TimeStamp.now - Run.FixedTimeStamp.now) * inverseFixedDeltaTime);
        }
        #endregion projection preview
    }
}