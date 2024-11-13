using MatcherMod.Modules;
using RoR2;
using UnityEngine;

namespace MatcherMod.Survivors.Matcher.Content
{
    public class CharacterItems
    {
        public static ItemDef MatchChicken;

        public static ItemDef GridUpgradedCount;
        public static ItemDef ExpandTileGrid;
        public static ItemDef AddTile2X;
        public static ItemDef AddTile3X;
        public static ItemDef AddTileWild;
        public static ItemDef AddTileBomb;
        public static ItemDef AddTileScroll;
        public static ItemDef AddTileTimeStop;

        public static void Init(AssetBundle assetBundle)
        {
            MatchChicken = Modules.Content.CreateAndAddItemDef(
                "Chicken",
                assetBundle.LoadAsset<Sprite>("texTileChicken"),
                ItemTier.NoTier,
                true);

            #region grid upgrading

            GridUpgradedCount = Modules.Content.CreateAndAddItemDef(
                "GridUpgraded",
                assetBundle.LoadAsset<Sprite>("texItemGridExpand"),
                ItemTier.NoTier,
                true);

            ExpandTileGrid = Modules.Content.CreateAndAddItemDef(
                "TileGrid",
                assetBundle.LoadAsset<Sprite>("texItemGridExpand"),
                ItemTier.NoTier,
                false);

            AddTile2X = Modules.Content.CreateAndAddItemDef(
                "Tile2x",
                assetBundle.LoadAsset<Sprite>("texItem2X"),
                ItemTier.NoTier,
                false);
            AddTile3X = Modules.Content.CreateAndAddItemDef(
                "Tile3x",
                assetBundle.LoadAsset<Sprite>("texItem3X"),
                ItemTier.NoTier,
                false);
            AddTileWild = Modules.Content.CreateAndAddItemDef(
                "TileWild",
                assetBundle.LoadAsset<Sprite>("texItemWild"),
                ItemTier.NoTier,
                false);
            AddTileBomb = Modules.Content.CreateAndAddItemDef(
                "TileBomb",
                assetBundle.LoadAsset<Sprite>("texItemBomb"),
                ItemTier.NoTier,
                false);
            AddTileScroll = Modules.Content.CreateAndAddItemDef(
                "TileScroll",
                assetBundle.LoadAsset<Sprite>("texItemScroll"),
                ItemTier.NoTier,
                false);
            AddTileTimeStop = Modules.Content.CreateAndAddItemDef(
                "TileTimeStop",
                assetBundle.LoadAsset<Sprite>("texItemTimeStop"),
                ItemTier.NoTier,
                false);

            #endregion grid upgrading
        }
    }
}
