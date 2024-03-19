using RA2Mod.Survivors.Tesla.States;
using RoR2;

namespace RA2Mod.Minions.TeslaTower.States
{
    public class TowerBigZapGauntlet : TowerBigZap
    {

        new public static float DamageCoefficient = 4f;
        new public static float BaseAttackRadius = 4;

        new public static float BaseDuration = 0.0f;

        public override void OnEnter()
        {

            base.OnEnter();

            damageCoefficient = DamageCoefficient;
            attackRadius = BaseAttackRadius;
        }

        protected override float GetBaseDuration()
        {
            return BaseDuration;
        }

        protected override void ModifySound()
        {

            zapSound = ZapSound;
            zapSoundCrit = ZapSound;
        }

        protected override void PlayPrep() { }

        protected override void PlayBlastEffect(EffectData fect)
        {

            EffectManager.SpawnEffect(BigZap.bigZapEffectPrefabArea, fect, true);
            //if (!UnityEngine.Input.GetKey(UnityEngine.KeyCode.G))
            //EffectManager.SpawnEffect(BigZap.bigZapEffectPrefab, fect, true);

        }
    }

}
