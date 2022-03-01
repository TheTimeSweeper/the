using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace Modules
{
    public static class ItemDisplays
    {
        private static Dictionary<string, GameObject> itemDisplayPrefabs = new Dictionary<string, GameObject>();
        
        public static Dictionary<string, int> itemDisplayCheckCount = new Dictionary<string, int>();
        private static bool recording = false;

        public static void PopulateDisplays()
        {
            //PopulateFromBody("CommandoBody");
            //PopulateFromBody("CrocoBody");
            PopulateFromBody("MageBody");
            PopulateFromBody("LunarExploderBody");
        }

        private static void PopulateFromBody(string bodyName)
        {
            ItemDisplayRuleSet itemDisplayRuleSet = Resources.Load<GameObject>("Prefabs/CharacterBodies/" + bodyName)?.GetComponent<ModelLocator>()?.modelTransform.GetComponent<CharacterModel>()?.itemDisplayRuleSet;
            if(itemDisplayRuleSet == null) {
                Debug.LogError("couldn't load ItemDisplayRuleSet from " + bodyName + ". Check if name was entered correctly");
                return;
            }

            ItemDisplayRuleSet.KeyAssetRuleGroup[] itemRuleGroups = itemDisplayRuleSet.keyAssetRuleGroups;

            for (int i = 0; i < itemRuleGroups.Length; i++)
            {
                ItemDisplayRule[] rules = itemRuleGroups[i].displayRuleGroup.rules;

                for (int j = 0; j < rules.Length; j++)
                {
                    GameObject followerPrefab = rules[j].followerPrefab;
                    if (followerPrefab)
                    {
                        string name = followerPrefab.name;
                        string key = name?.ToLower();
                        if (!itemDisplayPrefabs.ContainsKey(key))
                        {
                            itemDisplayPrefabs[key] = followerPrefab;

                            itemDisplayCheckCount[key] = 0;
                        }
                    }
                }
            }
        }

        public static GameObject LoadDisplay(string name) {

            if (itemDisplayPrefabs.ContainsKey(name.ToLower())) {

                if (itemDisplayPrefabs[name.ToLower()]) {

                    if (recording) {
                        itemDisplayCheckCount[name.ToLower()]++;
                    }

                    GameObject display = itemDisplayPrefabs[name.ToLower()];

                    #region IgnoreThisAndRunAway
                    //seriously you don't need this
                    //I see you're still here, well if you do need this here's what you do
                    //but again you don't need this
                    //capacitor is hardcoded to track your "UpperArmR", "LowerArmR", and "HandR" bones.
                    //this is for having the lightning on custom bones in your childlocator
                    if (name == "DisplayLightningArmCustom") {
                        Helpers.LogWarning(itemDisplayPrefabs["DisplayLightningArmRight"] != null);
                        display = R2API.PrefabAPI.InstantiateClone(itemDisplayPrefabs["DisplayLightningArmRight"], "DisplayLightningCustom", false);

                        LimbMatcher limbMatcher = display.GetComponent<LimbMatcher>();

                        limbMatcher.limbPairs[0].targetChildLimb = "LightningArm1";
                        limbMatcher.limbPairs[1].targetChildLimb = "LightningArm2";
                        limbMatcher.limbPairs[2].targetChildLimb = "LightningArmEnd";
                    }
                    #endregion

                    return display;
                }
            }
            Debug.LogError("item display " + name + " returned null");
            return null;
        }

        #region check unused item displays
        public static void recordUnused() {
            recording = true;
        }
        public static void printUnused() {

            string yes = "used:";
            string no = "not used:";

            foreach (KeyValuePair<string, int> pair in itemDisplayCheckCount) {
                string thing = $"\n{itemDisplayPrefabs[pair.Key].name} | {itemDisplayPrefabs[pair.Key]} | {pair.Value}";

                if (pair.Value > 0) {
                    yes += thing;
                } else {
                    no += thing;
                }
            }
            //Debug.Log(yes);
            Helpers.LogWarning(no);

            //resetUnused();
        }

        private static void resetUnused() {
            foreach (KeyValuePair<string, int> pair in itemDisplayCheckCount) {
                itemDisplayCheckCount[pair.Key] = 0;
            }
            recording = false;
        }
        #endregion

        #region add rule helpers
        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateGenericDisplayRuleGroup(Object keyAsset_, GameObject itemPrefab, string childName, Vector3 position, Vector3 rotation, Vector3 scale) {

            ItemDisplayRule singleRule = CreateDisplayRule(itemPrefab, childName, position, rotation, scale);
            return CreateDisplayRuleGroupWithRules(keyAsset_, singleRule);
        }

        public static ItemDisplayRule CreateDisplayRule(GameObject itemPrefab, string childName, Vector3 position, Vector3 rotation, Vector3 scale) {
            return new ItemDisplayRule {
                ruleType = ItemDisplayRuleType.ParentedPrefab,
                childName = childName,
                followerPrefab = itemPrefab,
                limbMask = LimbFlags.None,
                localPos = position,
                localAngles = rotation,
                localScale = scale
            };
        }

        public static ItemDisplayRule CreateDisplayRule(string prefabName, string childName, Vector3 position, Vector3 rotation, Vector3 scale) {
            return new ItemDisplayRule {
                ruleType = ItemDisplayRuleType.ParentedPrefab,
                childName = childName,
                followerPrefab = LoadDisplay(prefabName),
                limbMask = LimbFlags.None,
                localPos = position,
                localAngles = rotation,
                localScale = scale
            };
        }

        public static ItemDisplayRule CreateLimbMaskDisplayRule(LimbFlags limb) {
            return new ItemDisplayRule {
                ruleType = ItemDisplayRuleType.LimbMask,
                limbMask = limb,
                childName = "",
                followerPrefab = null
            };
        }

        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateDisplayRuleGroupWithRules(string itemName, params ItemDisplayRule[] rules) {
            return CreateDisplayRuleGroupWithRules(Resources.Load<ItemDef>("ItemDefs/" + itemName), rules);
        }

        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateDisplayRuleGroupWithRulesE(string itemName, params ItemDisplayRule[] rules) {
            return CreateDisplayRuleGroupWithRules(Resources.Load<EquipmentDef>("EquipmentDefs/" + itemName), rules);
        }

        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateDisplayRuleGroupWithRules(Object keyAsset_, params ItemDisplayRule[] rules) {

            return new ItemDisplayRuleSet.KeyAssetRuleGroup {
                keyAsset = keyAsset_,
                displayRuleGroup = new DisplayRuleGroup {
                    rules = rules
                }
            };
        }

        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateGenericDisplayRule(string itemName, string prefabName, string childName, Vector3 position, Vector3 rotation, Vector3 scale) {
           
            ItemDisplayRuleSet.KeyAssetRuleGroup displayRule = new ItemDisplayRuleSet.KeyAssetRuleGroup {
                keyAsset = Resources.Load<ItemDef>("ItemDefs/" + itemName),
                displayRuleGroup = new DisplayRuleGroup {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            childName = childName,
                            followerPrefab = LoadDisplay(prefabName),
                            limbMask = LimbFlags.None,
                            localPos = position,
                            localAngles = rotation,
                            localScale = scale
                        }
                    }
                }
            };

            return displayRule;
        }

        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateGenericDisplayRuleE(string itemName, string prefabName, string childName, Vector3 position, Vector3 rotation, Vector3 scale) {
            ItemDisplayRuleSet.KeyAssetRuleGroup displayRule = new ItemDisplayRuleSet.KeyAssetRuleGroup {
                keyAsset = Resources.Load<EquipmentDef>("EquipmentDefs/" + itemName),
                displayRuleGroup = new DisplayRuleGroup {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            childName = childName,
                            followerPrefab = LoadDisplay(prefabName),
                            limbMask = LimbFlags.None,
                            localPos = position,
                            localAngles = rotation,
                            localScale = scale
                        }
                    }
                }
            };

            return displayRule;
        }

        public static ItemDisplayRuleSet.KeyAssetRuleGroup CreateGenericDisplayRule(string itemName, GameObject itemPrefab, string childName, Vector3 position, Vector3 rotation, Vector3 scale) {
            ItemDisplayRuleSet.KeyAssetRuleGroup displayRule = new ItemDisplayRuleSet.KeyAssetRuleGroup {
                keyAsset = Resources.Load<ItemDef>("ItemDefs/" + itemName),
                displayRuleGroup = new DisplayRuleGroup {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            childName = childName,
                            followerPrefab = itemPrefab,
                            limbMask = LimbFlags.None,
                            localPos = position,
                            localAngles = rotation,
                            localScale = scale
                        }
                    }
                }
            };

            return displayRule;
        }
        #endregion add rule helpers
    }
}