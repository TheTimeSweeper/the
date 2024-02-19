using RoR2.Skills;
using System.Runtime.CompilerServices;

namespace RA2Mod.Modules
{
    public class Compat
    {
        public static bool driverInstalled => BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.rob.Driver");
        public static bool scepterInstalled => BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.DestroyedClone.AncientScepter");
    }
}
