using ModdedEntityStates.BaseStates;
using RoR2;
using UnityEngine;

namespace ModdedEntityStates.Joe {

    public class Primary1Swing : BaseMeleeAttackButEpic {

        public static float swingDamage => TestValueManager.value5;
        public static float jumpSwingDamage = 5f;

        private bool jumpSwing = false;

        public override void OnEnter() {

            SetSwingValues();

            base.OnEnter();
            R2API.DamageAPI.AddModdedDamageType(attack, Modules.DamageTypes.TenticleLifeStealing);
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

            base.swingSoundString = "play_joe_whoosh";
            base.hitSoundString = "";
            base.muzzleString = "";// swingIndex % 2 == 0 ? "SwingLeft" : "SwingRight";
            base.swingEffectPrefab = null;// Modules.Assets.swordSwingEffect;
            base.hitEffectPrefab = null;// Modules.Assets.swordHitImpactEffect;

            base.impactSound = Modules.Assets.FleshSliceSound.index;
        }

        public override void FixedUpdate() {

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

            base.OnExit();
        }
    }
}