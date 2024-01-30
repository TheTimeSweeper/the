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
        public const string MODVERSION = "0.1.0";

        public const string DEVELOPER_PREFIX = "HABIBI";

        public static RA2Plugin instance;

        public static bool testAsyncLoading = true;

        void Start()
        {
            Modules.SoundBanks.Init();
        }

        void Awake()
        {
            instance = this;

            Log.Init(Logger);
            GeneralConfig.Init();
            Log.CurrentTime("START " + (testAsyncLoading ? "async" : "sync"));

            Modules.Language.Init();

            new ChronoSurvivor().Initialize();

            new Modules.ContentPacks().Initialize();
        }
    }
}
