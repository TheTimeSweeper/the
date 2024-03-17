using RA2Mod.General;
using RA2Mod.Modules.BaseStates;
using RoR2;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RA2Mod.Survivors.Tesla.States
{

    public class ZapPunch : BaseMeleeAttack
    {

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

        public static NetworkSoundEventDef loaderZapFistSoundEvent = LegacyResourcesAPI.Load<NetworkSoundEventDef>("NetworkSoundEventDefs/nseLoaderM1Impact");

        public override void OnEnter()
        {

            hitboxGroupName = "PunchHitbox";

            damageCoefficient = GetDamageCoefficient();

            procCoefficient = ProcCoefficient;
            pushForce = 930;

            baseDuration = BasePunchDuration;
            attackStartPercentTime = baseAttackStartTime * animationDuration;
            attackEndPercentTime = baseAttackEndTime * animationDuration;
            earlyExitPercentTime = 0.8f;

            hitStopDuration = 0.14f;
            swingSoundString = "Play_PunchWoosh";
            hitSoundString = "";
            muzzleString = "PunchHitboxAnchor"; // swingIndex % 2 == 0 ? "SwingLeft" : "SwingRight";
            playbackRateParam = "Shock.playbackRate";
            swingEffectPrefab = TeslaAssets.TeslaZapConeEffect;
            hitEffectPrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/omnieffect/omniimpactvfxloader"); // Modules.Assets.swordHitImpactEffect;

            impactSound = RoR2.Audio.NetworkSoundEventIndex.Invalid;

            ModifyState();

            base.OnEnter();
        }

        protected virtual float GetDamageCoefficient()
        {
            return DefaultDamageCoefficient;
        }

        protected virtual void ModifyState() { }

        protected override void PlayAttackAnimation()
        {
            PlayAnimation("Gesture, Override", "ShockPunch", "Punch.playbackRate", duration * animationDuration);
        }

        protected override void FireAttackEnter()
        {
            base.FireAttackEnter();

            Vector3 direction = this.GetAimRay(true).direction;
            direction.y = Mathf.Max(direction.y, direction.y * 0.5f);
            FindModelChild("PunchHitboxAnchor").rotation = Util.QuaternionSafeLookRotation(direction);

            FireConeProjectile();
        }

        protected virtual void FireConeProjectile()
        {

            if (isAuthority)
            {
                if (FindModelChild(muzzleString))
                {
                    FireProjectileInfo fireProjectileInfo = default;
                    fireProjectileInfo.position = FindModelChild(muzzleString).position;
                    fireProjectileInfo.rotation = Quaternion.LookRotation(GetAimRay().direction);
                    fireProjectileInfo.crit = attack.isCrit;
                    fireProjectileInfo.damage = damageCoefficient * OrbDamageMultiplier * damageStat;
                    fireProjectileInfo.owner = gameObject;
                    fireProjectileInfo.projectilePrefab = TeslaAssets.TeslaLoaderZapConeProjectile;
                    ProjectileManager.instance.FireProjectile(fireProjectileInfo);
                };
            }
        }

        //protected override void OnHitEnemyAuthority(List<HurtBox> hits) {
        //    base.OnHitEnemyAuthority(hits);
        //}
    }
}