using EntityStates;
using RoR2;

namespace ModdedEntityStates.TeslaTrooper {
    public class ShieldZapCollectDamage : BaseSkillState {

        public RoR2.CameraTargetParams.AimRequest aimRequest;
        public bool completed;

        public override void FixedUpdate() {
            base.FixedUpdate();

            if(!characterBody.HasBuff(Modules.Buffs.zapShieldBuff)) {
                EntityStateMachine.FindByCustomName(gameObject, "Weapon").SetNextState(new ShieldZapReleaseDamage() { aimRequest = this.aimRequest });
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