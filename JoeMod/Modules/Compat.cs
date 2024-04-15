﻿using RoR2;
using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace Modules {

    public class Compat {

        public static bool TinkersSatchelInstalled;
        public static bool AetheriumInstalled;
        public static bool ScepterInstalled;
        public static bool VREnabled;

        public static void Initialize() {
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.cwmlolzlz.skills")) {
                SkillsPlusCompat.init();
            }
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.xoxfaby.BetterUI")) {
                BetterUICompat.init();
            }
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.ThinkInvisible.TinkersSatchel")) {
                TinkersSatchelInstalled = true;
            }
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.KomradeSpectre.Aetherium")) {
                AetheriumInstalled = true;
            }
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.DestroyedClone.AncientScepter")) {
                ScepterInstalled = true;
            }
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.DrBibop.VRAPI")) {
                VRCompat.init();
            }
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.weliveinasociety.CustomEmotesAPI") ) {
                MemeCompat.init();
            }
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.johnedwa.RTAutoSprintEx")) {
                TryAutoSprintCompat();
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static void TryAutoSprintCompat() {
            TeslaTrooperPlugin.instance.StartCoroutine(AutoSprintCompat());
        }

        private static IEnumerator AutoSprintCompat() {
            //let awake happen
            yield return null;
            TeslaTrooperPlugin.instance.GetComponent<RTAutoSprintEx.RTAutoSprintEx>().RT_SprintDisableMessage("ModdedEntityStates.TeslaTrooper.AimBigZap");
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