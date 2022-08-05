using RoR2;
using UnityEngine;
using R2API;
using UnityEngine.Networking;

namespace ModdedEntityStates.TeslaTrooper.Tower {
    public class TowerBigZap: TowerZap {

        new public static float DamageCoefficient = 12.0f;
        new public static float ProcCoefficient = 1f;
        new public static float BaseDuration = 0.6f;
        public static float BaseAttackRadius = 15;

        public float secondarySkillsPlusAreaMulti = 1f;
        public float secondarySkillsPlusDamageMulti = 1f;
        public float attackRadius;

        private string towerZapSound = "Play_tower_bparatta_they_fucking_turned_eiffel_tower_to_a_tesla_tower";

        protected override void InitDurationValues(float baseDuration, float baseCastStartTime, float baseCastEndTime = 1) {

            base.InitDurationValues(BaseDuration, BaseStartCastTime);

            attackRadius = BaseAttackRadius * secondarySkillsPlusAreaMulti;
        }
        
        protected override void OnCastEnter() {

            zapSound = towerZapSound;
            zapSoundCrit = towerZapSound;

            base.OnCastEnter();
        }

        protected override void fireOrb() {

            if (lightningTarget == null)
                return;

            lightningOrb.damageValue = 0;
            lightningOrb.damageType = DamageType.Silent;
            base.fireOrb();
            
            bool crit = RollCrit();

            if (NetworkServer.active) {

                Vector3 targetPoint = lightningTarget.transform.position;

                BlastAttack blast = new BlastAttack {
                    attacker = gameObject,
                    inflictor = gameObject,
                    teamIndex = teamComponent.teamIndex,
                    //attackerFiltering = AttackerFiltering.NeverHit

                    position = targetPoint,
                    radius = attackRadius,
                    falloffModel = BlastAttack.FalloffModel.None,

                    baseDamage = damageStat * DamageCoefficient * secondarySkillsPlusDamageMulti,
                    crit = crit,
                    damageType = DamageType.Shock5s,
                    //damageColorIndex = DamageColorIndex.WeakPoint,
                    
                    procCoefficient = ProcCoefficient,
                    //procChainMask = 
                    //losType = BlastAttack.LoSType.NearestHit,

                    baseForce = -5, //enfucker void grenade here we go
                    //bonusForce = ;

                    //impactEffect = EffectIndex.uh;
                };

                if (FacelessJoePlugin.conductiveMechanic && FacelessJoePlugin.conductiveEnemy) {
                    blast.AddModdedDamageType(Modules.DamageTypes.consumeConductive);
                }
                blast.Fire();

                #region effects
                EffectData fect = new EffectData {
                    origin = targetPoint,
                    scale = attackRadius,
                };

                EffectManager.SpawnEffect(BigZap.bigZapEffectPrefabArea, fect, true);

                EffectManager.SpawnEffect(BigZap.bigZapEffectPrefab, fect, true);

                fect.scale /= 2f;
                EffectManager.SpawnEffect(BigZap.bigZapEffectFlashPrefab, fect, true);
                #endregion effects
            }
        }
    }

}
