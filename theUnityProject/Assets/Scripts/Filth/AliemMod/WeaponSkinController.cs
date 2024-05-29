using RoR2;
using RoR2.Skills;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AliemMod.Components.Bundled {

    public class WeaponSkinController : MonoBehaviour {

        [SerializeField]
        private SkinDef[] weaponSkins;
        [SerializeField]
        private SkinDef[] weaponSkinsSecondaries;

        public void ApplyWeaponSkin(SkinDef skin) {
        }
    }
}
