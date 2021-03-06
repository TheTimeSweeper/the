using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace ModdedEntityStates.TeslaTrooper {
    public class ShieldZapCollectDamage : BaseSkillState {

        public static float ShieldBuffDuration = 4;

        public float skillsPlusSeconds = 0;

        public RoR2.CameraTargetParams.AimRequest aimRequest;

        private float blockedDamage = 0;

        private bool completed;

        public override void OnEnter() {
            base.OnEnter();

            EntityStateMachine.FindByCustomName(gameObject, "Weapon").SetNextState(new ShieldZapStart());

            ZapBarrierController controller = GetComponent<ZapBarrierController>();
            if (controller) {
                controller.StartRecordingDamage();
            }

            aimRequest = cameraTargetParams.RequestAimType(RoR2.CameraTargetParams.AimType.Aura);

            if (!base.characterBody.HasBuff(Modules.Buffs.zapShieldBuff)) {
                CharacterModel component = base.GetModelTransform().GetComponent<CharacterModel>();

                TemporaryOverlay temporaryOverlay = base.gameObject.AddComponent<TemporaryOverlay>();
                temporaryOverlay.duration = ShieldBuffDuration + 1;
                temporaryOverlay.originalMaterial = LegacyResourcesAPI.Load<Material>("Materials/matIsShocked");
                temporaryOverlay.AddToCharacerModel(component);
            }

            if (NetworkServer.active) {

                Util.CleanseBody(base.characterBody, true, false, false, true, true, false);
                base.characterBody.AddTimedBuff(Modules.Buffs.zapShieldBuff, ShieldBuffDuration + skillsPlusSeconds);
            }
        }

        public override void FixedUpdate() {
            base.FixedUpdate();

            //todo does hasbuff only work on server?
            if(!characterBody.HasBuff(Modules.Buffs.zapShieldBuff)) {
                ShieldZapReleaseDamage newNextState = new ShieldZapReleaseDamage() {
                    aimRequest = this.aimRequest,
                    collectedDamage = blockedDamage,
                };
                EntityStateMachine.FindByCustomName(gameObject, "Weapon").SetNextState(newNextState);
                completed = true;
                base.outer.SetNextStateToMain();
            }
        }

        public override void OnExit() {
            base.OnExit();

            if (!completed) {
                aimRequest.Dispose();
            }
        }
    }
}