using EntityStates;
using ModdedEntityStates.BaseStates;
using RoR2;
using UnityEngine;

namespace ModdedEntityStates.Joe {
    public class Primary1JumpSwingLand : BaseMeleeAttackButEpic {

        public static float jumpSwingDamage => 3.2f;

        public override void OnEnter() {

            SetSwingValues();

            if (!isGrounded) {
                SmallHop(characterMotor, 1);
            }

            base.OnEnter();

            R2API.DamageAPI.AddModdedDamageType(attack, Modules.DamageTypes.TenticleLifeStealing);
        }

        protected virtual void SetSwingValues() {
            this.hitboxName = "jumpswing";
            
            base.damageType = DamageType.Generic;
            base.damageCoefficient = jumpSwingDamage;
            base.procCoefficient = 1f;
            base.pushForce = 600f;
            base.bonusForce = Vector3.down * 500f;

            base.baseDuration = 12f/60f;
            base.attackStartTime = 0.0f;
            base.attackEndTime = 1f;
            base.baseEarlyExitTime = 0.75f;

            base.hitStopDuration = 0.2f;
            base.attackRecoil = 0.2f;
            base.hitHopVelocity = 4f;

            base.swingSoundString = "play_joe_priAtt2tower";
            base.hitSoundString = "";
            base.muzzleString = "JumpSwingMuzzle";
            base.swingEffectPrefab = Modules.Asset.JoeJumpSwingEffect;
            base.hitEffectPrefab = Modules.Asset.MercImpactEffect; 

            base.impactSound = Modules.Asset.FleshSliceSound.index;
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

            WindDownState newNextState = new WindDownState();
            newNextState.windDownTime = 0.35f;
            base.outer.SetNextState(newNextState);
        }

        public override void OnExit() {

            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Skill;
        }
    }
}