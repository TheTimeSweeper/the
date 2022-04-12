using EntityStates;
using ModdedEntityStates.BaseStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace ModdedEntityStates.TeslaTrooper {
    public class ShieldZap : BaseTimedSkillState
    {
        public static float ShieldBuffDuration = 4;
        public static float ReturnDamageCoefficient = 1; 
        public static float ReturnDamageProcCoefficient = 0.2f;

        public static float BaseDuration = 1;

        public static float BaseCastStartTime = 0.0f; //todo: anim: 0.13 when legs are separated
        public static float MoveSlowEndTime = 0.8f;

        public GameObject ShieldEffect = Modules.Assets.LoadAsset<GameObject>("prefabs/effects/impacteffects/simplelightningstrikeimpact");

        RoR2.CameraTargetParams.AimRequest aimRequest;

        public override void OnEnter() {
            base.OnEnter();
            InitDurationValues(BaseDuration, BaseCastStartTime, MoveSlowEndTime);

            ZapBarrierController controller = GetComponent<ZapBarrierController>();
            if (controller) {
                controller.StartRecordingDamage();
            }

            //todo: lingering gesture, interruptible legs
                //he's running in place what
            base.PlayCrossfade("FullBody, Override", "CastShield", "CastShield.playbackRate", duration, 0.1f * duration);
            base.PlayCrossfade("Gesture, Override", "CastShield", "CastShield.playbackRate", duration, 0.1f * duration);

            aimRequest = cameraTargetParams.RequestAimType(RoR2.CameraTargetParams.AimType.Aura);

            if (!base.characterBody.HasBuff(Modules.Buffs.zapShieldBuff)) {
                CharacterModel component = base.GetModelTransform().GetComponent<CharacterModel>();

                TemporaryOverlay temporaryOverlay = base.gameObject.AddComponent<TemporaryOverlay>();
                temporaryOverlay.duration = ShieldBuffDuration + 1;
                temporaryOverlay.originalMaterial = LegacyResourcesAPI.Load<Material>("Materials/matIsShocked");
                temporaryOverlay.AddToCharacerModel(component);
            }

            if (NetworkServer.active) {

                base.characterBody.AddTimedBuff(Modules.Buffs.zapShieldBuff, ShieldBuffDuration);
            }
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
            if (base.characterMotor && isFiring)
            {
                base.characterMotor.moveDirection = Vector3.zero;
            }
        }

        public override void OnExit() {
            base.OnExit();

            EntityStateMachine.FindByCustomName(gameObject, "Slide").SetNextState(new ShieldZapCollectDamage() { aimRequest = this.aimRequest });
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return hasFired && isFiring ? InterruptPriority.PrioritySkill : InterruptPriority.Skill;
        }
    }
}