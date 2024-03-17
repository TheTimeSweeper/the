using RoR2;
using UnityEngine;

namespace Modules.Survivors {
    public class AddMinionToOwnerTeslaTracker : MonoBehaviour {
        
        void Start() {
            GetComponent<CharacterBody>().master.GetComponent<MinionOwnership>().ownerMaster.GetBodyObject().GetComponent<TeslaTowerControllerController>().addNotTower(gameObject);
        }

        void OnDestroy() {

            GetComponent<CharacterBody>().master.GetComponent<MinionOwnership>().ownerMaster.GetBodyObject().GetComponent<TeslaTowerControllerController>().removeNotTower(gameObject);
        }
    }
}