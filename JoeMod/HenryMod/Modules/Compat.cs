using System;

namespace HenryMod.Modules {
    public class Compat {

        public static bool skillsPlusInstalled;

        public static void Initialize() {
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.cwmlolzlz.skills")) {
                skillsPlusInstalled = true;
                SkillsPlusCompat.init();
            }
        }
    }
}