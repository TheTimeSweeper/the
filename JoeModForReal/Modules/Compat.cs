using RoR2;
using System;
using System.Runtime.CompilerServices;

namespace Modules {
    internal class Compat {

        public static bool TinkersSatchelInstalled => BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.ThinkInvisible.TinkersSatchel");
        public static bool AetheriumInstalled => BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.KomradeSpectre.Aetherium");
        public static bool ScepterInstalled => BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.DestroyedClone.AncientScepter");
        public static bool RiskOfOptionsInstalled => BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions");
        //public static bool VREnabled;

        public static void Initialize() {
            //if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.cwmlolzlz.skills")) {
            //    SkillsPlusCompat.init();
            //}
            //if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.xoxfaby.BetterUI")) {
            //    BetterUICompat.init();
            //}
            //if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.DrBibop.VRAPI")) {
            //    VRCompat.init();
            //}
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.weliveinasociety.CustomEmotesAPI")) {
                MemeCompat.init();
            }
        }


        internal static int TryGetScepterCount(Inventory inventory) {
            if (!ScepterInstalled)
                return 0;

            return GetScepterCount(inventory);
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static int GetScepterCount(Inventory inventory) {
            return inventory.GetItemCount(AncientScepter.AncientScepterItem.instance.ItemDef);
        }
    }
}