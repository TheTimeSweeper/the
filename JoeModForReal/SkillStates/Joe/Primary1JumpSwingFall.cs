using EntityStates;
using RoR2;
using System;
using UnityEngine;

namespace ModdedEntityStates.Joe {
    public class Primary1JumpSwingFall : BaseSkillState {

        private float _extraGravity = 2.7f;

        public override void OnEnter() {

            base.PlayCrossfade("Arms, Override", "jumpSwingReady", 0.1f);

            base.characterBody.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;

            ref float ySpeed = ref characterMotor.velocity.y;
            ySpeed = Mathf.Min(ySpeed, 0);

            base.OnEnter();
        }

        public override void FixedUpdate() {
            
            StartAimMode();

            ref float ySpeed = ref characterMotor.velocity.y;
            ySpeed += Physics.gravity.y * _extraGravity * Time.deltaTime;

            if (base.isGrounded /*|| CheckEnemy()*/) {
                base.outer.SetNextState(GetLandState());
                return;
            }

            base.FixedUpdate();
        }


        protected virtual EntityState GetLandState() {
            return new Primary1JumpSwingLand();
        }

        private bool CheckEnemy() {

            if (base.fixedAge < 0.1f)
                return false;

            Ray mond = new Ray(FindModelChild("jumpSwingCheck").position, Vector3.forward);

            if (Util.CharacterSpherecast(gameObject, mond, 1.3f, out RaycastHit HitInfo, 0, LayerIndex.entityPrecise.mask, QueryTriggerInteraction.UseGlobal)) {

                if (FriendlyFireManager.ShouldDirectHitProceed(HitInfo.collider.GetComponent<HurtBox>().healthComponent, teamComponent.teamIndex)){
                    return true;
                }
                //if (HitInfo.collider.GetComponent<HurtBox>().healthComponent.body.teamComponent.teamIndex == teamComponent.teamIndex)
                   //return HitInfo.collider.GetComponent<HurtBox>().healthComponent.body;
            }

            return false;
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.PrioritySkill;
        }

        public override void OnExit() {

            base.characterBody.bodyFlags &= ~CharacterBody.BodyFlags.IgnoreFallDamage;
            base.OnExit();
        }

        public override void Update() {
            base.Update();
        }
    }
}