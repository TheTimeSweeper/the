using RoR2;
using RoR2.Skills;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RA2Mod.General
{
    public class GeneralCompat
    {
        public delegate void Meme_SurivorCatalog_Init();
        public static event Meme_SurivorCatalog_Init Meme_OnSurvivorCatalog_Init;

        public delegate void Driver_SurvivorCatalog_SetSurvivorDefs(GameObject driverBody);
        public static event Driver_SurvivorCatalog_SetSurvivorDefs FuckWithDriver;

        public static bool TinkersSatchelInstalled;
        public static bool AetheriumInstalled;
        public static bool ScepterInstalled;
        public static bool VREnabled;

        public static bool driverInstalled => BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.rob.Driver");
        public static bool scepterInstalled => BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.DestroyedClone.AncientScepter");

        public static void Init()
        {
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.ThinkInvisible.TinkersSatchel"))
            {
                TinkersSatchelInstalled = true;
            }
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.KomradeSpectre.Aetherium"))
            {
                AetheriumInstalled = true;
            }
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.DestroyedClone.AncientScepter"))
            {
                ScepterInstalled = true;
            }
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.weliveinasociety.CustomEmotesAPI"))
            {
                On.RoR2.SurvivorCatalog.Init += SurvivorCatalog_Init;
            }
            if (driverInstalled)
            {
                On.RoR2.SurvivorCatalog.SetSurvivorDefs += SurvivorCatalog_SetSurvivorDefs;
            }
        }

        private static void SurvivorCatalog_SetSurvivorDefs(On.RoR2.SurvivorCatalog.orig_SetSurvivorDefs orig, SurvivorDef[] newSurvivorDefs)
        {
            orig(newSurvivorDefs);

            for (int i = 0; i < newSurvivorDefs.Length; i++)
            {
                if (newSurvivorDefs[i].bodyPrefab.name == "RobDriverBody")
                {
                    FuckWithDriver?.Invoke(newSurvivorDefs[i].bodyPrefab);
                    return;
                }
            }

            Log.Debug("no driver. ra2 compat failed");
        }

        private static void SurvivorCatalog_Init(On.RoR2.SurvivorCatalog.orig_Init orig)
        {
            Meme_OnSurvivorCatalog_Init?.Invoke();
            orig();
        }

        internal static int TryGetScepterCount(Inventory inventory)
        {
            if (!ScepterInstalled)
                return 0;

            return GetScepterCount(inventory);
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static int GetScepterCount(Inventory inventory)
        {
            return inventory.GetItemCount(AncientScepter.AncientScepterItem.instance.ItemDef);
        }
    }
}
