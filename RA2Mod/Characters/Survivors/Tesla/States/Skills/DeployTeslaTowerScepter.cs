using RoR2;
using UnityEngine;

namespace RA2Mod.Survivors.Tesla.States
{
    public class DeployTeslaTowerScepter : DeployTeslaTower
    {

        private GameObject coilMasterPrefab = Survivors.TeslaTower.TeslaTowerScepter.masterPrefab;

        protected override void constructCoil(TotallyOriginalPlacementInfo placementInfo)
        {

            characterBody.SendConstructTurret(characterBody,
                                                   placementInfo.position,
                                                   placementInfo.rotation,
                                                   MasterCatalog.FindMasterIndex(coilMasterPrefab));

            //I am fucking still exploding but slightly less fucking
            Util.PlaySound("Play_buliding_uplace", gameObject);
        }
    }
}