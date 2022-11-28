using UnityEngine;
using RoR2;
using JoeMod;
//todo DRY
public class TowerWeaponComponent : MonoBehaviour {

    public bool hasTeslaCoil;
    public TowerSkinDef towerSkinDef;

    private CharacterBody characterBody;

    void Awake() {

        characterBody = GetComponent<CharacterBody>();

        characterBody.onInventoryChanged += CharacterBody_onInventoryChanged;
    }


    void Start() {

        towerSkinDef = Modules.Skins.GetCurrentSkinDef(characterBody) as TowerSkinDef;
    }

    private void CharacterBody_onInventoryChanged() {

        hasTeslaCoil = characterBody.inventory.GetItemCount(RoR2Content.Items.ShockNearby) > 0;
    }
}



