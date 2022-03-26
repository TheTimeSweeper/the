using EntityStates;

namespace ModdedEntityStates.TeslaTrooper {
    public class ShieldZapCollectDamage : BaseSkillState {

        RoR2.CameraTargetParams.AimRequest aimRequest;

        public override void OnEnter() {
            base.OnEnter();

            aimRequest = cameraTargetParams.RequestAimType(RoR2.CameraTargetParams.AimType.Aura);
        }

        public override void FixedUpdate() {
            base.FixedUpdate();

            if(!characterBody.HasBuff(Modules.Buffs.zapShieldBuff)) {
                outer.SetNextState(new ShieldZapReleaseDamage() { aimRequest = this.aimRequest });
            }
        }
    }
}