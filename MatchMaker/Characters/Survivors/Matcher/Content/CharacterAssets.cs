using RoR2;
using UnityEngine;
using MatcherMod.Modules;
using System;
using RoR2.Projectile;
using MatcherMod.Survivors.Matcher.Components;
using RoR2.Orbs;
using System.Collections.Generic;
using Matchmaker.MatchGrid;
using System.Collections;
using TMPro;
using R2API;

namespace MatcherMod.Survivors.Matcher.Content
{
    public static class CharacterAssets
    {
        // networked hit sounds
        public static NetworkSoundEventDef swordHitSoundEvent;

        //projectiles
        public static GameObject JoeFireball;

        private static AssetBundle _assetBundle;
        public static AssetBundle _gridAssetBundle;

        public static AsyncAsset<GameObject> matchGrid;
        public static GameObject JoeFireballExplosion;

        public static GameObject notMercSlashEffect;
        public static GameObject notMercSlashEffectThicc;

        public static AsyncAsset<GameObject> swordHitImpactEffect;

        public static AsyncAsset<BasicPickupDropTable> dropTableTier1Item;
        public static GameObject SkillTakenOrbEffect;

        public static GameObject BoxToOpenByMatching;

        public static TMPro.TMP_FontAsset bombardierFont;

        public static List<SpecialTileInfo> SpecialTiles = new List<SpecialTileInfo>();

        public static void Init(AssetBundle assetBundle)
        {

            _assetBundle = assetBundle;

            swordHitSoundEvent = Modules.Content.CreateAndAddNetworkSoundEventDef("HenrySwordHit");

            CreateEffects();

            CreateProjectiles();

            StealItemStealOrb();

            InitGridAssets();
        }
         
        //regular not-async loading
        #region effects
        private static void CreateEffects()
        {
            JoeFireballExplosion = _assetBundle.LoadEffect("JoeImpactEffectBasic");
            swordHitImpactEffect = Asset.AddAsyncAsset<GameObject>("RoR2/Base/Merc/OmniImpactVFXSlashMerc.prefab");

            Asset.LoadAssetAsync<GameObject>("RoR2/Base/Merc/MercSwordSlash.prefab", (slash) =>
            {
                notMercSlashEffect = slash.InstantiateClone("MatcherSwordMercSlash", false);
                Asset.RecolorEffects(Color.cyan, notMercSlashEffect);
                notMercSlashEffect.transform.Find("SwingTrail").localScale = new Vector3(1.1f, 1.2f, 5);
                notMercSlashEffect.GetComponent<ScaleParticleSystemDuration>().initialDuration = 2f;
            });
            
            Asset.LoadAssetAsync<GameObject>("RoR2/Base/Merc/MercSwordFinisherSlash.prefab", (slash) =>
            {
                notMercSlashEffectThicc = slash.InstantiateClone("MatcherSwordMercSlashFinisher", false);
                Asset.RecolorEffects(Color.cyan, notMercSlashEffectThicc);
                notMercSlashEffectThicc.transform.Find("SwingTrail").localScale = new Vector3(0.9f, 1.4f, 8);
                notMercSlashEffectThicc.GetComponent<ScaleParticleSystemDuration>().initialDuration = 3f;
            });
        }
        #endregion effects

        #region projectiles
        private static void CreateProjectiles()
        {
            JoeFireball = _assetBundle.LoadAsset<GameObject>("JoeFireballMatcher");
            JoeFireball.GetComponent<ProjectileImpactExplosion>().blastRadius = Content.CharacterConfig.M2_Staff_Radius.Value;
            Modules.Content.NetworkAndAddProjectilePrefab(JoeFireball);
        }
        #endregion projectiles

        #region before loading character
        //load ror2 font.
        //ignore this and run away
        internal static List<IEnumerator> GetAssetBundleInitializedCoroutines(AssetBundle assetBundle)
        {
            return new List<IEnumerator>
            {
                Modules.Asset.LoadAssetCoroutine<TMPro.TMP_FontAsset>("RoR2/Base/Common/Fonts/Bombardier/tmpBombDropshadow.asset", (font) =>
                {
                    bombardierFont = font;
                })
            };
        }
        #endregion before loading character

        //async loading, if all this looks funky, that's because it is.
        //long story short start a coroutine that when completed returns an asset that is loaded, then do stuff with it
        private static void InitGridAssets()
        {
            matchGrid = Modules.Asset.AddAsyncAsset<GameObject>(_assetBundle/*_gridAssetBundle*/, "Grid");
            dropTableTier1Item = Modules.Asset.AddAsyncAsset<BasicPickupDropTable>("RoR2/Base/Common/dtTier1Item.asset");//"RoR2/Base/Chest1/dtChest1.asset");

            //create coroutine to load asset
            //this coroutine is run by the contentpack so it loads async while the game loads
            Asset.LoadAssetAsync<GameObject>(_assetBundle, "BoxToOpenByMatching",
                //when loading is done, do our stuff with the result of our load
                (loadResult) =>
                {
                    BoxToOpenByMatching = loadResult;
                    BoxToOpenHologramContent hologramContent = BoxToOpenByMatching.GetComponent<BoxToOpenByMatching>().hologramPrefab.GetComponent<BoxToOpenHologramContent>();
                    for (int i = 0; i < hologramContent.TileCostTexts.Length; i++)
                    {
                        hologramContent.TileCostTexts[i].font = bombardierFont;
                    }
                });

            Asset.LoadAssetAsync<GameObject>(_assetBundle, "Tile2X", (tile) =>
            {
                tile.transform.Find("TileImage/Text").GetComponent<TMP_Text>().font = bombardierFont;
                SpecialTiles.Add(new SpecialTileInfo(tile.GetComponent<MatchTile>(), CharacterItems.AddTile2X, CharacterConfig.Special_2X_PercentChance));
            });
            Asset.LoadAssetAsync<GameObject>(_assetBundle, "Tile3X", (tile) =>
            {
                tile.transform.Find("TileImage/Text").GetComponent<TMP_Text>().font = bombardierFont;
                SpecialTiles.Add(new SpecialTileInfo(tile.GetComponent<MatchTile>(), CharacterItems.AddTile3X, CharacterConfig.Special_3X_PercentChance));
            });
            Asset.LoadAssetAsync<GameObject>(_assetBundle, "TileBomb", (tile) =>
            {
                SpecialTiles.Add(new SpecialTileInfo(tile.GetComponent<MatchTile>(), CharacterItems.AddTileBomb, CharacterConfig.Special_Bomb_PercentChance));
            });
            Asset.LoadAssetAsync<GameObject>(_assetBundle, "TileScroll", (tile) =>
            {
                SpecialTiles.Add(new SpecialTileInfo(tile.GetComponent<MatchTile>(), CharacterItems.AddTileScroll, CharacterConfig.Special_Scroll_PercentChance));
            });
            Asset.LoadAssetAsync<GameObject>(_assetBundle, "TileWild", (tile) =>
            {
                SpecialTiles.Add(new SpecialTileInfo(tile.GetComponent<MatchTile>(), CharacterItems.AddTileWild, CharacterConfig.Special_Wild_PercentChance));
            });
        }

        private static void StealItemStealOrb()
        {
            Asset.LoadAssetAsync<GameObject>("RoR2/Base/Common/VFX/ItemTransferOrbEffect.prefab", (result) =>
            {
                SkillTakenOrbEffect = R2API.PrefabAPI.InstantiateClone(result, "SkillTransferOrbEfect", false);

                ItemTakenOrbEffect itemOrbEffect = SkillTakenOrbEffect.GetComponent<ItemTakenOrbEffect>();
                SkillTakenOrbEffect skillOrbEffect = SkillTakenOrbEffect.AddComponent<SkillTakenOrbEffect>();

                skillOrbEffect.trailToColor = itemOrbEffect.trailToColor;
                skillOrbEffect.particlesToColor = itemOrbEffect.particlesToColor;
                skillOrbEffect.spritesToColor = itemOrbEffect.spritesToColor;
                skillOrbEffect.iconSpriteRenderer = itemOrbEffect.iconSpriteRenderer;

                SkillTakenOrbEffect.transform.Find("BillboardBase/DropShadow").gameObject.SetActive(false);
                SkillTakenOrbEffect.transform.Find("BillboardBase/Corners").gameObject.SetActive(false);

                skillOrbEffect.scaleSpriteComponent = SkillTakenOrbEffect.transform.Find("BillboardBase").GetComponent<ScaleSpriteByCamDistance>();
                skillOrbEffect.scaleSpriteComponent.enabled = false;

                OrbEffect orbEffect = SkillTakenOrbEffect.GetComponent<OrbEffect>();
                orbEffect.startVelocity1 = new Vector3(-10, 0, -10);
                orbEffect.startVelocity2 = new Vector3(10, 0, 10);
                orbEffect.endVelocity1 = new Vector3(-10, 10, -10);
                orbEffect.endVelocity2 = new Vector3(10, 10, 10);

                UnityEngine.Object.Destroy(itemOrbEffect);

                Modules.Content.CreateAndAddEffectDef(SkillTakenOrbEffect);
            });
        }
    }
}
