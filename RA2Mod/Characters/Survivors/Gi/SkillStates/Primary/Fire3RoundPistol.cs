using EntityStates;
using RA2Mod.Survivors.GI;
using RoR2;
using System;
using UnityEngine;

namespace RA2Mod.Survivors.GI.SkillStates
{
    public class Fire3RoundPistol : BurstFire
    {
        public override float baseInterval => GIConfig.M1_Pistol_Interval.Value;
        public override float baseFinalInterval => GIConfig.M1_Pistol_FinalInterval.Value;
        public override int shurikens => GIConfig.M1_Pistol_Shots.Value;

        public static float damageCoefficient => GIConfig.M1_Pistol_Damage.Value;
        public static float procCoefficient = 1f;
        public static float force = 10f;
        public static float recoil = 0.2f;
        public static float range = 120f;
        public static GameObject tracerEffectPrefab = GIAssets.gunTracer;

        private string muzzleString;

        public override void OnEnter()
        {
            base.OnEnter();
            characterBody.SetAimTimer(2f);
            muzzleString = "HandR";

            PlayAnimation("LeftArm, Override", "ShootGun", "ShootGun.playbackRate", 1.8f);
        }

        protected override void Fire()
        {
            characterBody.AddSpreadBloom(1.5f);
            EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, gameObject, muzzleString, false);
            Util.PlaySound("Play_GIShootSingle", gameObject);

            if (isAuthority)
            {
                Ray aimRay = GetAimRay();
                AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                new BulletAttack
                {
                    bulletCount = 1,
                    aimVector = aimRay.direction,
                    origin = aimRay.origin,
                    damage = damageCoefficient * damageStat,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.Generic,
                    falloffModel = GIConfig.M1_Pistol_Falloff.Value ? BulletAttack.FalloffModel.DefaultBullet : BulletAttack.FalloffModel.None,
                    maxDistance = range,
                    force = force,
                    hitMask = LayerIndex.CommonMasks.bullet,
                    minSpread = 0f,
                    maxSpread = 0f,
                    isCrit = RollCrit(),
                    owner = gameObject,
                    muzzleName = muzzleString,
                    smartCollision = true,
                    procChainMask = default,
                    procCoefficient = procCoefficient,
                    radius = 0.75f,
                    sniper = false,
                    stopperMask = LayerIndex.CommonMasks.bullet,
                    weapon = null,
                    tracerEffectPrefab = tracerEffectPrefab,
                    spreadPitchScale = 0f,
                    spreadYawScale = 0f,
                    queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                    hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.hitEffectPrefab,
                }.Fire();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}