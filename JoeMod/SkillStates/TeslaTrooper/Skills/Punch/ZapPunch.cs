using ModdedEntityStates.BaseStates; //todo just take make them in root moddedentitystates
using RoR2;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ModdedEntityStates.TeslaTrooper {

    public class ZapPunch : BaseMeleeAttackButEpic {

        #region Gameplay Values
        public static float DefaultDamageCoefficient = 3f;
        public static float ProcCoefficient = 1f;

        public static float OrbDamageMultiplier = 0.5f;
        public static float OrbProcCoefficient = 0.7f;
        public static int OrbCasts = 20;
        public static float OrbDistance = 25;

        public float BasePunchDuration = 1;

        protected float animationDuration = 0.46f;
        protected float baseAttackStartTime = 0.42f;
        protected float baseAttackEndTime = 0.71f;
        #endregion

        public static NetworkSoundEventDef loaderZapFistSoundEvent = RoR2.LegacyResourcesAPI.Load<NetworkSoundEventDef>("NetworkSoundEventDefs/nseLoaderM1Impact");

        public override void OnEnter() {
            
            base.hitboxName = "PunchHitbox";

            base.damageCoefficient = GetDamageCoefficient();

            base.procCoefficient = ProcCoefficient;
            base.pushForce = 690f;

            base.baseDuration = BasePunchDuration;
            base.attackStartTime = baseAttackStartTime * animationDuration;
            base.attackEndTime = baseAttackEndTime * animationDuration;
            base.baseEarlyExitTime = 0.8f;

            base.hitStopDuration = 0.14f;
            base.swingSoundString = "";
            base.hitSoundString = "";
            base.muzzleString = "PunchHitboxAnchor"; // swingIndex % 2 == 0 ? "SwingLeft" : "SwingRight";
            base.hitstopAnimationParameter = "Shock.playbackRate";
            base.swingEffectPrefab = Modules.Assets.TeslaZapConeEffect;
            base.hitEffectPrefab = Modules.Assets.LoadAsset<GameObject>("prefabs/effects/omnieffect/omniimpactvfxloader"); // Modules.Assets.swordHitImpactEffect;

            base.impactSound = RoR2.Audio.NetworkSoundEventIndex.Invalid;

            ModifyState();

            base.OnEnter();
        }

        protected virtual float GetDamageCoefficient() {
            return DefaultDamageCoefficient;
        }

        protected virtual void ModifyState() { }

        protected override void PlayAttackAnimation() {
            base.PlayAnimation("Gesture, Override", "ShockPunch", "Punch.playbackRate", duration* animationDuration);
        }

        protected override void OnFireAttackEnter() {

            Vector3 direction = Modules.VRCompat.GetAimRay(this).direction;
            direction.y = Mathf.Max(direction.y, direction.y * 0.5f);
            FindModelChild("PunchHitboxAnchor").rotation = Util.QuaternionSafeLookRotation(direction);

            FireConeProjectile();
        }

        protected virtual void FireConeProjectile() {

            if (isAuthority) {
                if (base.FindModelChild(base.muzzleString)) {
                    FireProjectileInfo fireProjectileInfo = default(FireProjectileInfo);
                    fireProjectileInfo.position = base.FindModelChild(this.muzzleString).position;
                    fireProjectileInfo.rotation = Quaternion.LookRotation(GetAimRay().direction);
                    fireProjectileInfo.crit = rolledCrit;
                    fireProjectileInfo.damage = damageCoefficient * OrbDamageMultiplier * this.damageStat;
                    fireProjectileInfo.owner = base.gameObject;
                    fireProjectileInfo.projectilePrefab = Modules.Assets.TeslaLoaderZapConeProjectile;
                    ProjectileManager.instance.FireProjectile(fireProjectileInfo);
                };
            }
        }

        protected override void OnHitEnemyAuthority(List<HurtBox> hits) {
            base.OnHitEnemyAuthority(hits);
        }
    }
}