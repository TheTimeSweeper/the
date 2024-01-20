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
            chronoSicknessItemDef.nameToken = ChronoSurvivor.CHRONO_PREFIX + "ITEM_SICKNESS_NAME";
            chronoSicknessItemDef.descriptionToken = ChronoSurvivor.CHRONO_PREFIX + "ITEM_SICKNESS_DESCRIPTION";
            chronoSicknessItemDef.pickupToken = ChronoSurvivor.CHRONO_PREFIX + "ITEM_SICKNESS_PICKUP";
            chronoSicknessItemDef.loreToken = ChronoSurvivor.CHRONO_PREFIX + "ITEM_SICKNESS_LORE";
            chronoSicknessItemDef.canRemove = false;
            chronoSicknessItemDef.hidden = true;
            chronoSicknessItemDef.tier = ItemTier.NoTier;

            Content.AddItemDef(chronoSicknessItemDef);
        }
    }
}