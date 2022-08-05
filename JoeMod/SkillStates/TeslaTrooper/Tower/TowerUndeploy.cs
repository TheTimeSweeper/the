using EntityStates;
using RoR2;
using ModdedEntityStates.BaseStates;

namespace ModdedEntityStates.TeslaTrooper.Tower {

    internal class TowerUndeploy : BaseTimedSkillState {


        public override void OnEnter() {
            base.OnEnter();
            InitDurationValues(0.5f, 1);
        }

        protected override void OnCastEnter () {
            base.OnCastEnter();

            Deployable deployable = characterBody.masterObject?.GetComponent<Deployable>();

            if (deployable) {

                deployable.ownerMaster?.RemoveDeployable(deployable);
                deployable.ownerMaster = null;
                deployable.onUndeploy.Invoke();
            }
        }
        //public override void OnEnter() {
        //    base.OnEnter();

        //    Deployable deployable = base.characterBody.masterObject?.GetComponent<Deployable>();

        //    Helpers.LogWarning(base.characterBody.masterObject);
        //    Helpers.LogWarning(deployable);

        //    if (deployable) {

        //        Helpers.LogWarning(base.characterBody.masterObject?.GetComponent<MinionOwnership>());
        //        Helpers.LogWarning(base.characterBody.masterObject?.GetComponent<MinionOwnership>()?.ownerMaster);
        //        Helpers.LogWarning(deployable.ownerMaster);

        //        base.characterBody.masterObject?.GetComponent<MinionOwnership>()?.ownerMaster?.RemoveDeployable(deployable);
        //        //forgive my abuse of the ?'s
        //        Helpers.LogWarning("the shit");
        //    }
        //}
    }
}