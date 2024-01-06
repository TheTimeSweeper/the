using EntityStates;
using PlagueMod.Survivors.Plague;
using RoR2;
using UnityEngine;

namespace PlagueMod.Survivors.Plague.SkillStates
{
    public class PaintShotgun : BaseSkillState
    {
        public static float damageCoefficient = 0.6f; // 60% damage per pellet
        public static float procCoefficient = 0.5f;
        public static float baseDuration = 0.5f; // Duration of the attack animation
        public static uint bulletCount = 6; // Number of pellets
        public static float range = 20f; // Effective range of the shotgun
        public static float spreadBloom = 1.2f; // Spread of the shotgun pellets

        private float duration;
        private bool hasFired;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            characterBody.SetAimTimer(2f);
            PlayAnimation("Gesture, Additive", "FireShotgun"); // Assuming the animation exists
        }

        private void FireShotgun()
        {
            if (hasFired) return;

            hasFired = true;
            Ray aimRay = GetAimRay();
            characterBody.AddSpreadBloom(spreadBloom);

            var bulletAttack = new BulletAttack
            {
                origin = aimRay.origin,
                aimVector = aimRay.direction,
                damage = damageCoefficient * damageStat,
                damageType = DamageType.Generic, // Can be customized
                falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                maxDistance = range,
                force = 0, // Paint pellets do not apply force
                hitMask = LayerIndex.CommonMasks.bullet,
                isCrit = Util.CheckRoll(critStat, characterBody.master),
                owner = gameObject,
                muzzleName = "Muzzle", // Should correspond to the model
                procCoefficient = procCoefficient,
                bulletCount = bulletCount,
                radius = 0.5f, // Radius of each pellet's hit area
                //spreadBloomAngle = spreadBloom,
                tracerEffectPrefab = null, // Custom effect or null if not needed
                hitEffectPrefab = null, // Custom hit effect or null
                queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                // Custom properties for paint effect
                damageColorIndex = DamageColorIndex.Item, // Example color index for paint
            };

            bulletAttack.Fire();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (fixedAge >= duration && isAuthority)
            {
                FireShotgun();
                outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Any;
        }
    }
    public class Shoot : BaseSkillState
    {
        public static float damageCoefficient = 1;// HenryStaticValues.gunDamageCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.6f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0.0f;
        public static float force = 800f;
        public static float recoil = 3f;
        public static float range = 256f;
        public static GameObject tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");

        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            characterBody.SetAimTimer(2f);
            muzzleString = "Muzzle";

            PlayAnimation("LeftArm, Override", "ShootGun", "ShootGun.playbackRate", 1.8f);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (fixedAge >= fireTime)
            {
                Fire();
            }

            if (fixedAge >= duration && isAuthority)
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        private void Fire()
        {
            if (!hasFired)
            {
                hasFired = true;

                characterBody.AddSpreadBloom(1.5f);
                EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, gameObject, muzzleString, false);
                Util.PlaySound("HenryShootPistol", gameObject);

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
                        falloffModel = BulletAttack.FalloffModel.None,
                        maxDistance = range,
                        force = force,
                        hitMask = LayerIndex.CommonMasks.bullet,
                        minSpread = 0f,
                        maxSpread = 0f,
                        isCrit = RollCrit(),
                        owner = gameObject,
                        muzzleName = muzzleString,
                        smartCollision = false,
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
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}