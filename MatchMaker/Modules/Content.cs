using MatcherMod.Survivors.Matcher;
using RoR2;
using RoR2.Skills;
using System;
using UnityEngine;

namespace MatcherMod.Modules
{
    //consolidate contentaddition here in case something breaks and/or want to move to r2api
    internal class Content
    {
        internal static void AddCharacterBodyPrefab(GameObject bprefab)
        {
            ContentPacks.bodyPrefabs.Add(bprefab);
        }

        internal static void AddMasterPrefab(GameObject prefab)
        {
            ContentPacks.masterPrefabs.Add(prefab);
        }

        internal static void AddProjectilePrefab(GameObject prefab)
        {
            ContentPacks.projectilePrefabs.Add(prefab);
        }
        internal static void NetworkAndAddProjectilePrefab(GameObject prefab)
        {
            R2API.PrefabAPI.RegisterNetworkPrefab(prefab);
            AddProjectilePrefab(prefab);
        }

        internal static void AddSurvivorDef(SurvivorDef survivorDef)
        {

            ContentPacks.survivorDefs.Add(survivorDef);
        }
        internal static void CreateSurvivor(GameObject bodyPrefab, GameObject displayPrefab, Color charColor, string tokenPrefix) { CreateSurvivor(bodyPrefab, displayPrefab, charColor, tokenPrefix, null, 100f); }
        internal static void CreateSurvivor(GameObject bodyPrefab, GameObject displayPrefab, Color charColor, string tokenPrefix, float sortPosition) { CreateSurvivor(bodyPrefab, displayPrefab, charColor, tokenPrefix, null, sortPosition); }
        internal static void CreateSurvivor(GameObject bodyPrefab, GameObject displayPrefab, Color charColor, string tokenPrefix, UnlockableDef unlockableDef) { CreateSurvivor(bodyPrefab, displayPrefab, charColor, tokenPrefix, unlockableDef, 100f); }
        internal static void CreateSurvivor(GameObject bodyPrefab, GameObject displayPrefab, Color charColor, string tokenPrefix, UnlockableDef unlockableDef, float sortPosition)
        {
            SurvivorDef survivorDef = ScriptableObject.CreateInstance<SurvivorDef>();
            survivorDef.bodyPrefab = bodyPrefab;
            survivorDef.displayPrefab = displayPrefab;
            survivorDef.primaryColor = charColor;

            survivorDef.cachedName = bodyPrefab.name.Replace("Body", "");
            survivorDef.displayNameToken = tokenPrefix + "NAME";
            survivorDef.descriptionToken = tokenPrefix + "DESCRIPTION";
            survivorDef.outroFlavorToken = tokenPrefix + "OUTRO_FLAVOR";
            survivorDef.mainEndingEscapeFailureFlavorToken = tokenPrefix + "OUTRO_FAILURE";

            survivorDef.desiredSortPosition = sortPosition;
            survivorDef.unlockableDef = unlockableDef;

            Modules.Content.AddSurvivorDef(survivorDef);
        }

        internal static void AddUnlockableDef(UnlockableDef unlockableDef)
        {
            ContentPacks.unlockableDefs.Add(unlockableDef);
        }
        internal static UnlockableDef CreateAndAddUnlockbleDef(string identifier, string nameToken, Sprite achievementIcon)
        {
            UnlockableDef unlockableDef = ScriptableObject.CreateInstance<UnlockableDef>();
            unlockableDef.cachedName = identifier;
            unlockableDef.nameToken = nameToken;
            unlockableDef.achievementIcon = achievementIcon;

            AddUnlockableDef(unlockableDef);

            return unlockableDef;
        }

        internal static void AddNetworkedObject(GameObject gameObject)
        {
            ContentPacks.networkedObjects.Add(gameObject);
        }

        internal static void AddSkillDef(SkillDef skillDef)
        {
            ContentPacks.skillDefs.Add(skillDef);
        }

        internal static void AddSkillFamily(SkillFamily skillFamily)
        {
            ContentPacks.skillFamilies.Add(skillFamily);
        }

        internal static void AddEntityState(Type entityState)
        {
            ContentPacks.entityStates.Add(entityState);
        }

        internal static void AddBuffDef(BuffDef buffDef)
        {
            ContentPacks.buffDefs.Add(buffDef);
        }
        internal static BuffDef CreateAndAddBuff(string buffName, Sprite buffIcon, Color buffColor, bool canStack, bool isDebuff)
        {
            BuffDef buffDef = ScriptableObject.CreateInstance<BuffDef>();
            buffDef.name = buffName;
            buffDef.buffColor = buffColor;
            buffDef.canStack = canStack;
            buffDef.isDebuff = isDebuff;
            buffDef.eliteDef = null;
            buffDef.iconSprite = buffIcon;

            AddBuffDef(buffDef);

            return buffDef;
        }
        internal static void AddItemDef(ItemDef itemDef)
        {
            ContentPacks.itemDefs.Add(itemDef);
        }

        internal static ItemDef CreateAndAddItemDef(string itemName, Sprite iconSprite, ItemTier itemTier, bool hidden = false, bool canRemove = false)
            => CreateAndAddItemDef(MatcherPlugin.DEVELOPER_PREFIX + itemName + "Item", itemName, iconSprite, itemTier, hidden, canRemove);
        internal static ItemDef CreateAndAddItemDef(string itemname, string token, Sprite iconSprite, ItemTier itemTier, bool hidden = false, bool canRemove = false)
        {
            ItemDef itemDef = ScriptableObject.CreateInstance<ItemDef>();
            itemDef.name = itemname;
            itemDef.nameToken = MatcherSurvivor.TOKEN_PREFIX + $"ITEM_{token.ToUpperInvariant()}_NAME";
            itemDef.descriptionToken = MatcherSurvivor.TOKEN_PREFIX + $"ITEM_{token.ToUpperInvariant()}_DESCRIPTION";
            itemDef.pickupToken = MatcherSurvivor.TOKEN_PREFIX + $"ITEM_{token.ToUpperInvariant()}_PICKUP";
            itemDef.loreToken = MatcherSurvivor.TOKEN_PREFIX + $"ITEM_{token.ToUpperInvariant()}_LORE";
            itemDef.canRemove = canRemove;
            itemDef.pickupIconSprite = iconSprite;
            itemDef.hidden = hidden;
            itemDef.tier = itemTier;
            itemDef.deprecatedTier = itemTier;

            AddItemDef(itemDef);

            return itemDef;
        }

        internal static void AddEffectDef(EffectDef effectDef)
        {
            ContentPacks.effectDefs.Add(effectDef);
        }
        internal static EffectDef CreateAndAddEffectDef(GameObject effectPrefab, bool donotPool = false)
        {
            if (donotPool)
            {
                VFXAttributes vfxAttributes = effectPrefab.GetOrAddComponent<VFXAttributes>();
                vfxAttributes.DoNotPool = true;
            }
            EffectDef effectDef = new EffectDef(effectPrefab);

            AddEffectDef(effectDef);

            return effectDef;
        }

        internal static void AddNetworkSoundEventDef(NetworkSoundEventDef networkSoundEventDef)
        {
            ContentPacks.networkSoundEventDefs.Add(networkSoundEventDef);
        }
        internal static NetworkSoundEventDef CreateAndAddNetworkSoundEventDef(string eventName)
        {
            NetworkSoundEventDef networkSoundEventDef = ScriptableObject.CreateInstance<NetworkSoundEventDef>();
            networkSoundEventDef.akId = AkSoundEngine.GetIDFromString(eventName);
            networkSoundEventDef.eventName = eventName;

            AddNetworkSoundEventDef(networkSoundEventDef);
            
            return networkSoundEventDef;
        }
    }
}