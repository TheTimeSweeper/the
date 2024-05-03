using EntityStates;

namespace RA2Mod.Survivors.Desolator.States
{
    public class ScepterDeployEnter : DeployEnter {
        protected override void SetNextState()
        {
            _complete = true;
            var state = new ScepterDeployIrradiate { aimRequest = this.aimRequest };
            outer.SetNextState(state);
        }
    }
}
