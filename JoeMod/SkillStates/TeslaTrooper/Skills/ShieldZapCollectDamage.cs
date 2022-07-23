using EntityStates;
using RoR2;

namespace ModdedEntityStates.TeslaTrooper {
    public class ShieldZapCollectDamage : BaseSkillState {

        public RoR2.CameraTargetParams.AimRequest aimRequest;

        private float blockedDamage = 0;

        ZapBarrierController controller;
        private bool completed;

        public override void OnEnter() {
            base.OnEnter();

            controller = GetComponent<ZapBarrierController>();
            if (controller) {
                controller.onBlockedDamage += onBlockedDamage;
            }
        }

        private void onBlockedDamage(float damageBlocked) {
            blockedDamage += damageBlocked;
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

            if (controller) {
                controller.onBlockedDamage -= onBlockedDamage;
            }

            if (!completed) {
                aimRequest.Dispose();
            }
        }
    }
}