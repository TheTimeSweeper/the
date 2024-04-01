using BepInEx;
using PlagueMod.Survivors.Plague;
using R2API.Utils;
using RoR2;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Permissions;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

//rename this namespace
namespace PlagueMod
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]

    public class PlaguePlugin : BaseUnityPlugin
    {
        // if you don't change these you're giving permission to deprecate the mod-
        //  please change the names to your own stuff, thanks
        //   this shouldn't even have to be said
        public const string MODUID = "com.thetimesweeper.PlagueKnightMod";
        public const string MODNAME = "PlagueKnightMod";
        public const string MODVERSION = "0.0.1";

        // a prefix for name tokens to prevent conflicts- please capitalize all name tokens for convention
        public const string DEVELOPER_PREFIX = "HABIBI";

        public static PlaguePlugin instance;

        void Awake()
        {
            instance = this;

            Log.Init(Logger);
            Modules.Language.Init();
            
            // collect item display prefabs for use in our display rules
            Modules.ItemDisplays.PopulateDisplays();
            
            // character initialization. this should be after itemdisplays
            new PlagueSurvivor().Initialize();

            // now make a content pack and add it this has to be last
            new Modules.ContentPacks().Initialize();
        }
    }
}
