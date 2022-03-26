using UnityEngine;
using RoR2;

public class TeslaWeaponComponent : MonoBehaviour {

    public bool hasTeslaCoil;

    private CharacterBody characterBody;

    void Awake() {
        characterBody = GetComponent<CharacterBody>();
        characterBody.onInventoryChanged += CharacterBody_onInventoryChanged;
    }

    private void CharacterBody_onInventoryChanged() {
        hasTeslaCoil = characterBody.master.inventory.GetItemCount(RoR2Content.Items.ShockNearby) > 0;
    }
}
