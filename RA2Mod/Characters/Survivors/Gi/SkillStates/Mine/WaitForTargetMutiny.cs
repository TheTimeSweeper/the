using EntityStates.Engi.Mine;
using UnityEngine.Networking;

namespace RA2Mod.Survivors.GI.SkillStates.Mine
{
    public class WaitForTargetMutiny : WaitForTarget
    {
        public override void OnEnter()
        {
            base.OnEnter();
            targetFinder.lookRange = 8;
            if (NetworkServer.active)
            {
                base.armingStateMachine.SetNextState(new MineArmingMutiny());
            }
        }
    }
}
