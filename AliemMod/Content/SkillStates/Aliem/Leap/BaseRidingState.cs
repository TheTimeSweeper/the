using AliemMod.Components;
using AliemMod.Content;
using AliemMod.Modules;
using EntityStates;
using ModdedEntityStates.Aliem.AI;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace ModdedEntityStates.Aliem
{
    public class BaseRidingState : GenericCharacterMain {

		public CharacterBody riddenBody;

		public Collider riddenCollider;

        private GameObject _anchor;

        private Vector3 _initialPosition;
        private Vector3 _lastPosition;

        private Quaternion _lastRotation;

        private Transform _modelTransform;

        public override void OnEnter() {
            base.OnEnter();

            if (this.riddenBody && NetworkServer.active) {
				this.riddenBody.AddBuff(Buffs.riddenBuff);
			}

			riddenCollider = findHighestHurtbox();
            if (riddenCollider == null)
            {
                outer.SetNextStateToMain();
                return;
            }
            GetComponent<AliemRidingColliderHolderThatsIt>().riddenCollider = riddenCollider;
            Vector3 ridePosition = riddenCollider.bounds.center;
            ridePosition.y = riddenCollider.bounds.max.y;
            _anchor = new GameObject("aliemAnchor");
            _anchor.transform.SetParent(riddenCollider.transform);
            _anchor.transform.position = ridePosition;
            if (this.riddenBody.modelLocator != null && this.riddenBody.modelLocator.modelTransform != null)
            {
                _anchor.transform.rotation = this.riddenBody.modelLocator.modelTransform.rotation;
            }else
            {
                _anchor.transform.rotation = this.riddenBody.gameObject.transform.rotation;
            }

            if (AliemConfig.M3_Leap_RidingControl.Value && riddenBody && riddenBody.master && riddenBody.master.aiComponents.Length > 0 && riddenBody.master.aiComponents[0])
            {
                riddenBody.master.aiComponents[0].stateMachine.SetNextState(new OverrideAICombat { overridor = inputBank});
            }

            gameObject.layer = RoR2.LayerIndex.fakeActor.intVal;

            _modelTransform = GetModelTransform();
            _initialPosition = _modelTransform ? _modelTransform.position : transform.position;
            _lastPosition = _initialPosition;

            _lastRotation = transform.rotation;

            //this.characterDirection.enabled = false;
            this.modelLocator.enabled = false;

           //modelAnimator.SetLayerWeight(modelAnimator.GetLayerIndex("AimPitch"), 0);
           //modelAnimator.SetLayerWeight(modelAnimator.GetLayerIndex("AimYaw"), 0);
        }

        private Collider findHighestHurtbox() {

            if (riddenBody.hurtBoxGroup == null)
                return null;

            HurtBox[] hurtboxes = riddenBody.hurtBoxGroup.hurtBoxes;
			HurtBox highestHurtbox = null;
            for (int i = 0; i < hurtboxes.Length; i++) {

                HurtBox hurtBox = hurtboxes[i];
                if (hurtBox == null)
                    continue;
                if(highestHurtbox == null)
                {
                    highestHurtbox = hurtBox;
                    continue;
                }
				if(hurtBox.transform.position.y > highestHurtbox.transform.position.y) {
					highestHurtbox = hurtBox;
                }
            }
			return highestHurtbox?.collider;
        }

        public override void Update()
        {
            base.Update();

            if (riddenCollider != null && _anchor != null)
            {
                UpdateRidingPosition();
            }
        }

        public override void FixedUpdate() {
            base.FixedUpdate();

			if (riddenBody.healthComponent.alive) {

				if (base.isAuthority && base.characterMotor) {
					
					characterMotor.moveDirection = Vector3.zero;
					characterMotor.velocity = Vector3.zero;
					characterMotor.rootMotion = Vector3.zero;

                    if (riddenCollider != null && _anchor != null)
                    {
                        UpdateRidingPosition(true);
                    }
                    else {

						base.outer.SetNextStateToMain();
						return;
					}
				}
			} else {
				base.outer.SetNextStateToMain();
				return;
            }
		}

        private void UpdateRidingPosition(bool fixedUpdate = false)
        {
            Vector3 lerpPosition = Vector3.Lerp(_initialPosition, _anchor.transform.position, base.fixedAge * 3);

            lerpPosition = Vector3.Lerp(_lastPosition, lerpPosition, 0.5f);

            characterMotor.Motor.SetPosition(lerpPosition);
            
            if (_modelTransform)
            {
                _modelTransform.position = lerpPosition;
                Quaternion rotation = Quaternion.Lerp(_lastRotation, _anchor.transform.rotation, 0.1f);
                _modelTransform.rotation = rotation;
                characterDirection.forward = _anchor.transform.forward;
                _lastRotation = rotation;
            }

            _lastPosition = lerpPosition;
        }

        public override void OnExit() {
			base.OnExit();
			if (riddenBody && NetworkServer.active) {
				riddenBody.RemoveBuff(Buffs.riddenBuff);
            }

            if (AliemConfig.M3_Leap_RidingControl.Value && riddenBody && riddenBody.master && riddenBody.master.aiComponents.Length > 0 && riddenBody.master.aiComponents[0])
            {
                riddenBody.master.aiComponents[0].stateMachine.SetNextState(new EntityStates.AI.Walker.Combat());
            }

            GetComponent<AliemRidingColliderHolderThatsIt>().riddenCollider = null;
            gameObject.layer = RoR2.LayerIndex.defaultLayer.intVal;
            UnityEngine.Object.Destroy(_anchor);

            PlayAnimation("FullBody, Underride", "BufferEmpty");

            //this.characterDirection.enabled = true;
            this.modelLocator.enabled = true;

            //modelAnimator.SetLayerWeight(modelAnimator.GetLayerIndex("AimPitch"), 1);
            //modelAnimator.SetLayerWeight(modelAnimator.GetLayerIndex("AimYaw"), 1);
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            if (riddenBody)
            {
                writer.Write(riddenBody.gameObject);
            }
        }
        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            GameObject readGameObject = reader.ReadGameObject();
            if (readGameObject)
            {
                riddenBody = readGameObject.GetComponent<CharacterBody>();
            }
        }
    }
}
