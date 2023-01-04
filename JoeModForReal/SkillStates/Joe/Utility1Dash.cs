using EntityStates;
using RoR2;
using UnityEngine.Networking;

namespace ModdedEntityStates.Joe {

    public class Utility1Dash : UtilityBaseDash {

        public override void OnEnter() {

			if (isAuthority && inputBank.skill1.down && activatorSkillSlot.stock > 0) {

				EntityStateMachine.FindByCustomName(gameObject, "Body").SetNextState(new Utility1ChargeMeleeDash());

				base.outer.SetNextStateToMain();
				return;
			}
            
            base.OnEnter();
            
            if (NetworkServer.active) {
                characterBody.AddTimedBuff(Modules.Buffs.DashArmorBuff, duration + 0.1f);
            }

            Util.PlaySound("play_joe_roguelDash", gameObject);
        }

        protected override void SetNextState() {

            WindDownState state = new WindDownState();
            state.windDownTime = 0.2f;//todo testvaluemanager
            state.interruptPriority = InterruptPriority.PrioritySkill;

            base.outer.SetNextState(state);
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.PrioritySkill;
        }
    }
}