using UnityEngine;

namespace AliemMod.Components {
    public class AliemMenuSound : MonoBehaviour {

        //still dont' know how to properly play sounds in menu rip
        //probably never will lol
        void OnEnable() {
            RoR2.Util.PlaySound("Play_INV_DigPopOut", gameObject);
        }
    }
}
