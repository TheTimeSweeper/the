using EntityStates.Engi.Mine;
using UnityEngine.Networking;

namespace RA2Mod.Survivors.GI.SkillStates.Mine
{
    public class WaitForStickMutiny : WaitForStick
    {
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (NetworkServer.active && base.projectileStickOnImpact.stuck)
            {
                this.outer.SetNextState(new ArmMutiny());
            }
        }
    }
}
