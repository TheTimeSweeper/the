using EntityStates;
using R2API;
using RA2Mod.Survivors.Chrono.SkillDefs;
using RA2Mod.Survivors.Chrono.Components;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RA2Mod.Survivors.Chrono.SkillStates
{
    public class Vanish : BaseSkillState, IHasSkillDefComponent<ChronoTrackerVanish>
    {
        public static float damageCoefficient => ChronoConfig.M4Damage.Value;
        public static float procCoefficient = 0.5f;
        public static float baseDuration => ChronoConfig.M4Duration.Value;
        
        public static float baseTickInterval => ChronoConfig.M4Interval.Value;

        private float duration;
        private float tickInterval;

        private float nextInterval;

        public ChronoTrackerVanish componentFromSkillDef { get; set; }

        private DamageInfo damageInfo;
        private HurtBox targetHurtBox;
        private Transform muzzleTransform;
        
        private ChronoTether vanishTether;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            tickInterval = baseTickInterval / attackSpeedStat;
            characterBody.SetAimTimer(2f);

            Util.PlaySound("Play_ChronoAttackShort", gameObject);

            muzzleTransform = GetModelChildLocator().FindChild("HandR");
            if (muzzleTransform == null)
                muzzleTransform = transform;

            PlayAnimation("Arms, Override", "cast 2", "cast.playbackRate", duration);
            if (isAuthority)
            {
                targetHurtBox = componentFromSkillDef.GetTrackingTarget();
            }
            vanishTether = Object.Instantiate(ChronoAssets.chronoVanishTether);
            Log.Warning("tether "+ vanishTether != null);

            damageInfo = new DamageInfo
            {
                //position = this.targetRoot.transform.position,
                attacker = gameObject,
                inflictor = gameObject,
                damage = damageCoefficient * this.damageStat,
                damageColorIndex = DamageColorIndex.Default,
                damageType = DamageType.Generic,
                crit = base.RollCrit(),
                force = Vector3.zero,
                procChainMask = default(ProcChainMask),
                procCoefficient = procCoefficient,
            };
            damageInfo.AddModdedDamageType(ChronoDamageTypes.chronoDamage);
            damageInfo.AddModdedDamageType(ChronoDamageTypes.vanishingDamage);
        }

        public void DoDamage()
        {
            damageInfo.position = targetHurtBox.transform.position;

            targetHurtBox.healthComponent.TakeDamage(damageInfo);
        }

        public override void OnExit()
        {
            base.OnExit();

            vanishTether?.Dispose();
        }

        public override void Update()
        {
            base.Update();
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


            while (fixedAge >= nextInterval)
            {
                nextInterval += tickInterval;

                if (NetworkServer.active)
                {
                    DoDamage();
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
        }

        // Token: 0x06000E65 RID: 3685 RVA: 0x0003E1B4 File Offset: 0x0003C3B4
        public override void OnDeserialize(NetworkReader reader)
        {
            this.targetHurtBox = reader.ReadHurtBoxReference().ResolveHurtBox();
        }
    }
}