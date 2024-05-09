using EntityStates;

namespace RA2Mod.Survivors.Desolator.States
{
    public class ScepterDeployEnter : DeployEnter {
        protected override void SetNextState()
        {
            _complete = true;
            ScepterDeployIrradiate state = new ScepterDeployIrradiate { aimRequest = this.aimRequest, fromEnter = true, activatorSkillSlot = this.activatorSkillSlot };
            outer.SetNextState(state);
        }
    }
}
