using EntityStates;
using EntityStates.DefectiveUnit;
using EntityStates.Drifter;
using GorpMod.Modules;
using GorpMod.Enemies.Gorp.Content;
using RoR2;
using RoR2.Projectile;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace GorpMod.Characters.Enemies.Gorp.States
{
    public class GorpMortarAttack : BaseSkillState
    {
        public static float baseDuration => CharacterConfig.Mortar_baseDuration;
        public static float minimumTime => CharacterConfig.Mortar_minimumTime;
        public static float projectileHorizontalSpeed => CharacterConfig.Mortar_projectileHorizontalSpeed;
        public static float animationTimeMulti => CharacterConfig.Mortar_animationTimeMulti;

        protected virtual int firings => 3;
        // Token: 0x04003698 RID: 13976
        protected float duration;
        // Token: 0x04003696 RID: 13974
        protected Transform trackedTarget;
        // Token: 0x04003697 RID: 13975
        private Transform muzzleTransform;
        protected TargetingAndPredictionController getTargetComponent;

        private int fired = 0;
        
        public override void OnEnter()
        {
            base.OnEnter();
            this.getTargetComponent = base.GetComponent<TargetingAndPredictionController>();
            duration = baseDuration / attackSpeedStat;
            if (this.getTargetComponent && base.isAuthority)
            {
                this.trackedTarget = this.getTargetComponent.StartPredictTarget(new Action(this.OnTargetLost));
            }
            this.muzzleTransform = base.FindModelChild("MortarMuzzle");

            PlayTheOneAnimationWeGot();
        }

        private void PlayTheOneAnimationWeGot()
        {
            PlayAnimation("Body", "SpikeAttack", "SpikeAttack.playbackRate", duration * animationTimeMulti);
        }

        // Token: 0x060029E9 RID: 10729 RVA: 0x000D1336 File Offset: 0x000CF536
        private void OnTargetLost()
        {
            this.trackedTarget = null;
            if (base.isAuthority)
            {
                this.outer.SetNextStateToMain();
            }
        }

        // Token: 0x060029EC RID: 10732 RVA: 0x000D13C4 File Offset: 0x000CF5C4
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (!this.trackedTarget && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
            if (base.fixedAge >= this.duration)
            {
                if (fired < firings)
                {
                    fired++;
                    fixedAge = 0;
                    this.FireProjectile();
                }
                else
                {
                    if (base.isAuthority)
                    {
                        this.outer.SetNextStateToMain();
                    }
                }
            }
        }

        // Token: 0x060029ED RID: 10733 RVA: 0x000D1420 File Offset: 0x000CF620
        protected virtual void FireProjectile()
        {
            if (!this.trackedTarget)
            {
                return;
            }
            //for the next fire
            PlayTheOneAnimationWeGot();

            if (NetworkServer.active)
            {

                MasterSummon masterSummon = new MasterSummon
                {
                    masterPrefab = CharacterAssets.GipMaster,
                    position = muzzleTransform.position,
                    rotation = transform.rotation,
                    summonerBodyObject = null,
                    teamIndexOverride = teamComponent.teamIndex,
                    ignoreTeamMemberLimit = true,
                    propogateMinionOwnership = false,
                    loadout = null,
                    inventoryToCopy = null,//characterBody.inventory,
                    inventoryItemCopyFilter = Inventory.defaultItemCopyFilterDelegate,
                    inventorySetupCallback = null,
                    preSpawnSetupCallback = null,
                    useAmbientLevel = true
                };
                CharacterMaster characterMaster = masterSummon.Perform();
                characterMaster.onBodyStart += CharacterMaster_onBodyStart;

            }
        }

        private void CharacterMaster_onBodyStart(CharacterBody spawnedBody)
        {
            Vector3 vector = this.trackedTarget.position - this.muzzleTransform.position;
            vector.y = 0f;
            float magnitude = vector.magnitude;
            float predictionTime = Mathf.Max(minimumTime, magnitude / projectileHorizontalSpeed);
            Vector3 position;
            if (!this.getTargetComponent.GetPredictionPosition(predictionTime, out position))
            {
                position = this.trackedTarget.position;
            }
            Vector3 initialVelocity = Trajectory.CalculateInitialVelocityFromTime(transform.position, position, predictionTime, Physics.gravity.y * 2.2f, DenialProjectile.minimumDistance, DenialProjectile.maximumDistance);

            FireProjectileInfo fireProjectileInfo = new FireProjectileInfo
            {
                projectilePrefab = EntityStates.Poltergeist.DeathState.throwPrefab,
                owner = base.gameObject,
                position = this.muzzleTransform.position,
                rotation = Util.QuaternionSafeLookRotation(initialVelocity.normalized),
                damage = DenialProjectile.damageCoefficient * this.damageStat,
                crit = base.RollCrit(),
                speedOverride = initialVelocity.magnitude,
                useSpeedOverride = true
            };
            ThrownObjectProjectileController component = ProjectileManager.instance.FireProjectileImmediateServer(fireProjectileInfo, null, 0, 0.0).GetComponent<ThrownObjectProjectileController>();
            if (component != null)
            {
                component.SetPassengerServer(spawnedBody.gameObject);
            }
            if (component == null)
            {
                return;
            }
            component.DisableCollision(0.1f);

            DamageInfo info = new DamageInfo
            {
                damage = spawnedBody.healthComponent.health,//healthComponent.fullCombinedHealth * CharacterConfig.Mortar_healthPercentCost
            };
            healthComponent.TakeDamage(info);
        }
    }
}
