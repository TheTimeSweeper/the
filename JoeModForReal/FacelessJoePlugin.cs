using BepInEx;
using BepInEx.Logging;
using System;
using JoeModForReal.Content.Survivors;
using System.Security;
using System.Security.Permissions;
using R2API.Utils;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace JoeModForReal {

    [BepInPlugin(MODUID, MODNAME, MODVERSION)]

    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    public class FacelessJoePlugin : BaseUnityPlugin {
        public const string MODUID = "com.TheTimeSweeper.FacelessJoe";
        public const string MODNAME = "Faceless Joe";
        public const string MODVERSION = "0.1.0";

        // a prefix for name tokens to prevent conflicts- please capitalize all name tokens for convention
        public const string DEV_PREFIX = "HABIBI";

        public static FacelessJoePlugin instance;
        public static ManualLogSource Log;

        void Awake() {

            instance = this;
            Log = Logger;

            Modules.Files.Init(Info);
            Modules.Language.HookRegisterLanguageTokens();

            Modules.Config.ReadConfig();
        }

        private void Start() {

            Logger.LogInfo("[Initializing Joe]");

            //if (Modules.Config.Debug)
            //    gameObject.AddComponent<TestValueManager>();

            Modules.Assets.Initialize();
            Modules.SoundBanks.Init();
            Modules.Projectiles.Init();
            Modules.EntityStates.Init();

            //Modules.Tokens.GenerateTokens();
            Modules.Compat.Initialize();
            Modules.ItemDisplays.PopulateDisplays(); // collect item display prefabs for use in our display rules

            Modules.Buffs.RegisterBuffs(); // add and register custom buffs/debuffs
            Modules.Dots.RegisterDots();

            new Modules.ContentPacks().Initialize();

            new JoeSurivor().Initialize();

            Logger.LogInfo("[Initialized]");
        }

    }
}
