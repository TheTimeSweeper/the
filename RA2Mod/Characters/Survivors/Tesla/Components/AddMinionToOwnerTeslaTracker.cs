using RoR2;
using UnityEngine;

namespace RA2Mod.Survivors.Tesla.Components
{
    public class AddMinionToOwnerTeslaTracker : MonoBehaviour
    {

        void Start()
        {
            GetComponent<CharacterBody>().master.GetComponent<MinionOwnership>().ownerMaster.GetBodyObject().GetComponent<TeslaTowerControllerController>().addNotTower(gameObject);
        }

        void OnDestroy()
        {

            GetComponent<CharacterBody>().master.GetComponent<MinionOwnership>().ownerMaster.GetBodyObject().GetComponent<TeslaTowerControllerController>().removeNotTower(gameObject);
        }
    }
}