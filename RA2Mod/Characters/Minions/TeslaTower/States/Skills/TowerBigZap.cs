using RoR2;
using UnityEngine;
using R2API;
using UnityEngine.Networking;
using System;
using RA2Mod.Survivors.Tesla.States;
using RA2Mod.Survivors.Tesla;

namespace RA2Mod.Minions.TeslaTower.States
{

    public class TowerBigZap : TowerZap
    {
        public static event Action<GameObject> onTowerBigZapMultiHit;

        new public static float DamageCoefficient = 15.0f;
        new public static float ProcCoefficient = 1f;
        new public static float BaseDuration = 0.6f;
        public static float BaseAttackRadius = 16;

        public float secondarySkillsPlusAreaMulti = 1f;
        public float secondarySkillsPlusDamageMulti = 1f;
        public float damageCoefficient;
        public float attackRadius;

        private string towerZapSound = "Play_tower_bparatta_they_fucking_turned_eiffel_tower_to_a_tesla_tower";

        public override void OnEnter()
        {

            base.OnEnter();

            damageCoefficient = DamageCoefficient;
            attackRadius = BaseAttackRadius * secondarySkillsPlusAreaMulti;
        }

        protected override float GetBaseDuration()
        {
            return BaseDuration;
        }

        protected override void ModifySound()
        {

            zapSound = towerZapSound;
            zapSoundCrit = towerZapSound;
        }

        protected override void fireOrb()
        {

            if (lightningTarget == null)
                return;

            lightningOrb.damageValue = 0;
            lightningOrb.procCoefficient = 0;
            lightningOrb.damageType = DamageType.Silent;
            base.fireOrb();

            bool crit = RollCrit();

            if (NetworkServer.active)
            {

                Vector3 targetPoint = lightningTarget.transform.position;

                BlastAttack blast = new BlastAttack
                {
                    attacker = gameObject,
                    inflictor = gameObject,
                    teamIndex = teamComponent.teamIndex,
                    //attackerFiltering = AttackerFiltering.NeverHit

                    position = targetPoint,
                    radius = attackRadius,
                    falloffModel = BlastAttack.FalloffModel.None,

                    baseDamage = damageStat * damageCoefficient * secondarySkillsPlusDamageMulti,
                    crit = crit,
                    damageType = DamageType.Shock5s,
                    damageColorIndex = DamageColorIndex.WeakPoint,

                    procCoefficient = ProcCoefficient,
                    //procChainMask = 
                    //losType = BlastAttack.LoSType.NearestHit,

                    baseForce = -5, //enfucker void grenade here we gos
                    //bonusForce = ;a

                    //impactEffect = EffectIndex.uh;
                };
                BlastAttack.Result blastResult = blast.Fire();
                if (blastResult.hitCount >= 10)
                {
                    onTowerBigZapMultiHit?.Invoke(gameObject);
                }

                #region effects
                EffectData fect = new EffectData
                {
                    origin = targetPoint,
                    scale = attackRadius,
                };

                PlayBlastEffect(fect);
                #endregion effects
            }
        }

        protected virtual void PlayBlastEffect(EffectData fect)
        {
            EffectManager.SpawnEffect(BigZap.bigZapEffectPrefabArea, fect, true);

            EffectManager.SpawnEffect(BigZap.bigZapEffectPrefab, fect, true);

            fect.scale /= 2f;
            EffectManager.SpawnEffect(BigZap.bigZapEffectFlashPrefab, fect, true);
        }

        public override void OnExit()
        {
            base.OnExit();

            if (!hasFired)
            {
                OnCastEnter();
                hasFired = true;
            }

            if (TeslaConfig.M4_Tower_Targeting.Value)
            {
                GetComponent<TowerOwnerTrackerComponent>()?.OwnerTrackerComponent?.SetTowerLockedTarget(null);
            }
        }
    }

}
