using ModdedEntityStates.BaseStates;
using RoR2;
using UnityEngine;

namespace ModdedEntityStates.Joe {
    public class Primary1JumpSwingLand : BaseMeleeAttackButEpic {

        public override void OnEnter() {

            SetSwingValues();

            //JOE: slow for a bit

            base.OnEnter();

            R2API.DamageAPI.AddModdedDamageType(attack, Modules.DamageTypes.TenticleLifeStealing);
        }

        protected virtual void SetSwingValues() {
            this.hitboxName = "jumpswing";

            base.damageType = DamageType.Generic;
            base.damageCoefficient = Primary1Swing.jumpSwingDamage;
            base.procCoefficient = 1f;
            base.pushForce = 600f;
            base.bonusForce = Vector3.zero;

            base.baseDuration = 12f/60f;
            base.attackStartTime = 0.0f;
            base.attackEndTime = 1f;
            base.baseEarlyExitTime = 0.75f;

            base.hitStopDuration = 0.2f;
            base.attackRecoil = 0.2f;
            base.hitHopVelocity = 4f;

            base.swingSoundString = "play_joe_priAtt2tower";
            base.hitSoundString = "";
            base.muzzleString = "JumpSwingMuzzle";// swingIndex % 2 == 0 ? "SwingLeft" : "SwingRight";
            base.swingEffectPrefab = Modules.Assets.JoeJumpSwingEffect;// Modules.Assets.swordSwingEffect;
            base.hitEffectPrefab = null;// Modules.Assets.swordHitImpactEffect;

            base.impactSound = Modules.Assets.FleshSliceSound.index;
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
}