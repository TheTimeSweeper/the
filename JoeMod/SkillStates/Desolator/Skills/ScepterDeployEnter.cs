using EntityStates;

namespace ModdedEntityStates.Desolator {
    public class ScepterDeployEnter : DeployEnter {
        protected override EntityState ChooseNextState() {
            _complete = true;
            return new ScepterDeployIrradiate { aimRequest = this.aimRequest };
        }
    }
}
