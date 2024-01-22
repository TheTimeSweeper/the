using EntityStates;
using R2API;
using RA2Mod.Survivors.Chrono.SkillDefs;
using RA2Mod.Survivors.Chrono.Components;
using RoR2;
using UnityEngine;

namespace RA2Mod.Survivors.Chrono.SkillStates
{
    public class Vanish : BaseSkillState, IHasSkillDefComponent<ChronoTrackerVanish>
    {
        public static float damageCoefficient = 0.1f;
        public static float procCoefficient = 1f;
        public static float baseDuration = 3f;

        public static float tickInterval = 0.2f;

        public static GameObject tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");

        private float duration;

        public ChronoTrackerVanish componentFromSkillDef { get; set; }

        private DamageInfo damageInfo;
        private HurtBox targetHurtBox;
        private Transform muzzleTransform;

        private ChronoTether vanishTether;

        private float nextInterval;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            characterBody.SetAimTimer(2f);

            Util.PlaySound("Play_ChronoAttackShort", gameObject);

            muzzleTransform = GetModelChildLocator().FindChild("HandR");
            if (muzzleTransform == null)
                muzzleTransform = transform;

            PlayAnimation("Arms, Override", "cast 2", "cast.playbackRate", duration);

            targetHurtBox = componentFromSkillDef.GetTrackingTarget();

            vanishTether = Object.Instantiate(ChronoAssets.chronoVanishTether);

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
                procCoefficient = 0f,
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

            vanishTether.Dispose();
        }

        public override void Update()
        {
            base.Update();
            vanishTether.transform.position = muzzleTransform.position;
            if (targetHurtBox)
            {
                vanishTether.SetTetherPoint(targetHurtBox.transform.position);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (fixedAge >= duration && isAuthority || targetHurtBox == null || !targetHurtBox.healthComponent.alive)
            {
                outer.SetNextStateToMain();
                return;
            }

            while (fixedAge >= nextInterval)
            {
                nextInterval += tickInterval;
                DoDamage();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}