using EntityStates;
using R2API;
using RA2Mod.Survivors.Chrono;
using RoR2;
using UnityEngine;
using static RoR2.BulletAttack;

namespace RA2Mod.Survivors.Chrono.States
{
    public class ChronoShoot : BaseSkillState
    {
        public virtual float damageCoefficient => ChronoConfig.M1_Shoot_Damage.Value;
        public static float procCoefficient = 1f;
        public virtual float baseDuration => ChronoConfig.M1_Shoot_Duration.Value;
        public virtual float hitRadius => ChronoConfig.M1_Shoot_Radius.Value;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0.0f;
        public static float force = 000f;
        public virtual float recoil => ChronoConfig.M1Screenshake.Value;
        public static float range = 256f;
        //public static GameObject tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");

        protected float duration;
        protected string muzzleString;
        private float fireTime;
        private bool hasFired;
        private bool triggeredComboExplosion;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            characterBody.SetAimTimer(2f);
            muzzleString = "Muzzle";
            PlayShootAnimation();
        }

        protected virtual void PlayShootAnimation()
        {
            PlayAnimation("Arms, Override", "cast 2", "cast.playbackRate", duration);
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
                Util.PlaySound("Play_ChronoAttack", gameObject);

                if (isAuthority)
                {
                    Ray aimRay = GetAimRay();
                    AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                    BulletAttack bulletAttack = new BulletAttack {
                        bulletCount = 1,
                        aimVector = aimRay.direction,
                        origin = aimRay.origin,
                        damage = 0,//damageCoefficient * damageStat,
                        procCoefficient = 0,//procCoefficient,
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = DamageType.Generic,
                        falloffModel = BulletAttack.FalloffModel.None,
                        maxDistance = range,
                        force = force,
                        hitMask = LayerIndex.CommonMasks.bullet,
                        minSpread = 0f,
                        maxSpread = 0f,
                        isCrit = false,//RollCrit(),
                        owner = gameObject,
                        muzzleName = muzzleString,
                        smartCollision = true,
                        procChainMask = default,
                        radius = 0.9f,
                        sniper = false,
                        stopperMask = LayerIndex.CommonMasks.bullet,
                        weapon = null,
                        tracerEffectPrefab = ChronoAssets.chronoTracer,
                        spreadPitchScale = 0f,
                        spreadYawScale = 0f,
                        queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                        hitEffectPrefab = null,//EntityStates.Commando.CommandoWeapon.FirePistol2.hitEffectPrefab,
                        hitCallback = HitCallBack
                    };

                    bulletAttack.Fire();
                }
            }
        }

        //credit to moffien with pilot
        private bool HitCallBack(BulletAttack bulletRef, ref BulletHit hitInfo)
        {
            if (hitInfo.point != null && !triggeredComboExplosion)
            {
                triggeredComboExplosion = true;

                if (ChronoAssets.lunarSunExplosion)
                {
                    EffectManager.SpawnEffect(
                        ChronoAssets.lunarSunExplosion, 
                        new EffectData { 
                            origin = hitInfo.point, 
                            scale = hitRadius 
                        }, 
                        true);
                }
                BlastAttack blastAttack = new BlastAttack()
                {
                    attacker = base.gameObject,
                    attackerFiltering = AttackerFiltering.Default,
                    baseDamage = this.damageStat * damageCoefficient,
                    baseForce = 0f,
                    bonusForce = default(Vector3),
                    canRejectForce = true,
                    crit = base.RollCrit(),
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.Generic,
                    falloffModel = BlastAttack.FalloffModel.None,
                    inflictor = base.gameObject,
                    position = hitInfo.point,
                    procChainMask = default,
                    procCoefficient = procCoefficient,
                    radius = hitRadius,
                    teamIndex = base.GetTeam()
                };
                blastAttack.AddModdedDamageType(ChronoDamageTypes.chronoDamage);
                blastAttack.Fire();
            }

            return BulletAttack.defaultHitCallback.Invoke(bulletRef, ref hitInfo);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}