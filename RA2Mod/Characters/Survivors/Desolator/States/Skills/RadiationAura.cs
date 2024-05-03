using EntityStates;
using RA2Mod.Survivors.Desolator.Components;
using RoR2;
using UnityEngine.Networking;

namespace RA2Mod.Survivors.Desolator.States
{

    public class RadiationAura : BaseSkillState {

        public static float BuffDuration = 4f;
        public static float Radius = 40;

        public RoR2.CameraTargetParams.AimRequest aimRequest;

        //todo desomove
        private DesolatorAuraHolder _auraHolder;

        public override void OnEnter() {
            base.OnEnter();

            _auraHolder = GetComponent<DesolatorAuraHolder>();
            _auraHolder?.ActivateAura();

            Util.PlaySound("Play_Desolator_Deploy_High", gameObject);

            aimRequest = cameraTargetParams.RequestAimType(RoR2.CameraTargetParams.AimType.Aura);

            if (NetworkServer.active) {
                characterBody.AddTimedBuff(DesolatorBuffs.desolatorArmorBuff, BuffDuration);
            }
        }

        public override void FixedUpdate() {
            base.FixedUpdate();

            if (fixedAge > BuffDuration) {

                if (aimRequest != null)
                    aimRequest.Dispose();
                
                base.outer.SetNextStateToMain();
            }
        }

        public override void OnExit() {
            base.OnExit();

            _auraHolder?.DeactivateAura();
        }
    }
}
