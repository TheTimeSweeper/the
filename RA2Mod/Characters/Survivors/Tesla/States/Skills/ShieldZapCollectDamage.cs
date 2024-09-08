using EntityStates;
using RA2Mod.Survivors.Tesla;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RA2Mod.Survivors.Tesla.States
{
    public class ShieldZapCollectDamage : BaseSkillState
    {
        public static float ShieldBuffDuration = 4;

        public float skillsPlusSeconds = 0;

        public CameraTargetParams.AimRequest aimRequest;

        private float blockedDamage = 0;

        private bool completed;
        private bool buffDetectedAtSomePoint;
        private TemporaryOverlayInstance temporaryOverlay;

        public override void OnEnter()
        {
            base.OnEnter();

            EntityStateMachine.FindByCustomName(gameObject, "Weapon").SetNextState(new ShieldZapStart());

            TeslaZapBarrierController controller = GetComponent<TeslaZapBarrierController>();
            if (controller)
            {
                controller.StartRecordingDamage();
            }

            aimRequest = cameraTargetParams.RequestAimType(CameraTargetParams.AimType.Aura);

            if (!characterBody.HasBuff(TeslaBuffs.zapShieldBuff))
            {
                CharacterModel characterModel = GetModelTransform().GetComponent<CharacterModel>();

                temporaryOverlay = TemporaryOverlayManager.AddOverlay(gameObject);
                temporaryOverlay.duration = ShieldBuffDuration + 1;
                temporaryOverlay.originalMaterial = LegacyResourcesAPI.Load<Material>("Materials/matIsShocked");
                temporaryOverlay.AddToCharacterModel(characterModel);
            }

            if (NetworkServer.active)
            {

                Util.CleanseBody(characterBody, true, false, false, true, true, false);
                characterBody.AddTimedBuff(TeslaBuffs.zapShieldBuff, ShieldBuffDuration + skillsPlusSeconds);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!buffDetectedAtSomePoint)
            {
                buffDetectedAtSomePoint |= characterBody.HasBuff(TeslaBuffs.zapShieldBuff);
                return;
            }

            //UGLY HACK: client takes a sec to realize host has given the body a buff up in onEnter
            if (buffDetectedAtSomePoint && !characterBody.HasBuff(TeslaBuffs.zapShieldBuff))
            {
                ShieldZapReleaseDamage newNextState = new ShieldZapReleaseDamage()
                {
                    aimRequest = aimRequest,
                    collectedDamage = blockedDamage,
                    temporaryOverlay = temporaryOverlay,
                };
                /*EntityStateMachine.FindByCustomName(gameObject, "Weapon")*/
                outer.SetNextState(newNextState);
                completed = true;
                //base.outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            
            if (!completed)
            {
                if (aimRequest != null)
                    aimRequest.Dispose();
            }
        }
    }
}