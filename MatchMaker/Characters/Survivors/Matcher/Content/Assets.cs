using RoR2;
using UnityEngine;
using MatcherMod.Modules;
using System;
using RoR2.Projectile;
using MatcherMod.Survivors.Matcher.Components;
using RoR2.Orbs;

namespace MatcherMod.Survivors.Matcher.MatcherContent
{
    public static class Assets
    {
        // networked hit sounds
        public static NetworkSoundEventDef swordHitSoundEvent;

        //projectiles
        public static GameObject JoeFireball;

        private static AssetBundle _assetBundle;
        public static AssetBundle _gridAssetBundle;

        public static AsyncAsset<GameObject> matchGrid;
        internal static GameObject JoeFireballExplosion;

        public static AsyncAsset<BasicPickupDropTable> dtTier1Item;
        public static GameObject SkillTakenOrbEffect;

        public static GameObject BoxToOpenByMatching;

        public static void Init(AssetBundle assetBundle)
        {

            _assetBundle = assetBundle;

            swordHitSoundEvent = Modules.Content.CreateAndAddNetworkSoundEventDef("HenrySwordHit");

            CreateEffects();

            CreateProjectiles();

            //Modules.Asset.LoadAssetBundleAsync("matcher", (result) =>
            //{
            //    _gridAssetBundle = result;
            //    InitGridAssets();
            //});

            InitGridAssets();

            StealItemStealOrb();
        }

        private static void StealItemStealOrb()
        {
            GameObject result = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OrbEffects/ItemTransferOrbEffect");

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

        private static void InitGridAssets()
        {
            matchGrid = Modules.Asset.AddAsyncAsset<GameObject>(_assetBundle/*_gridAssetBundle*/, "Grid");
            dtTier1Item = Modules.Asset.AddAsyncAsset<BasicPickupDropTable>("RoR2/Base/Common/dtTier1Item.asset");
            Modules.Asset.LoadAssetsAsync<GameObject, TMPro.TMP_FontAsset>(
                new AsyncAsset<GameObject>(_assetBundle, "BoxToOpenByMatching"),
                new AsyncAsset<TMPro.TMP_FontAsset>("RoR2/Base/Common/Fonts/Bombardier/tmpBombDropshadow.asset"), 
                (box, font) =>
                {
                    BoxToOpenByMatching = box;
                    BoxToOpenHologramContent hologramContent = BoxToOpenByMatching.GetComponent<BoxToOpenByMatching>().hologramPrefab.GetComponent<BoxToOpenHologramContent>();
                    for (int i = 0; i < hologramContent.TileCostTexts.Length; i++)
                    {
                        hologramContent.TileCostTexts[i].font = font;
                    }
                });
        }

        #region effects
        private static void CreateEffects()
        {
            JoeFireballExplosion = _assetBundle.LoadEffect("JoeImpactEffectBasic");
        }
        #endregion effects

        #region projectiles
        private static void CreateProjectiles()
        {
            JoeFireball = _assetBundle.LoadAsset<GameObject>("JoeFireballMatcher");
            JoeFireball.GetComponent<ProjectileImpactExplosion>().blastRadius = MatcherContent.Config.M2_Staff_Radius.Value;
            Content.NetworkAndAddProjectilePrefab(JoeFireball);
        }
        #endregion projectiles
    }
}
