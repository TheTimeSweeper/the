using EntityStates;
using RA2Mod.General;
using RA2Mod.Modules.BaseStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RA2Mod.Survivors.Tesla.States
{
    public class ShieldZapStart : BaseTimedSkillState
    {
        public override float TimedBaseDuration => BaseDuration;
        public override float TimedBaseCastStartPercentTime => BaseCastStartTime;
        public override float TimedBaseCastEndPercentTime => MoveSlowEndTime;

        public static float BaseDuration = 1;

        public static float BaseCastStartTime = 0.0f; //todo: anim: 0.13 when legs are separated
        public static float MoveSlowEndTime = 0.6f;

        public GameObject CastShieldEffect = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/simplelightningstrikeimpact");

        public override void OnEnter()
        {
            base.OnEnter();

            //todo: lingering gesture, interruptible legs
            //he's running in place what
            PlayCrossfade("FullBody, Override", "CastShield", "CastShield.playbackRate", duration, 0.1f * duration);
            PlayCrossfade("Gesture, Override", "CastShield", "CastShield.playbackRate", duration, 0.1f * duration);
        }

        protected override void OnCastEnter()
        {
            EffectManager.SpawnEffect(CastShieldEffect, new EffectData
            {
                origin = this.GetModelChildLocator(true).FindChild("MuzzleGauntlet").position,
            }, false);
        }

        public override void Update()
        {
            base.Update();
            if (characterMotor && isFiring)
            {
                characterMotor.moveDirection = Vector3.zero;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return hasFired && isFiring ? InterruptPriority.PrioritySkill : InterruptPriority.Skill;
        }
    }
}