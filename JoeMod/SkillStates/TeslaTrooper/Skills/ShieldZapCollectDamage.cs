using EntityStates;
using RoR2;

namespace ModdedEntityStates.TeslaTrooper {
    public class ShieldZapCollectDamage : BaseSkillState {

        public RoR2.CameraTargetParams.AimRequest aimRequest;

        public override void FixedUpdate() {
            base.FixedUpdate();

            if(!characterBody.HasBuff(Modules.Buffs.zapShieldBuff)) {
                EntityStateMachine.FindByCustomName(gameObject, "Weapon").SetNextState(new ShieldZapReleaseDamage() { aimRequest = this.aimRequest });
                base.outer.SetNextStateToMain();
            }
        }
    }
}