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

            //1: yielding other coroutines
            //about equal with loading everything in awake
            if (RA2Plugin.testAsyncLoading == 1)
            {
                Log.CurrentTime("ASYNC START");

                yield return Survivors.Chrono.ChronoAssets.InitAsync(Survivors.Chrono.ChronoSurvivor.instance.assetBundle);

                Log.CurrentTime("ASYNC FINISH");
            }

            //2: loading all coroutines in a list and stepping through them, yielding null
            //best
            if (RA2Plugin.testAsyncLoading == 2)
            {
                Log.CurrentTime("ASYNC2 START");

                List<System.Collections.IEnumerator> enumerators = Survivors.Chrono.ChronoAssets.InitAsync2(Survivors.Chrono.ChronoSurvivor.instance.assetBundle);

                for (int i = 0; i < enumerators.Count; i++)
                {
                    while (enumerators[i].MoveNext()) yield return null;
                }

                Log.CurrentTime("ASYNC2 FINISH");
            }

            //3: number 2 but attempting to step through multiple in a while
            //worst
            if (RA2Plugin.testAsyncLoading == 3)
            {
                Log.CurrentTime("ASYNC3 START");

                List<System.Collections.IEnumerator> enumerators = Survivors.Chrono.ChronoAssets.InitAsync2(Survivors.Chrono.ChronoSurvivor.instance.assetBundle);
                
                bool inComplete = true;
                while (inComplete)
                {
                    inComplete = false;
                    for (int i = 0; i < enumerators.Count; i++)
                    {
                        inComplete |= enumerators[i].MoveNext();
                    }
                    if (inComplete) yield return null;
                }

                Log.CurrentTime("ASYNC3 FINISH");
            }

            //4: number 3 but removing finished enumerators
            //slightly better than worst
            if (RA2Plugin.testAsyncLoading == 4)
            {
                Log.CurrentTime("ASYNC3.2 START");

                List<System.Collections.IEnumerator> enumerators = Survivors.Chrono.ChronoAssets.InitAsync2(Survivors.Chrono.ChronoSurvivor.instance.assetBundle);

                bool inComplete = true;
                while (inComplete)
                {
                    inComplete = false;
                    for (int i = enumerators.Count - 1; i >= 0; i--)
                    {
                        bool movingNext = enumerators[i].MoveNext();
                        if (!movingNext)
                        {
                            enumerators.RemoveAt(i);
                        }
                        inComplete |= movingNext;
                    }
                    if (inComplete) yield return null;
                }

                Log.CurrentTime("ASYNC3.2 FINISH");
            }

            //5: number 4 without an extra bool
            //slightly better than slightly better than worst
            if (RA2Plugin.testAsyncLoading == 5)
            {
                Log.CurrentTime("ASYNC3.2.2 START");

                List<System.Collections.IEnumerator> enumerators = Survivors.Chrono.ChronoAssets.InitAsync2(Survivors.Chrono.ChronoSurvivor.instance.assetBundle);

                while (enumerators.Count > 0)
                {
                    for (int i = enumerators.Count - 1; i >= 0; i--)
                    {
                        if (!enumerators[i].MoveNext())
                        {
                            enumerators.RemoveAt(i);
                        }
                    }
                    if (enumerators.Count > 0) yield return null;
                }

                Log.CurrentTime("ASYNC3.2.2 FINISH");
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