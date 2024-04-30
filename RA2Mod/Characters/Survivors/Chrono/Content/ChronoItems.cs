using RA2Mod.Modules;
using RoR2;
using UnityEngine;

namespace RA2Mod.Survivors.Chrono
{
    public static class ChronoItems
    {
        public static ItemDef chronoSicknessItemDef;

        public static void Init()
        {
            chronoSicknessItemDef = ScriptableObject.CreateInstance<ItemDef>();
            chronoSicknessItemDef.name = "ChronoSickness";
            chronoSicknessItemDef.nameToken = ChronoSurvivor.TOKEN_PREFIX + "ITEM_SICKNESS_NAME";
            chronoSicknessItemDef.descriptionToken = ChronoSurvivor.TOKEN_PREFIX + "ITEM_SICKNESS_DESCRIPTION";
            chronoSicknessItemDef.pickupToken = ChronoSurvivor.TOKEN_PREFIX + "ITEM_SICKNESS_PICKUP";
            chronoSicknessItemDef.loreToken = ChronoSurvivor.TOKEN_PREFIX + "ITEM_SICKNESS_LORE";
            chronoSicknessItemDef.canRemove = false;
            chronoSicknessItemDef.hidden = true;
            chronoSicknessItemDef.tier = ItemTier.NoTier;
            chronoSicknessItemDef.deprecatedTier = ItemTier.NoTier;

            Content.AddItemDef(chronoSicknessItemDef);
        }
    }
}