using RoR2;
using UnityEngine;

namespace ModdedEntityStates.TeslaTrooper {
    public class DeployTeslaTowerScepter : DeployTeslaTower {

        private GameObject coilMasterPrefab = Modules.Survivors.TeslaTowerScepter.masterPrefab;

        protected override void constructCoil(TotallyOriginalPlacementInfo placementInfo) {

            base.characterBody.SendConstructTurret(base.characterBody,
                                                   placementInfo.position,
                                                   placementInfo.rotation,
                                                   MasterCatalog.FindMasterIndex(coilMasterPrefab));

            //I am fucking still exploding but slightly less fucking
            Util.PlaySound("Play_buliding_uplace", gameObject);
        }
    }
}