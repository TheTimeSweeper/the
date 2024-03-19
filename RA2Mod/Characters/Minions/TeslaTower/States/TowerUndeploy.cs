using EntityStates;
using RoR2;
using RA2Mod.Modules.BaseStates;

namespace RA2Mod.Minions.TeslaTower.States
{

    internal class TowerUndeploy : BaseTimedSkillState
    {
        public override float TimedBaseDuration => 0.5f;

        public override float TimedBaseCastStartPercentTime => 1;

        protected override void OnCastEnter()
        {
            base.OnCastEnter();

            Deployable deployable = characterBody.masterObject?.GetComponent<Deployable>();

            if (deployable)
            {

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