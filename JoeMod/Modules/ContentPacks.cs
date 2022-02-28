using RoR2;
using RoR2.ContentManagement;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HenryMod.Modules
{
    public class Content {
        public static void AddCharacterBodyPrefab(GameObject bprefab) {

            ContentPacks.bodyPrefabs.Add(bprefab);
        }
        public static void AddMasterPrefab(GameObject prefab) {

            ContentPacks.masterPrefabs.Add(prefab);
        }
        public static void AddProjectilePrefab(GameObject prefab) {

            ContentPacks.projectilePrefabs.Add(prefab);
        }

        public static void AddSurvivorDef(SurvivorDef survivorDef) {

            ContentPacks.survivorDefs.Add(survivorDef);
        }
        public static void AddUnlockableDef(UnlockableDef unlockableDef) {

            ContentPacks.unlockableDefs.Add(unlockableDef);
        }
        public static void AddSkillDef(SkillDef skillDef) {

            ContentPacks.skillDefs.Add(skillDef);
        }
        public static void AddSkillFamily(SkillFamily skillFamily) {

            ContentPacks.skillFamilies.Add(skillFamily);
        }
        public static void AddEntityState(Type entityState) {

            ContentPacks.entityStates.Add(entityState);
        }

        public static void AddBuffDef(BuffDef buffDef) {

            ContentPacks.buffDefs.Add(buffDef);
        }
        public static void AddEffectDef(EffectDef effectDef) {

            ContentPacks.effectDefs.Add(effectDef);
        }

        public static void AddNetworkSoundEventDef(NetworkSoundEventDef networkSoundEventDef) {

            ContentPacks.networkSoundEventDefs.Add(networkSoundEventDef);
        }
    }

    internal class ContentPacks : IContentPackProvider
    {
        internal ContentPack contentPack = new ContentPack();
        public string identifier => FacelessJoePlugin.MODUID;

        internal static List<GameObject> bodyPrefabs = new List<GameObject>();
        internal static List<GameObject> masterPrefabs = new List<GameObject>();
        internal static List<GameObject> projectilePrefabs = new List<GameObject>();

        internal static List<SurvivorDef> survivorDefs = new List<SurvivorDef>();
        internal static List<UnlockableDef> unlockableDefs = new List<UnlockableDef>();

        internal static List<SkillFamily> skillFamilies = new List<SkillFamily>();
        internal static List<SkillDef> skillDefs = new List<SkillDef>();
        internal static List<Type> entityStates = new List<Type>();

        internal static List<BuffDef> buffDefs = new List<BuffDef>();
        internal static List<EffectDef> effectDefs = new List<EffectDef>();

        internal static List<NetworkSoundEventDef> networkSoundEventDefs = new List<NetworkSoundEventDef>();

        public void Initialize()
        {
            ContentManager.collectContentPackProviders += ContentManager_collectContentPackProviders;
        }

        private void ContentManager_collectContentPackProviders(ContentManager.AddContentPackProviderDelegate addContentPackProvider)
        {
            addContentPackProvider(this);
        }

        public System.Collections.IEnumerator LoadStaticContentAsync(LoadStaticContentAsyncArgs args)
        {
            this.contentPack.identifier = this.identifier;

            contentPack.bodyPrefabs.Add(bodyPrefabs.ToArray());
            contentPack.masterPrefabs.Add(masterPrefabs.ToArray());
            contentPack.projectilePrefabs.Add(projectilePrefabs.ToArray());

            contentPack.survivorDefs.Add(survivorDefs.ToArray());
            contentPack.unlockableDefs.Add(unlockableDefs.ToArray());

            contentPack.skillDefs.Add(skillDefs.ToArray());
            contentPack.skillFamilies.Add(skillFamilies.ToArray());
            contentPack.entityStateTypes.Add(entityStates.ToArray());

            contentPack.buffDefs.Add(buffDefs.ToArray());
            contentPack.effectDefs.Add(effectDefs.ToArray());

            contentPack.networkSoundEventDefs.Add(networkSoundEventDefs.ToArray());


            args.ReportProgress(1f);
            yield break;
        }

        public System.Collections.IEnumerator GenerateContentPackAsync(GetContentPackAsyncArgs args)
        {
            ContentPack.Copy(this.contentPack, args.output);
            args.ReportProgress(1f);
            yield break;
        }

        public System.Collections.IEnumerator FinalizeAsync(FinalizeAsyncArgs args)
        {
            args.ReportProgress(1f);
            yield break;
        }
    }
}