using EntityStates;
using R2API;
using RA2Mod.Survivors.Chrono.Components;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using RA2Mod.General.SkillDefs;

namespace RA2Mod.Survivors.Chrono.States
{
    public class Vanish : BaseSkillState, IHasSkillDefComponent<ChronoTrackerVanish>
    {
        public virtual float damageCoefficient => ChronoConfig.M4_Vanish_TickDamage.Value;
        public static float procCoefficient = 1;
        public virtual float baseDuration => ChronoConfig.M4_Vanish_Duration.Value;
        
        public virtual float baseTickInterval => ChronoConfig.M4_Vanish_TickInterval.Value;

        private float duration;
        private float tickInterval;

        private float nextInterval;

        public ChronoTrackerVanish componentFromSkillDef1 { get; set; }

        private DamageInfo damageInfo;
        private HurtBox targetHurtBox;
        protected bool targetingAlly;
        protected Transform muzzleTransform;
        
        private ChronoTether vanishTether;
        private SetStateOnHurt setStateOnHurt;
        private bool rolledCrit;
        private uint soundID;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            tickInterval = baseTickInterval / attackSpeedStat;
            StartAimMode(4);

            soundID = Util.PlaySound("Play_ChronoAttackShort", gameObject);

            muzzleTransform = FindModelChild("Muzzle");
            if (muzzleTransform == null)
                muzzleTransform = transform;

            PlayShootAnimation();

            if (isAuthority)
            {
                targetHurtBox = componentFromSkillDef1.GetTrackingTarget();
                targetingAlly = componentFromSkillDef1.GetIsAlly();
            }
            vanishTether = Object.Instantiate(ChronoAssets.chronoVanishTether);

            SetTetherPoints();

            if (targetingAlly)
            {
                if (NetworkServer.active)
                {
                    targetHurtBox.healthComponent.body.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, duration);
                }
                return;
            }

            if (Util.HasEffectiveAuthority(targetHurtBox.healthComponent.gameObject))
            {
                TryFreeze(targetHurtBox.healthComponent.body);
            }
            rolledCrit = base.RollCrit();
        }

        private void ResetDamageInfo()
        {
            damageInfo = new DamageInfo
            {
                position = targetHurtBox.transform.position,
                attacker = gameObject,
                inflictor = gameObject,
                damage = damageCoefficient * this.damageStat,
                damageColorIndex = DamageColorIndex.Default,
                damageType = DamageType.Generic,
                crit = rolledCrit,
                force = Vector3.zero,
                procChainMask = default(ProcChainMask),
                procCoefficient = procCoefficient,
            };
            damageInfo.AddModdedDamageType(ChronoDamageTypes.chronoDamagePierce);
            damageInfo.AddModdedDamageType(ChronoDamageTypes.vanishingDamage);
        }

        private void TryFreeze(CharacterBody body)
        {
            //if(body.TryGetComponent(out SetStateOnHurt setStateOnHurt))
            //{
            //    if (setStateOnHurt.targetStateMachine)
            //    {
            //        FrozenState frozenState = new FrozenState();
            //        frozenState.freezeDuration = duration;
            //        setStateOnHurt.targetStateMachine.SetInterruptState(frozenState, InterruptPriority.Frozen);
            //    }
            //    EntityStateMachine[] array = setStateOnHurt.idleStateMachine;
            //    for (int i = 0; i < array.Length; i++)
            //    {
            //        array[i].SetNextState(new Idle());
            //    }
            //}
        }

        protected virtual void PlayShootAnimation()
        {
            PlayAnimation("Arms, Override", "cast 2", "cast.playbackRate", duration);
        }

        public virtual void DoDamage()
        {
            //create a new damgeinfo every time in case it gets rejected initially
            ResetDamageInfo();

            targetHurtBox.healthComponent.TakeDamage(damageInfo);

            GlobalEventManager.instance.OnHitEnemy(damageInfo, targetHurtBox.healthComponent.gameObject);
            GlobalEventManager.instance.OnHitAll(damageInfo, targetHurtBox.healthComponent.gameObject);
        }

        public override void OnExit()
        {
            base.OnExit();

            vanishTether?.Dispose();
            Util.PlaySound("Stop_TetherLoop", gameObject);
        }

        public override void Update()
        {
            base.Update();

            SetTetherPoints();
        }

        protected virtual void SetTetherPoints()
        {
            if (!vanishTether)
                return;
            vanishTether.transform.position = muzzleTransform.position;
            if (targetHurtBox)
            {
                vanishTether.SetTetherPoint(targetHurtBox.transform.position);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (fixedAge >= duration || targetHurtBox == null || !targetHurtBox.healthComponent.alive)
            {
                if (isAuthority)
                {
                    outer.SetNextStateToMain();
                }
                return;
            }
            if (!targetingAlly)
            {
                while (fixedAge >= nextInterval)
                {
                    nextInterval += tickInterval;

                    if (NetworkServer.active)
                    {
                        DoDamage();
                    }
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

        // Token: 0x06000E64 RID: 3684 RVA: 0x0003E1A0 File Offset: 0x0003C3A0
        public override void OnSerialize(NetworkWriter writer)
        {
            writer.Write(HurtBoxReference.FromHurtBox(this.targetHurtBox));
            writer.Write(targetingAlly);
        }

        // Token: 0x06000E65 RID: 3685 RVA: 0x0003E1B4 File Offset: 0x0003C3B4
        public override void OnDeserialize(NetworkReader reader)
        {
            this.targetHurtBox = reader.ReadHurtBoxReference().ResolveHurtBox();
            targetingAlly = reader.ReadBoolean();
        }
    }
}