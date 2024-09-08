using EntityStates;
using RA2Mod.Modules.BaseStates;
using RoR2;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

namespace RA2Mod.Survivors.Desolator.States
{

    public class DeployIrradiate : BaseTimedSkillState {

        #region gameplay Values
        public static float DamageCoefficient = 0.2f;
        public static float BarrierPercentPerEnemy = 0.05f;
        public static float Range = 60;
        public static float MaxBarrierPercent = 0.35f;
        public const float SqrBarrierRange = 1600;

        public static float BaseDuration = 3f;
        public static float StartTime = 1f;
        #endregion

        public RoR2.CameraTargetParams.AimRequest aimRequest;
        public bool fromEnter;
        protected bool _complete;
        private Animator _animator;
        private float _cannonSpin;
        private bool _heldTooLongYaDoofus;
        private bool _inputDown;

        protected virtual GameObject deployProjectilePrefab => DesolatorAssets.DesolatorDeployProjectile;

        public override float TimedBaseDuration => BaseDuration;
        public override float TimedBaseCastStartPercentTime => StartTime;

        public override void OnEnter() {
            base.OnEnter();

            _animator = base.GetModelAnimator();

            Util.PlaySound("Play_Desolator_Deploy", base.gameObject);
            PlayCrossfade("FullBody, Override", "DesolatorDeployPump",/* "DeployPump.playbackRate", duration,*/ 0.05f);

            if (General.GeneralCompat.driverInstalled)
            {
                TryDriverCompat();
            }

            if (base.isAuthority) {
                DropRadiationProjectile();
                GiveBarrierPerEnemy();
            }

            if (NetworkServer.active) {
                characterBody.AddBuff(DesolatorBuffs.desolatorDeployBuff);
            }

            //bit of overengineering but input is important
            if (isAuthority && fromEnter && base.inputBank.skill4.down) {
                _heldTooLongYaDoofus = true;
            } else {
                _inputDown = true;
            }

            characterBody.hideCrosshair = true;
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private void TryDriverCompat()
        {
            if (gameObject.TryGetComponent(out RobDriver.Modules.Components.DriverController cantDrive55))
            {
                cantDrive55.StartTimer(1);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            duration = TimedBaseDuration / characterBody.attackSpeed;

            if (_heldTooLongYaDoofus && isAuthority && base.inputBank.skill4.justReleased)
            {
                _heldTooLongYaDoofus = false;
            }
            if (!_heldTooLongYaDoofus && isAuthority && base.inputBank.skill4.justPressed)
            {
                _inputDown = true;
            }

            //bit of a hack to get around Body ESM not being in GenericCharacterMain
            if (isAuthority && GetThisSkillPressed() && _inputDown)
            {
                outer.SetNextState(new DeployCancel());
            }

            if (isAuthority && inputBank.skill3.justPressed)
            {
                skillLocator.utility.ExecuteIfReady();
            }
        }

        private bool GetThisSkillPressed()
        {
            if (activatorSkillSlot == skillLocator.special)
            {
                return base.inputBank.skill4.justReleased;
            }
            if(activatorSkillSlot == skillLocator.secondary)
            {
                return base.inputBank.skill2.justReleased;
            }
            return false;
        }

        public override void Update() {
            base.Update();
            _cannonSpin = Mathf.Lerp(0, 2.2f, fixedAge/duration);
            _animator.SetFloat("CannonSpin", _cannonSpin, 0.1f, Time.deltaTime);  //bit of a magic number because animation for some reason resets at 1.0
            _animator.SetFloat("CannonBarCharge", Mathf.Min(fixedAge / duration, 0.99f), 0.1f, Time.deltaTime);
        }

        private void GiveBarrierPerEnemy() {
             
            List<TeamComponent> enemies = GatherEnemies(teamComponent.teamIndex);
            int nearbyEnemies = 0;
            for (int i = 0; i < enemies.Count; i++) {

                if (Vector3.SqrMagnitude(enemies[i].transform.position - transform.position) < SqrBarrierRange) {
                    nearbyEnemies++;
                }
            }

            float barrierGained = nearbyEnemies * BarrierPercentPerEnemy * healthComponent.fullBarrier;
            float targetBarrier = Mathf.Min(barrierGained + healthComponent.barrier, healthComponent.fullBarrier * MaxBarrierPercent);

            healthComponent.AddBarrierAuthority(Mathf.Max(targetBarrier - healthComponent.barrier, 0));
        }

        //credit to tiler2 https://github.com/ThinkInvis/RoR2-TILER2/blob/3e2a6d4105417de06abb3ef3f85da844170abf8a/StaticModules/MiscUtil.cs#L455
        /// <summary>
        /// Returns a list of enemy TeamComponents given an ally team (to ignore while friendly fire is off) and a list of ignored teams (to ignore under all circumstances).
        /// </summary>
        /// <param name="allyIndex">The team to ignore if friendly fire is off.</param>
        /// <param name="ignore">Additional teams to always ignore.</param>
        /// <returns>A list of all TeamComponents that match the provided team constraints.</returns>
        public static List<TeamComponent> GatherEnemies(TeamIndex allyIndex, params TeamIndex[] ignore)
        {
            var retv = new List<TeamComponent>();
            bool isFF = FriendlyFireManager.friendlyFireMode != FriendlyFireManager.FriendlyFireMode.Off;
            var scan = ((TeamIndex[])Enum.GetValues(typeof(TeamIndex))).Except(ignore);
            foreach (var ind in scan)
            {
                if (isFF || allyIndex != ind)
                    retv.AddRange(TeamComponent.GetTeamMembers(ind));
            }
            return retv;
        }

        protected void DropRadiationProjectile() {
            FireProjectileInfo fireProjectileInfo = new FireProjectileInfo {
                projectilePrefab = deployProjectilePrefab,
                crit = base.RollCrit(),
                force = 0f,
                damage = this.damageStat * DamageCoefficient,
                owner = base.gameObject,
                rotation = Quaternion.identity,
                position = base.characterBody.corePosition,
                //damageTypeOverride = DamageType.WeakOnHit
            };

            ModifyProjectile(ref fireProjectileInfo);

            ProjectileManager.instance.FireProjectile(fireProjectileInfo);
        }

        protected virtual void ModifyProjectile(ref FireProjectileInfo fireProjectileInfo) { }

        protected override void SetNextState()
        {
            _complete = true;
            DeployIrradiate state = new DeployIrradiate { aimRequest = this.aimRequest, activatorSkillSlot = activatorSkillSlot };
            outer.SetNextState(state);
        }

        public override void OnExit() {
            base.OnExit();

            characterBody.hideCrosshair = false;

            if (!_complete) {
                aimRequest?.Dispose();

                activatorSkillSlot.UnsetSkillOverride(gameObject, DesolatorSurvivor.cancelDeploySkillDef, RoR2.GenericSkill.SkillOverridePriority.Contextual);

                PlayCrossfade("FullBody, Override", "BufferEmpty", 0.5f);

                PlayCrossfade("RadCannonBar", "DesolatorIdlePose", 0.1f);
                PlayCrossfade("RadCannonSpin", "DesolatorIdlePose", 0.1f);
            }

            if (NetworkServer.active) {

                characterBody.AddTimedBuff(DesolatorBuffs.desolatorDeployBuff, 0.2f);
                characterBody.RemoveBuff(DesolatorBuffs.desolatorDeployBuff);
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.PrioritySkill;
        }
    }
}
