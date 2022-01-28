using EntityStates;
using HenryMod.ModdedEntityStates.BaseStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace JoeMod.ModdedEntityStates.TeslaTrooper
{
    public class ShieldZap : BaseTimedSkillState
    {

        public static float ShieldBuffDuration = 4;
        //todo: damage return
        public static float ReturnDamageCoefficient = 1; //todo: base damage or total damage?
        public static float ReturnDamageProcCoefficient = 0.2f;

        public static float BaseDuration = 1;

        public static float BaseCastStartTime = 0.13f;
        public static float MoveSlowEndTime = 0.8f;

        public GameObject ShieldEffect = Resources.Load<GameObject>("prefabs/effects/impacteffects/simplelightningstrikeimpact");

        public override void OnEnter()
        {
            base.OnEnter();
            InitDurationValues(BaseDuration, BaseCastStartTime, MoveSlowEndTime);

            if (NetworkServer.active)
            {                                 //todo: custom buff, movesped, shieldy visual effect
                                              //todo: damage return     
                base.characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, ShieldBuffDuration);
            }

            //todo: lingering gesture, interruptible legs
            base.PlayCrossfade("FullBody, Override", "CastShield", "CastShield.playbackRate", duration, 0.1f*duration);


            //todo: blastattack on start?
        }

        protected override void OnCastEnter()
        {
            EffectManager.SpawnEffect(ShieldEffect, new EffectData
            {
                origin = base.GetModelChildLocator().FindChild("MuzzleGauntlet").position,
            }, false);
        }

        public override void Update()
        {
            base.Update();
            if (base.characterMotor)
            {
                base.characterMotor.moveDirection = Vector3.zero;
            }
        }
    }
}