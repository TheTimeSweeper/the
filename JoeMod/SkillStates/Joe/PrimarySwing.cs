using EntityStates;
using HenryMod.ModdedEntityStates.BaseStates;
using RoR2;
using UnityEngine;

namespace HenryMod.ModdedEntityStates.Joe {

    public class Primary1Swing : BaseMeleeAttackButEpic {

        public static float swingDamage = 1.6f;
        public static float jumpSwingDamage = 5f;

        private bool jumpSwing = false;

        public override void OnEnter() {

            if (!isGrounded) {
                base.outer.SetNextState(new Primary1JumpSwingFall());
                jumpSwing = true;
                return;
            }

            SetSwingValues();

            base.OnEnter();
        }

        protected virtual void SetSwingValues() {
            this.hitboxName = swingIndex % 2 == 0 ? "swing1" : "swing2";

            base.damageType = DamageType.Generic;
            base.damageCoefficient = Primary1Swing.swingDamage;
            base.procCoefficient = 1f;
            base.pushForce = 100f;
            base.bonusForce = Vector3.zero;

            base.baseDuration = 0.96f;
            base.attackStartTime = 0.05f;
            base.attackEndTime = 0.31f;
            base.baseEarlyExitTime = 0.38f;

            base.hitStopDuration = 0.0069f;
            base.attackRecoil = 0.2f;
            base.hitHopVelocity = 4f;

            base.swingSoundString = "";
            base.hitSoundString = "";
            base.muzzleString = "";// swingIndex % 2 == 0 ? "SwingLeft" : "SwingRight";
            base.swingEffectPrefab = null;// Modules.Assets.swordSwingEffect;
            base.hitEffectPrefab = null;// Modules.Assets.swordHitImpactEffect;

            base.impactSound = Modules.Assets.swordHitSoundEvent.index;
        }

        public override void FixedUpdate() {

            if (jumpSwing)
                return;

            base.FixedUpdate();
            base.StartAimMode();
        }

        protected override void PlayAttackAnimation() {
            base.PlayAnimation("Arms, Override",
                               swingIndex % 2 == 0 ? "swing1 v2" : "swing2 v2",
                               "swing.playbackRate", 
                               this.duration);
        }

        protected override void PlaySwingEffect() {
            base.PlaySwingEffect();
        }

        protected override void OnHitEnemyAuthority() {
            base.OnHitEnemyAuthority();
        }

        protected override void SetNextState() {
            //int index = this.swingIndex;
            //if (index == 0) index = 1;
            //else index = 0;

            this.outer.SetNextState(new Primary1Swing  {
                swingIndex = this.swingIndex + 1
            });
        }

        public override void OnExit() {

            if (jumpSwing)
                return;
            base.OnExit();
        }
    }

    public class Primary1JumpSwingFall : BaseSkillState {

        private float _extraGravity = 1.8f;

        public override void OnEnter() {

            base.PlayCrossfade("Arms, Override", "jumpSwingReady", 0.1f);

            base.characterBody.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;

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

            base.characterBody.bodyFlags -= CharacterBody.BodyFlags.IgnoreFallDamage;
            base.OnExit();
        }

        public override void Update() {
            base.Update();
        }
    }

    public class Primary1JumpSwingLand : BaseMeleeAttackButEpic {

        public static float swingDamage = 1.6f;

        private bool jumpSwing = false;

        public override void OnEnter() {

            SetSwingValues();

            //JOE: slow for a bit

            base.OnEnter();
        }

        protected virtual void SetSwingValues() {
            this.hitboxName = "jumpswing";

            base.damageType = DamageType.Generic;
            base.damageCoefficient = Primary1Swing.jumpSwingDamage;
            base.procCoefficient = 1f;
            base.pushForce = 600f;
            base.bonusForce = Vector3.zero;

            base.baseDuration = 0.3f;
            base.attackStartTime = 0.3f;
            base.attackEndTime = 1f;
            base.baseEarlyExitTime = 0.75f;

            base.hitStopDuration = 0.2f;
            base.attackRecoil = 0.2f;
            base.hitHopVelocity = 4f;

            base.swingSoundString = "_priAtt2tower";
            base.hitSoundString = "";
            base.muzzleString = "JumpSwingMuzzle";// swingIndex % 2 == 0 ? "SwingLeft" : "SwingRight";
            base.swingEffectPrefab = Modules.Assets.JoeJumpSwingEffect;// Modules.Assets.swordSwingEffect;
            base.hitEffectPrefab = null;// Modules.Assets.swordHitImpactEffect;

            base.impactSound = Modules.Assets.swordHitSoundEvent.index;
        }

        public override void FixedUpdate() {

            base.FixedUpdate();
            base.StartAimMode();
        }

        protected override void PlayAttackAnimation() {

            base.PlayAnimation("Arms, Override", "jumpSwingLand", "jumpSwing.playbackRate", this.duration);
        }

        protected override void PlaySwingEffect() {
            base.PlaySwingEffect();
        }

        protected override void OnHitEnemyAuthority() {
            base.OnHitEnemyAuthority();
        }

        protected override void SetNextState() {

            return;
        }

        public override void OnExit() {

            base.OnExit();
        }
    }


    public class PrimaryStupidSwing : Primary1Swing {

        protected override void SetSwingValues() {

            base.SetSwingValues();

            base.attackStartTime = 0.0f;
            base.baseEarlyExitTime = 0.0f;
            base.keypress = true;
        }

        protected override void SetNextState() {
            //int index = this.swingIndex;
            //if (index == 0) index = 1;
            //else index = 0;

            this.outer.SetNextState(new PrimaryStupidSwing {
                swingIndex = this.swingIndex + 1
            });
        }
    }
}