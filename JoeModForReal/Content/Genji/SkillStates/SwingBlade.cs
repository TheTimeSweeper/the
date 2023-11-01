using JoeModForReal.Content.Survivors;
using ModdedEntityStates.BaseStates;
using RoR2;
using UnityEngine;

namespace ModdedEntityStates.Genji {
    public class SwingBlade : BaseMeleeAttackButEpic {

        public static float bladeDamage => GenjiConfig.dragonBladeDamage.Value;
        public static float bladeBaseDuration => GenjiConfig.dragonBladeSwingDuration.Value;

        public override void OnEnter() {
            SetSwingValues();
            base.OnEnter();
        }

        protected virtual void SetSwingValues() {
            this.hitboxName = swingIndex % 2 == 0 ? "swing1" : "swing2";

            base.damageType = DamageType.Generic;
            base.damageCoefficient = bladeDamage;
            base.procCoefficient = 1f;
            base.pushForce = 100f;
            base.bonusForce = Vector3.zero;
            
            base.baseDuration = bladeBaseDuration;
            base.attackStartTime = 0.1f;//todo animation
            base.attackEndTime = 0.4f;
            base.baseEarlyExitTime = 0.5f;

            base.hitStopDuration = 0.1f;
            base.attackRecoil = 0.2f;
            base.hitHopVelocity = 3f;

            base.swingSoundString = "";//"play_joe_whoosh";
            base.hitSoundString = "";
            base.muzzleString = "notMercSlash";// swingIndex % 2 == 0 ? "SwingLeft" : "SwingRight";
            base.swingEffectPrefab = Modules.Assets.MercSwordSlash;
            base.hitEffectPrefab = Modules.Assets.MercImpactEffect;// Modules.Assets.swordHitImpactEffect;

            base.impactSound = Modules.Assets.FleshSliceSound.index;
        }

        public override void FixedUpdate() {
            base.FixedUpdate();

            StartAimMode();
        }

        protected override void PlayAttackAnimation() {
            base.PlayAnimation("Arms, Override",
                               "swing1 v2",
                               "swing.playbackRate",
                               this.duration*2);
        }
    }
}