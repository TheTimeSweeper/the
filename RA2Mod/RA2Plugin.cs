using BepInEx;
using R2API.Utils;
using RA2Mod.General;
using RA2Mod.Survivors.Chrono;
using RA2Mod.Survivors.GI;
//using RA2Mod.Survivors.Tesla;
//using RA2Mod.Minions.TeslaTower;
using System.Security;
using System.Security.Permissions;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace RA2Mod
{
    [BepInDependency("com.rune580.riskofoptions", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.johnedwa.RTAutoSprintEx", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.cwmlolzlz.skills", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.xoxfaby.BetterUI", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.KomradeSpectre.Aetherium", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.ThinkInvisible.TinkersSatchel", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.DestroyedClone.AncientScepter", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.DrBibop.VRAPI", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.weliveinasociety.CustomEmotesAPI", BepInDependency.DependencyFlags.SoftDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]
    public class RA2Plugin : BaseUnityPlugin
    {
        public const string MODUID = "com.thetimesweeper.ra2mod";
        public const string MODNAME = "RA2Mod";
        public const string MODVERSION = "0.5.1";

        public const string DEVELOPER_PREFIX = "HABIBI";

        public static RA2Plugin instance;

        public RA2Plugin()
        {
            if (Log._startTime == default(System.DateTime))
            {
                Log._startTime = System.DateTime.Now;
            }
        }

        void Start()
        {
            Modules.SoundBanks.Init();
        }

        void Awake()
        {
            instance = this;
            Log.Init(Logger);

            GeneralConfig.Init();
            GeneralCompat.Init();
            GeneralStates.Init();
            GeneralHooks.Init();

            Log.CurrentTime("START new");

            Modules.Language.Init();

            new ChronoSurvivor().Initialize();
            new GISurvivor().Initialize();
            //new TeslaTrooperSurvivor().Initialize();
            new Modules.ContentPacks().Initialize();

            if (GeneralConfig.Debug.Value)
            {
                UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            }
        }

        private void SceneManager_sceneLoaded(UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.LoadSceneMode arg1)
        {
            if (arg0.name == "title")
            {
                Log.CurrentTime("TITLE SCREEN");
                Log.AllTimes();
            }
        }
    }
}
