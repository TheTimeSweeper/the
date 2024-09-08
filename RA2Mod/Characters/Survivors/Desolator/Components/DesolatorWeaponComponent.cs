using UnityEngine;
using RoR2;
//todo DRY

namespace RA2Mod.Survivors.Desolator.Components
{
    public class DesolatorWeaponComponent : MonoBehaviour
    {
        private CharacterBody characterBody;
        private Animator animator;

        void Awake()
        {

            characterBody = GetComponent<CharacterBody>();

            animator = characterBody.modelLocator.modelTransform.GetComponent<Animator>();

            characterBody.onInventoryChanged += CharacterBody_onInventoryChanged;
        }
        private void CharacterBody_onInventoryChanged()
        {

            bool hasHoldingItem = characterBody.inventory.GetItemCount(RoR2Content.Items.ChainLightning) > 0 ||
                                  characterBody.inventory.GetItemCount(DLC1Content.Items.ChainLightningVoid) > 0 ||
                                  characterBody.inventory.GetItemCount(RoR2Content.Items.ArmorReductionOnHit) > 0 ||
                                  characterBody.inventory.GetItemCount(DLC1Content.Items.MoveSpeedOnKill) > 0;

            animator.SetBool("LeftHandClosed", hasHoldingItem);
        }
    }
}

