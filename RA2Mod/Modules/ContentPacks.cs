using RoR2;
using RoR2.ContentManagement;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RA2Mod.Modules {
    internal class ContentPacks : IContentPackProvider
    {
        internal ContentPack contentPack = new ContentPack();
        public string identifier => RA2Plugin.MODUID;

        public static List<GameObject> bodyPrefabs = new List<GameObject>();
        public static List<GameObject> masterPrefabs = new List<GameObject>();
        public static List<GameObject> projectilePrefabs = new List<GameObject>();

        public static List<SurvivorDef> survivorDefs = new List<SurvivorDef>();
        public static List<UnlockableDef> unlockableDefs = new List<UnlockableDef>();

        public static List<SkillFamily> skillFamilies = new List<SkillFamily>();
        public static List<SkillDef> skillDefs = new List<SkillDef>();
        public static List<Type> entityStates = new List<Type>();
        public static List<EntityStateConfiguration> entityStateConfigurations = new List<EntityStateConfiguration>();

        public static List<BuffDef> buffDefs = new List<BuffDef>();
        public static List<EffectDef> effectDefs = new List<EffectDef>();
        public static List<ItemDef> itemDefs = new List<ItemDef>();

        public static List<NetworkSoundEventDef> networkSoundEventDefs = new List<NetworkSoundEventDef>();
        public static List<GameObject> networkedObjects = new List<GameObject>();

        public delegate void DoThingsAfterShitHasLoaded();
        public DoThingsAfterShitHasLoaded doShitAfterThingsHaveLoaded;

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

            if (RA2Plugin.testAsyncLoading)
            {
                //Log.CurrentTime("ASYNC START");
                //yield return Survivors.Chrono.ChronoAssets.InitAsync(Survivors.Chrono.ChronoSurvivor.instance.assetBundle);
                //Log.CurrentTime("ASYNC FINISH");

                Log.CurrentTime("ASYNC2 START");
                List<System.Collections.IEnumerator> enumerators = Survivors.Chrono.ChronoAssets.InitAsync2(Survivors.Chrono.ChronoSurvivor.instance.assetBundle);

                for (int i = 0; i < enumerators.Count; i++)
                {
                    while (enumerators[i].MoveNext()) { yield return null; }
                }
                Log.CurrentTime("ASYNC2 FINISH");
            }

            contentPack.bodyPrefabs.Add(bodyPrefabs.ToArray());
            contentPack.masterPrefabs.Add(masterPrefabs.ToArray());
            contentPack.projectilePrefabs.Add(projectilePrefabs.ToArray());

            contentPack.survivorDefs.Add(survivorDefs.ToArray());
            contentPack.unlockableDefs.Add(unlockableDefs.ToArray());

            contentPack.skillDefs.Add(skillDefs.ToArray());
            contentPack.skillFamilies.Add(skillFamilies.ToArray());
            contentPack.entityStateTypes.Add(entityStates.ToArray());
            contentPack.entityStateConfigurations.Add(entityStateConfigurations.ToArray());

            contentPack.buffDefs.Add(buffDefs.ToArray());
            contentPack.effectDefs.Add(effectDefs.ToArray());
            contentPack.itemDefs.Add(itemDefs.ToArray());

            contentPack.networkSoundEventDefs.Add(networkSoundEventDefs.ToArray());
            contentPack.networkedObjectPrefabs.Add(networkedObjects.ToArray());

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
            doShitAfterThingsHaveLoaded?.Invoke();
            args.ReportProgress(1f);
            yield break;
        }
    }
}