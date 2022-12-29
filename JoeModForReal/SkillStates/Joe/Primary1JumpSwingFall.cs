using EntityStates;
using RoR2;
using UnityEngine;

namespace ModdedEntityStates.Joe {
    public class Primary1JumpSwingFall : BaseSkillState {

        private float _extraGravity = 2.2f;

        public override void OnEnter() {

            base.PlayCrossfade("Arms, Override", "jumpSwingReady", 0.1f);

            base.characterBody.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;

            ref float ySpeed = ref characterMotor.velocity.y;
            ySpeed = Mathf.Max(ySpeed, 0);

            //TODO: add/set initial upwards yspeed 
            //       - hithop?

            base.OnEnter();
        }

        public override void FixedUpdate() {

            StartAimMode();

            ref float ySpeed = ref characterMotor.velocity.y;
            ySpeed += Physics.gravity.y * _extraGravity * Time.deltaTime;

            if (base.isGrounded) {
                base.outer.SetNextState(new Primary1JumpSwingLand());
                return;
            }

            base.FixedUpdate();
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