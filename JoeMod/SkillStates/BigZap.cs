using EntityStates;
using RoR2;
using UnityEngine;

namespace HenryMod.ModdedEntityStates.Joe
{
    public class BigZap : BaseSkillState
    {
        public static float DamageCoefficient = 4.2f;
        public static float ProcCoefficient = 1f;
        public static float AttackRadius = 7;

        public Vector3 aimPoint;

        public GameObject bigZapEffectPrefab = Resources.Load<GameObject>("prefabs/effects/magelightningbombexplosion");

        public override void OnEnter()
        {
            base.OnEnter();

            new BlastAttack
            {
                attacker = base.gameObject,
                inflictor = base.gameObject,
                teamIndex = TeamComponent.GetObjectTeam(base.gameObject),
                //attackerFiltering = AttackerFiltering.NeverHit

                position = aimPoint,
                radius = AttackRadius,
                falloffModel = BlastAttack.FalloffModel.None,

                baseDamage = this.damageStat * DamageCoefficient,
                crit = base.RollCrit(),
                damageType = DamageType.Stun1s,
                //damageColorIndex = DamageColorIndex.Default,

                procCoefficient = 1,
                //procChainMask = 
                //losType = BlastAttack.LoSType.NearestHit,

                //baseForce = this.blastAttackForce,
                //bonusForce = ;

                //impactEffect = EffectIndex.uh;

            }.Fire();
            EffectData fect = new EffectData
            {
                origin = aimPoint,
                scale = AttackRadius,
            };
            EffectManager.SpawnEffect(bigZapEffectPrefab, fect, true);
        }
    }
}