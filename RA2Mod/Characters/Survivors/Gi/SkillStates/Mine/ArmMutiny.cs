using EntityStates.Engi.Mine;
using UnityEngine.Networking;

namespace RA2Mod.Survivors.GI.SkillStates.Mine
{
    public class ArmMutiny : Arm
    {
        public override void OnEnter()
        {
            base.OnEnter();

            if (NetworkServer.active)
            {
                this.outer.SetNextState(new WaitForTargetMutiny());
            }
        }
    }
}
