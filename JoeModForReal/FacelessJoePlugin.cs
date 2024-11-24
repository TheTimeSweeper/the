using BepInEx;
using BepInEx.Logging;
using JoeModForReal.Content.Survivors;
using R2API.Utils;
using RoR2;
using System;
using System.Security;
using System.Security.Permissions;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace JoeModForReal {

    [BepInPlugin(MODUID, MODNAME, MODVERSION)]

    [BepInDependency("com.rune580.riskofoptions", BepInDependency.DependencyFlags.SoftDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    public class FacelessJoePlugin : BaseUnityPlugin {
        public const string MODUID = "com.TheTimeSweeper.FacelessJoe";
        public const string MODNAME = "Faceless Joe";
        public const string MODVERSION = "0.2.0";

        // a prefix for name tokens to prevent conflicts- please capitalize all name tokens for convention
        public const string DEV_PREFIX = "HABIBI";

        public static FacelessJoePlugin instance;
        public static ManualLogSource Log;

        //gnome would debug/prototype character stuff on a blank character called andrew
        public static bool andrew = true;

        void Awake() {

            instance = this;
            Log = Logger;

            Modules.Files.Init(Info);
            Modules.Language.HookRegisterLanguageTokens();

            Modules.Config.ReadConfig();
            //andrew &= Modules.Config.Debug;

            if (Modules.Config.Debug)
                Modules.Tokens.GenerateTokens();
        }

        private void Start() {

            Logger.LogInfo("[Initializing Joe]");

            if (Modules.Config.Debug)
                gameObject.AddComponent<TestValueManager>();

            Modules.DamageTypes.RegisterDamageTypes();

            Modules.Asset.Initialize();
            Modules.Projectiles.Init();
            Modules.EntityStates.Init();

            Modules.ItemDisplays.PopulateDisplays(); // collect item display prefabs for use in our display rules

            Modules.Buffs.RegisterBuffs(); // add and register custom buffs/debuffs
            Modules.Dots.RegisterDots();

            Modules.Asset.LateInitialize();

            new Modules.ContentPacks().Initialize();

            new JoeSurivor().Initialize();

            new GenjiSurvivor().Initialize();

            Modules.Compat.Initialize();

            if (andrew) {
                new KoalSurvivor().Initialize();
                Logger.LogError("ANDREW IS TRUE. DO NOT RELEASE");
            }

            Hook();

            Logger.LogInfo("[Initialized]");
        }
        
        private void Hook() {
            RoR2.GlobalEventManager.onCharacterDeathGlobal += GlobalEventManager_onCharacterDeathGlobal;

            //On.RoR2.UI.LoadoutPanelController.Rebuild += LoadoutPanelController_Rebuild;
            //On.RoR2.UI.CharacterSelectController.BuildSkillStripDisplayData += CharacterSelectController_BuildSkillStripDisplayData;
            //On.RoR2.UI.CharacterSelectController.RebuildLocal += CharacterSelectController_RebuildLocal;
        }

        private void CharacterSelectController_BuildSkillStripDisplayData(On.RoR2.UI.CharacterSelectController.orig_BuildSkillStripDisplayData orig, RoR2.UI.CharacterSelectController self, Loadout loadout, ValueType bodyInfo, object dest) {
            orig(self, loadout, bodyInfo, dest);
            ((RoR2.UI.CharacterSelectController.BodyInfo)bodyInfo).bodyPrefab.GetComponents<Components.ILoadoutDisplayStrip>();
        }

        //private void CharacterSelectController_RebuildLocal(On.RoR2.UI.CharacterSelectController.orig_RebuildLocal orig, RoR2.UI.CharacterSelectController self) {
        //    orig(self);

        //    Loadout loadout = Loadout.RequestInstance();

        //}

        private void LoadoutPanelController_Rebuild(On.RoR2.UI.LoadoutPanelController.orig_Rebuild orig, RoR2.UI.LoadoutPanelController self) {
            orig(self);

        }

        private void GlobalEventManager_onCharacterDeathGlobal(RoR2.DamageReport damageReport) {

            if (Modules.Config.jerry.Value) {

                if (damageReport.victimBody) {
                    RoR2.Util.PlaySound("play_joe_jerryDeath", damageReport.victimBody.gameObject);
                }
            }
        }
    }
}
