using UnityEngine;
using RoR2;

public class TeslaWeaponComponent : MonoBehaviour {

    public bool hasTeslaCoil;

    private CharacterBody characterBody;
    private Inventory inventory;
    private Animator animator;

    void Awake() {
        characterBody = GetComponent<CharacterBody>();
        inventory = characterBody.inventory;
        characterBody.onInventoryChanged += CharacterBody_onInventoryChanged;

        animator = characterBody.modelLocator.modelTransform.GetComponent<Animator>();
    }

    private void CharacterBody_onInventoryChanged() {

        hasTeslaCoil = inventory.GetItemCount(RoR2Content.Items.ShockNearby) > 0;

        bool hasHoldingItem = inventory.GetItemCount(RoR2Content.Items.ChainLightning) > 0 ||
                              inventory.GetItemCount(DLC1Content.Items.ChainLightningVoid) > 0;

        animator.SetBool("LeftHandClosed", hasHoldingItem);
    }
}
