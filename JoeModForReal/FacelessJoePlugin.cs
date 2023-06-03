using BepInEx;
using BepInEx.Logging;
using System;
using JoeModForReal.Content.Survivors;
using System.Security;
using System.Security.Permissions;
using R2API.Utils;
using System.Collections;
using UnityEngine;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace JoeModForReal {

    [BepInPlugin(MODUID, MODNAME, MODVERSION)]

    [BepInDependency("com.rune580.riskofoptions", BepInDependency.DependencyFlags.SoftDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    public class FacelessJoePlugin : BaseUnityPlugin {
        public const string MODUID = "com.TheTimeSweeper.FacelessJoe";
        public const string MODNAME = "Faceless Joe";
        public const string MODVERSION = "0.1.0";

        // a prefix for name tokens to prevent conflicts- please capitalize all name tokens for convention
        public const string DEV_PREFIX = "HABIBI";

        public static FacelessJoePlugin instance;
        public static ManualLogSource Log;

        public static bool andrew;

        void Awake() {

            instance = this;
            Log = Logger;

            Modules.Files.Init(Info);
            Modules.Language.HookRegisterLanguageTokens();

            Modules.Config.ReadConfig();
            andrew &= Modules.Config.Debug;

            if (Modules.Config.Debug)
                Modules.Tokens.GenerateTokens();
        }

        private void Start() {

            Logger.LogInfo("[Initializing Joe]");

            if (Modules.Config.Debug)
                gameObject.AddComponent<TestValueManager>();

            Modules.SoundBanks.Init();

            Modules.DamageTypes.RegisterDamageTypes();

            Modules.Assets.Initialize();
            Modules.Projectiles.Init();
            Modules.EntityStates.Init();

            Modules.Compat.Initialize();

            Modules.ItemDisplays.PopulateDisplays(); // collect item display prefabs for use in our display rules

            Modules.Buffs.RegisterBuffs(); // add and register custom buffs/debuffs
            Modules.Dots.RegisterDots();

            Modules.Assets.LateInitialize();

            new Modules.ContentPacks().Initialize();

            new JoeSurivor().Initialize();

            Hook();

            Logger.LogInfo("[Initialized]");
        }
        
        private void Hook() {
            RoR2.GlobalEventManager.onCharacterDeathGlobal += GlobalEventManager_onCharacterDeathGlobal;
        }

        private void GlobalEventManager_onCharacterDeathGlobal(RoR2.DamageReport damageReport) {

            if (Modules.Config.jerry.Value) {

                Helpers.LogWarning(damageReport.victimBody);
                if (damageReport.victimBody) {
                    RoR2.Util.PlaySound("play_joe_jerryDeath", damageReport.victimBody.gameObject);
                }
            }
        }
    }
}
