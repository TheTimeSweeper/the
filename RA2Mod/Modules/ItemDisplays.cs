using RoR2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RA2Mod.Modules
{
    internal static class ItemDisplays
    {
        private static Dictionary<string, GameObject> itemDisplayPrefabs = new Dictionary<string, GameObject>();
        public static Dictionary<Object, ItemDisplayRule[]> KeyAssetDisplayRules = new Dictionary<Object, ItemDisplayRule[]>();
        public static Dictionary<string, Object> KeyAssets = new Dictionary<string, Object>();

        public static bool initialized = false;
        public static bool initializing = false;

        public static event System.Action OnDisplaysPopulated;

        public static void SetItemDisplaysWhenReady(System.Action onComplete)
        {
            if (initialized)
            {
                onComplete?.Invoke();
                return;
            }

            OnDisplaysPopulated += onComplete;

            if (!initializing)
            { 
                initializing = true;
                ContentPacks.asyncLoadCoroutines.Add(PopulateDisplays());
                RoR2Application.onLoad += DisposeWhenDone;
            }
        }

        internal static void DisposeWhenDone()
        {
            initialized = false;
            initializing = false;

            itemDisplayPrefabs = null;
            KeyAssetDisplayRules = null;
            KeyAssets = null;
        }

        internal static IEnumerator PopulateDisplays()
        {
            IEnumerator loadIDRS = Asset.LoadAssetCoroutine<GameObject>("RoR2/Base/Loader/LoaderBody.prefab", (result) => {
                PopulateFromBody(result);
            });

            while (loadIDRS.MoveNext())
            {
                yield return null;
            }

            PopulateCustomLightningArm();
            //if you have any custom item displays to add here I would be very impressed

            initialized = true;
            OnDisplaysPopulated?.Invoke();
        }

        private static void PopulateFromBody(GameObject bodyPrefab)
        {
            //GameObject bodyPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("RoR2/Base/Loader/LoaderBody.prefab");

            ItemDisplayRuleSet itemDisplayRuleSet = bodyPrefab.GetComponent<ModelLocator>().modelTransform.GetComponent<CharacterModel>().itemDisplayRuleSet;

            ItemDisplayRuleSet.KeyAssetRuleGroup[] itemRuleGroups = itemDisplayRuleSet.keyAssetRuleGroups;

            for (int i = 0; i < itemRuleGroups.Length; i++)
            {
                ItemDisplayRule[] rules = itemRuleGroups[i].displayRuleGroup.rules;

                KeyAssetDisplayRules[itemRuleGroups[i].keyAsset] = rules;
                KeyAssets[itemRuleGroups[i].keyAsset.name] = itemRuleGroups[i].keyAsset;

                for (int j = 0; j < rules.Length; j++)
                {
                    GameObject followerPrefab = rules[j].followerPrefab;
                    if (followerPrefab)
                    {
                        string key = followerPrefab.name?.ToLowerInvariant();
                        if (!itemDisplayPrefabs.ContainsKey(key))
                        {
                            itemDisplayPrefabs[key] = followerPrefab;
                        }
                    }
                }
            }
        }

        private static void PopulateCustomLightningArm()
        {
            #region IgnoreThisAndRunAway
            //seriously you don't need this
            //I see you're still here, well if you do need this here's what you do
            //but again you don't need this
            //capacitor is hardcoded to track your "UpperArmR", "LowerArmR", and "HandR" bones.
            //this is for having the lightning on custom bones in your childlocator

            GameObject display = R2API.PrefabAPI.InstantiateClone(itemDisplayPrefabs["displaylightningarmright"], "DisplayLightningCustom", false);

            LimbMatcher limbMatcher = display.GetComponent<LimbMatcher>();

            limbMatcher.limbPairs[0].targetChildLimb = "LightningArm1";
            limbMatcher.limbPairs[1].targetChildLimb = "LightningArm2";
            limbMatcher.limbPairs[2].targetChildLimb = "LightningArmEnd";

            itemDisplayPrefabs["displaylightningarmcustom"] = display;
            #endregion
        }

        public static GameObject LoadDisplay(string name) {

            if (itemDisplayPrefabs.ContainsKey(name.ToLowerInvariant())) {

                if (itemDisplayPrefabs[name.ToLowerInvariant()]) {

                    GameObject display = itemDisplayPrefabs[name.ToLowerInvariant()];

                    return display;
                }
            }
            Log.Error("item display " + name + " returned null");
            return null;
        }

        #region add rule helpers

        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateGenericDisplayRule(Object itemDef, string prefabName, string childName, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            return CreateDisplayRuleGroupWithRules(itemDef, new ItemDisplayRule[] {
                CreateDisplayRule(prefabName, childName, position, rotation, scale)
            });
        }
        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateGenericDisplayRule(Object itemDef, GameObject prefab, string childName, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            return CreateDisplayRuleGroupWithRules(itemDef, new ItemDisplayRule[] {
                CreateDisplayRule(prefab, childName, position, rotation, scale)
            });
        }

        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateDisplayRuleGroupWithRules(string itemName, params ItemDisplayRule[] rules) => CreateDisplayRuleGroupWithRules(GetKeyAssetFromString(itemName), rules);
        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateDisplayRuleGroupWithRules(Object keyAsset_, params ItemDisplayRule[] rules)
        {
            if (keyAsset_ == null)
                Log.Error("could not find keyasset");

            return new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = keyAsset_,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = rules
                }
            };
        }

        public static ItemDisplayRule CreateDisplayRule(string prefabName, string childName, Vector3 position, Vector3 rotation, Vector3 scale) => CreateDisplayRule(LoadDisplay(prefabName), childName, position, rotation, scale);
        public static ItemDisplayRule CreateDisplayRule(GameObject itemPrefab, string childName, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            return new ItemDisplayRule
            {
                ruleType = ItemDisplayRuleType.ParentedPrefab,
                childName = childName,
                followerPrefab = itemPrefab,
                limbMask = LimbFlags.None,
                localPos = position,
                localAngles = rotation,
                localScale = scale
            };
        }
        public static ItemDisplayRule CreateLimbMaskDisplayRule(LimbFlags limb)
        {
            return new ItemDisplayRule
            {
                ruleType = ItemDisplayRuleType.LimbMask,
                limbMask = limb,
                childName = "",
                followerPrefab = null
                //localPos = Vector3.zero,
                //localAngles = Vector3.zero,
                //localScale = Vector3.zero
            };
        }

        private static Object GetKeyAssetFromString(string itemName)
        {
            Object itemDef = KeyAssets[itemName];

            if (itemDef == null)
            {
                Log.Error("Could not load keyasset for " + itemName);
            }

            return itemDef;
        }

        #endregion add rule helpers
    }
}