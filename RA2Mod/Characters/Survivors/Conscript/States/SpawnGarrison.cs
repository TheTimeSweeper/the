using RA2Mod.Modules.BaseStates;
using UnityEngine;
using UnityEngine.Networking;

namespace RA2Mod.Survivors.Conscript.States
{
    public class SpawnGarrison : BaseTimedSkillState
    {
        public override float TimedBaseDuration => 1;
        public override float TimedBaseCastStartPercentTime => 1;

        public override void OnEnter()
        {
            base.OnEnter();

            Ray aimray = GetAimRay();
            Vector3 pos = aimray.origin + aimray.direction * 10; 
            if (Physics.Raycast(GetAimRay(), out RaycastHit hitInfo, 10, RoR2.LayerIndex.world.mask))
            {
                pos = hitInfo.point;
            }

            GameObject garrison = UnityEngine.Object.Instantiate(ConscriptAssets.Garrison, pos, Quaternion.identity);
            garrison.GetComponent<RoR2.TeamFilter>().teamIndex = teamComponent.teamIndex;
            NetworkServer.Spawn(garrison);
        }
    }
}
