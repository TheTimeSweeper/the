using MatcherMod.Modules;
using RoR2;
using UnityEngine;

namespace MatcherMod.Survivors.Matcher.MatcherContent
{
    public class Items
    {
        public static ItemDef ChickenItem;

        public static void Init()
        {
            ChickenItem = ScriptableObject.CreateInstance<ItemDef>();

            ChickenItem = ScriptableObject.CreateInstance<ItemDef>();
            ChickenItem.name = "itemMatcherChicken";
            ChickenItem.nameToken = MatcherSurvivor.TOKEN_PREFIX + "ITEM_CHICKEN_NAME";
            ChickenItem.descriptionToken = MatcherSurvivor.TOKEN_PREFIX + "ITEM_CHICKEN_DESCRIPTION";
            ChickenItem.pickupToken = MatcherSurvivor.TOKEN_PREFIX + "ITEM_CHICKEN_PICKUP";
            ChickenItem.loreToken = MatcherSurvivor.TOKEN_PREFIX + "ITEM_CHICKEN_LORE";
            ChickenItem.canRemove = false;
            ChickenItem.pickupIconSprite = MatcherSurvivor.instance.assetBundle.LoadAsset<Sprite>("texTileChicken");
            ChickenItem.hidden = true;
            ChickenItem.tier = ItemTier.NoTier;
            ChickenItem.deprecatedTier = ItemTier.NoTier;

            Content.AddItemDef(ChickenItem);
        }
    }
}
