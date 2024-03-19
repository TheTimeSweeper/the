using UnityEngine;
using RoR2;
using RA2Mod.Survivors.Tesla;
//todo DRY
public class TowerWeaponComponent : MonoBehaviour {

    public bool hasTeslaCoil;
    public TeslaSkinDef towerSkinDef;

    private CharacterBody characterBody;

    void Awake() {

        characterBody = GetComponent<CharacterBody>();

        characterBody.onInventoryChanged += CharacterBody_onInventoryChanged;
    }


    void Start() {

        try {
            towerSkinDef = RA2Mod.Modules.Skins.GetCurrentSkinDef(characterBody) as TeslaSkinDef;
        }
        catch {
            Destroy(this);
        }
    }

    private void CharacterBody_onInventoryChanged() {

        hasTeslaCoil = characterBody.inventory.GetItemCount(RoR2Content.Items.ShockNearby) > 0;
    }
}



