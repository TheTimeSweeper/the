using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace JoeModForReal.Components {
    public class JoeWeaponComponent : MonoBehaviour {

        private CharacterBody _characterBody;
        private ChildLocator _childLocator;
        private bool _hasScepter;

        void Awake() {

            _characterBody = GetComponent<CharacterBody>();
            _childLocator = _characterBody.modelLocator.modelTransform.GetComponent<ChildLocator>();

            _characterBody.onInventoryChanged += CharacterBody_onInventoryChanged;
        }

        private void CharacterBody_onInventoryChanged() {

            bool hasScepter = Modules.Compat.TryGetScepterCount(_characterBody.inventory) > 0;
            SetHasScepter(hasScepter);
        }

        private void SetHasScepter(bool hasScepter) {

            if (hasScepter == _hasScepter)
                return;
            _hasScepter = hasScepter;

            _childLocator.FindChildGameObject("JoeSword").SetActive(!hasScepter);

            if (hasScepter) {
                Util.PlaySound("play_joe_loz_fanfare", gameObject);
            }
        }
    }
}
