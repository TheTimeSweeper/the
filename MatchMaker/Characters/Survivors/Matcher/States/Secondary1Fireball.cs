using EntityStates;
using MatcherMod.Survivors.Matcher.Components;
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
        public static float baseDuration = 0.4f;
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

            base.PlayAnimation("Gesture, Override", "StaffShoot", "Staff.playbackRate", this.duration);

            if (consumedMatches > 0)
            {
                GetModelTransform().gameObject.GetComponent<MatcherViewController>().RevealStaff(duration * 2);
            }
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
                        damage = base.characterBody.damage * ((1 + additionalStocks) * CharacterConfig.M2_Staff_Damage.Value + (consumedMatches * CharacterConfig.M2_Staff_Damage_Match.Value)),
                        force = 2000f,
                        crit = base.RollCrit(),
                        damageColorIndex = DamageColorIndex.Default,
                        //speedOverride = 100,
                        projectilePrefab = consumedMatches > 0 ? CharacterAssets.JoeFireballBig : CharacterAssets.JoeFireball
                    };

                    ProjectileManager.instance.FireProjectile(projectileInfo);
                }
            }
        }

        //public override InterruptPriority GetMinimumInterruptPriority() {
        //    return base.GetMinimumInterruptPriority();
        //}
    }
}