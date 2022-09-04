using EntityStates;
using ModdedEntityStates.BaseStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace ModdedEntityStates.Desolator {
    public class RadiationAuraStart : BaseTimedSkillState {

        public static float BaseDuration = 1;

        public static float BaseCastStartTime = 0.0f;

        public override void OnEnter() {
            base.OnEnter();
            InitDurationValues(BaseDuration, BaseCastStartTime);

            //todo: lingering gesture, interruptible legs
            //he's running in place what
            base.PlayCrossfade("FullBody, Override", "CastShield", "CastShield.playbackRate", duration, 0.1f * duration);
            base.PlayCrossfade("Gesture, Override", "CastShield", "CastShield.playbackRate", duration, 0.1f * duration);
        }

    }
}

namespace ModdedEntityStates.TeslaTrooper {
    public class ShieldZapStart : BaseTimedSkillState
    {
        public static float BaseDuration = 1;

        public static float BaseCastStartTime = 0.0f; //todo: anim: 0.13 when legs are separated
        public static float MoveSlowEndTime = 0.6f;

        public GameObject CastShieldEffect = Modules.Assets.LoadAsset<GameObject>("prefabs/effects/impacteffects/simplelightningstrikeimpact");
        
        public override void OnEnter() {
            base.OnEnter();
            InitDurationValues(BaseDuration, BaseCastStartTime, MoveSlowEndTime);

            //todo: lingering gesture, interruptible legs
                //he's running in place what
            base.PlayCrossfade("FullBody, Override", "CastShield", "CastShield.playbackRate", duration, 0.1f * duration);
            base.PlayCrossfade("Gesture, Override", "CastShield", "CastShield.playbackRate", duration, 0.1f * duration);
        }

        protected override void OnCastEnter()
        {
            EffectManager.SpawnEffect(CastShieldEffect, new EffectData
            {
                origin =  Modules.VRCompat.GetModelChildLocator(this).FindChild("MuzzleGauntlet").position,
            }, false);
        }

        public override void Update()
        {
            base.Update();
            if (base.characterMotor && isFiring)
            {
                base.characterMotor.moveDirection = Vector3.zero;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return hasFired && isFiring ? InterruptPriority.PrioritySkill : InterruptPriority.Skill;
        }
    }
}