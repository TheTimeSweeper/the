using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace PlagueMod.Survivors.Plague.SkillStates
{
    public class SimpleBlastJump : BaseBlastJump
    {
        // Token: 0x04000DDA RID: 3546
        public static float blastAttackRadius = 8;

        // Token: 0x04000DDB RID: 3547
        public static float blastAttackDamageCoefficient = 3;

        // Token: 0x04000DDC RID: 3548
        public static float blastAttackProcCoefficient = 1;

        // Token: 0x04000DDD RID: 3549
        public static float blastAttackForce;

        public static GameObject effectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OmniEffect/OmniExplosionVFXGreaterWisp");

        public override void OnEnter()
        {
            base.OnEnter();

            if (NetworkServer.active)
            {
                BlastAttack blastAttack = new BlastAttack();
                blastAttack.radius = blastAttackRadius;
                blastAttack.procCoefficient = blastAttackProcCoefficient;
                blastAttack.position = this.blastPosition;
                blastAttack.attacker = base.gameObject;
                blastAttack.crit = Util.CheckRoll(base.characterBody.crit, base.characterBody.master);
                blastAttack.baseDamage = base.characterBody.damage * blastAttackDamageCoefficient;
                blastAttack.falloffModel = BlastAttack.FalloffModel.None;
                blastAttack.baseForce = blastAttackForce;
                blastAttack.teamIndex = TeamComponent.GetObjectTeam(blastAttack.attacker);
                blastAttack.damageType = DamageType.Stun1s;
                blastAttack.attackerFiltering = AttackerFiltering.NeverHitSelf;
                blastAttack.Fire();
            }

            EffectManager.SpawnEffect(
                effectPrefab,
                new EffectData()
                    {
                        origin = transform.position,
                        scale = blastAttackRadius,
                    }, 
                false);
        }
    }
}