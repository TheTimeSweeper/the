using KinematicCharacterController;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace RA2Mod.Survivors.Chrono.Components
{
    public class ChronoProjectionMotor : BaseCharacterController {

        [SerializeField]
        public Transform cameraPivot;

        [SerializeField]
        private Transform positionProjection;
        public Vector3 viewPosition => lastPosition + Vector3.up * 1;

        [SerializeField]
        private Animator projectionAnimator;

        [SerializeField]
        private Transform truePositionView;

        [SerializeField]
        private Transform heightBeam;

        private float setYaw;
        private float viewYaw = -1;
        private Vector3 setDeltaPosition;
        private Vector3 viewDeltaPosition;

        private float heightBeamYMin;
        private float heightBeamYMax;

        private Vector3 lastPosition;
        private Vector3 lastLastPosition;
        private Run.TimeStamp lastLastPositionTime;

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

        public void InitNonAuthority()
        {
            heightBeam.gameObject.SetActive(false);
            truePositionView.gameObject.SetActive(false);
        }

        public void SimpleMove(Vector3 deltaPosition, Vector3 finalPosition)
        {
            Motor.MoveCharacter(finalPosition);
            if (deltaPosition.y > 0)
            {
                Motor.ForceUnground();
            }
            setDeltaPosition = deltaPosition;
        }

        public void UpdateAim(Vector3 direction)
        {
            setYaw = Util.QuaternionSafeLookRotation(direction, Vector3.up).eulerAngles.y;
        }

        public void UpdateHeightBeam(float yPosition, float maxHeight)
        {
            heightBeamYMin = yPosition;
            heightBeamYMax = yPosition + maxHeight;
        }
        #endregion motor

        #region projection preview
        void Awake()
        {
            inverseFixedDeltaTime = 1 / Time.fixedDeltaTime;

            lastLastPosition = transform.position;
            lastPosition = transform.position;

            projectionAnimator.SetFloat("walkSpeed", 2);
            projectionAnimator.SetBool("isMoving", true);
        }

        void FixedUpdate()
        {
            lastLastPosition = lastPosition;
            lastLastPositionTime = Run.TimeStamp.now;
            Vector3 foundPosition = Find();
            if (foundPosition != default(Vector3))
            {
                lastPosition = foundPosition;
            }
            positionProjection.transform.position = lastPosition;
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
            positionProjection.transform.position = Vector3.Lerp(lastLastPosition, lastPosition, Mathf.Clamp01((Run.TimeStamp.now - lastLastPositionTime) * inverseFixedDeltaTime));
            
            if (viewYaw == -1)
                viewYaw = setYaw;
            viewYaw = Mathf.Lerp(viewYaw, setYaw, 0.2f);
            positionProjection.transform.rotation = Quaternion.Euler(0f, viewYaw, 0f);
            
            viewDeltaPosition = Vector3.Lerp(viewDeltaPosition, setDeltaPosition, 0.2f);
            Vector3 currentDeltaPosition = positionProjection.transform.InverseTransformDirection(viewDeltaPosition);
            if (currentDeltaPosition.sqrMagnitude > 1)
            {
                currentDeltaPosition = currentDeltaPosition.normalized;
            }
            projectionAnimator.SetFloat("rightSpeed", currentDeltaPosition.x, 0.4f, Time.deltaTime);
            projectionAnimator.SetFloat("forwardSpeed", currentDeltaPosition.z, 0.4f, Time.deltaTime);

            Vector3 setPosition = heightBeam.position;
            setPosition.y = Mathf.Min(heightBeamYMin, positionProjection.transform.position.y);
            heightBeam.position = setPosition;

            heightBeam.localScale = new Vector3(heightBeamYMax - setPosition.y, 1, 1);
        }
        #endregion projection preview
    }
}