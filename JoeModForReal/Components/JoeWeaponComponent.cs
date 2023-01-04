using RoR2;
using System;
using UnityEngine;

namespace JoeModForReal.Components {
    public class JoeWeaponComponent : MonoBehaviour {

        private CharacterBody characterBody;
        private ChildLocator childLocator;

        void Awake() {

            characterBody = GetComponent<CharacterBody>();
            childLocator = characterBody.modelLocator.modelTransform.GetComponent<ChildLocator>();

            characterBody.onInventoryChanged += CharacterBody_onInventoryChanged;
        }

        private void CharacterBody_onInventoryChanged() {
            bool hasScepter = Modules.Compat.TryGetScepterCount(characterBody.inventory) > 0;

            childLocator.FindChildGameObject("JoeSword").SetActive(!hasScepter);
        }
    }
}
