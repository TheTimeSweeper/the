using EntityStates;
using Matchmaker.Survivors.Matcher.SkillDefs;
using RoR2;
using RoR2.Projectile;
using System;
using UnityEngine;

namespace MatcherMod.Survivors.Matcher.SkillStates {

    public class Secondary1Fireball : BaseSkillState, IMatchBoostedState {

        public static float damageCoefficient => 5.5f;

        public int consumedMatches { get; set; }

        public static float procCoefficient = 1f;
        public static float baseDuration = 0.3f;
        //public static float throwForce = 80f;

        private float duration;
        private float castTime;
        private bool hasFired;
        private Animator animator;

        public override void OnEnter() {
            base.OnEnter();
            this.duration = Secondary1Fireball.baseDuration / this.attackSpeedStat;
            this.castTime = 0;
            base.characterBody.SetAimTimer(2f);
            this.animator = base.GetModelAnimator();

            base.PlayAnimation("Arms, Override", "cast 2", "cast.playbackRate", this.duration);
        }

        public override void OnExit() {
            base.OnExit();
        }

        public override void FixedUpdate() {
            base.FixedUpdate();

            if (base.fixedAge >= this.castTime) {
                this.Fire();
            }

            if (base.fixedAge >= this.duration && base.isAuthority) {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        private void Fire() {

            if (!this.hasFired) {
                this.hasFired = true;
                Util.PlaySound("play_joe_fireShoot", base.gameObject);

                if (base.isAuthority) {
                    Ray aimRay = base.GetAimRay();

                    FireProjectileInfo projectileInfo = new FireProjectileInfo
                    {
                        position = aimRay.origin,
                        rotation = Util.QuaternionSafeLookRotation(aimRay.direction),
                        owner = base.gameObject,
                        damage = Secondary1Fireball.damageCoefficient * this.damageStat * (1 + consumedMatches),
                        force = 4000f,
                        crit = base.RollCrit(),
                        damageColorIndex = DamageColorIndex.Default,
                        speedOverride = 100,
                        projectilePrefab = MatcherContent.Assets.JoeFireball
                    };

                    ProjectileManager.instance.FireProjectile(projectileInfo);
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return base.GetMinimumInterruptPriority();
        }
    }
}