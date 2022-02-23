using RoR2;
using UnityEngine;

namespace JoeMod.ModdedEntityStates.TeslaTrooper.Tower {
    public class TowerBigZap: TowerZap {

        new public static float DamageCoefficient = 12.0f;
        new public static float ProcCoefficient = 1f;
        new public static float BaseDuration = 0.3f;
        public static float AttackRadius = 15;

        protected override void InitDurationValues(float baseDuration, float baseCastStartTime, float baseCastEndTime = 1) {

            base.InitDurationValues(BaseDuration, BaseStartCastTime);
        }

        protected override void fireOrb() {

            lightningOrb.damageValue = 0;
            base.fireOrb();
            
            Vector3 targetPoint = lightningTarget.transform.position;

            new BlastAttack {
                attacker = gameObject,
                inflictor = gameObject,
                teamIndex = teamComponent.teamIndex,
                //attackerFiltering = AttackerFiltering.NeverHit

                position = targetPoint,
                radius = AttackRadius,
                falloffModel = BlastAttack.FalloffModel.None,

                baseDamage = damageStat * DamageCoefficient,
                crit = RollCrit(),
                damageType = DamageType.Shock5s,
                damageColorIndex = DamageColorIndex.WeakPoint,

                procCoefficient = ProcCoefficient,
                //procChainMask = 
                //losType = BlastAttack.LoSType.NearestHit,

                baseForce = -5, //enfucker void grenade here we go
                //bonusForce = ;

                //impactEffect = EffectIndex.uh;
            }.Fire();

            #region effects
            EffectData fect = new EffectData {
                origin = targetPoint,
                scale = AttackRadius,
            };

            EffectManager.SpawnEffect(BigZap.bigZapEffectPrefabArea, fect, true);

            EffectManager.SpawnEffect(BigZap.bigZapEffectPrefab, fect, true);

            fect.scale /= 2f;
            EffectManager.SpawnEffect(BigZap.bigZapEffectFlashPrefab, fect, true);
            #endregion effects
        }
    }

}
