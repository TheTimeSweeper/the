using BepInEx;
using RA2Mod.Survivors.Chrono;
using R2API.Utils;
using RoR2;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;
using RA2Mod.General;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace RA2Mod
{
    [BepInDependency("com.johnedwa.RTAutoSprintEx", BepInDependency.DependencyFlags.SoftDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]
    public class RA2Plugin : BaseUnityPlugin
    {
        public const string MODUID = "com.thetimesweeper.ra2mod";
        public const string MODNAME = "RA2Mod";
        public const string MODVERSION = "0.0.0";

        public const string DEVELOPER_PREFIX = "HABIBI";

        public static RA2Plugin instance;

        public static bool testAsyncLoading = true;

        void Awake()
        {
            instance = this;
            
            Log.Init(Logger);
            Log.CurrentTime("START " + (testAsyncLoading? "async" : "sync"));

            GeneralConfig.Init();

            Modules.Language.Init();

            new ChronoSurvivor().Initialize();

            new Modules.ContentPacks().Initialize();
        }

        void Start()
        {
            Modules.SoundBanks.Init();
        }
    }
}
