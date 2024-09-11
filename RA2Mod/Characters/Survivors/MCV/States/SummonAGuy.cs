using EntityStates;
using RoR2;

namespace RA2Mod.Survivors.MCV.States
{
    internal class SummonAGuy: BaseSkillState
    {
        public override void OnEnter()
        {
            base.OnEnter();

            new MasterSummon
            {
                masterPrefab = MasterCatalog.GetMasterPrefab(MasterCatalog.FindMasterIndex("CommandoMonsterMaster")),
                position = transform.position,
                rotation = transform.rotation,
                summonerBodyObject = gameObject,
                ignoreTeamMemberLimit = true,
                inventoryToCopy = characterBody.inventory
            }.Perform();
        }
    }
}