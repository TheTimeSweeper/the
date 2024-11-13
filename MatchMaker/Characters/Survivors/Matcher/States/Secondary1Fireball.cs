using EntityStates;
using MatcherMod.Survivors.Matcher.Content;
using MatcherMod.Survivors.Matcher.SkillDefs;
using RoR2;
using RoR2.Projectile;
using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements.StyleSheets;

namespace MatcherMod.Survivors.Matcher.SkillStates
{
    public class Secondary1Fireball : BaseSkillState, IMatchBoostedState {

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

                int additionalStocks = activatorSkillSlot.stock;
                activatorSkillSlot.stock = 0;

                if (base.isAuthority) {
                    Ray aimRay = base.GetAimRay();

                    FireProjectileInfo projectileInfo = new FireProjectileInfo
                    {
                        position = aimRay.origin,
                        rotation = Util.QuaternionSafeLookRotation(aimRay.direction),
                        owner = base.gameObject,
                        damage = CharacterConfig.M2_Staff_Damage.Value * (1 + consumedMatches) * (1 + additionalStocks) * base.characterBody.damage,
                        force = 2000f,
                        crit = base.RollCrit(),
                        damageColorIndex = DamageColorIndex.Default,
                        //speedOverride = 100,
                        projectilePrefab = Content.CharacterAssets.JoeFireball
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