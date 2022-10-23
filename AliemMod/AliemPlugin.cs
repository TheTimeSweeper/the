using BepInEx;
using BepInEx.Logging;
using Modules.Survivors;
using R2API.Utils;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

//[BepInDependency("com.cwmlolzlz.skills", BepInDependency.DependencyFlags.SoftDependency)]

[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
[BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
[R2APISubmoduleDependency(new string[]
{
    "PrefabAPI",
    "LanguageAPI",
    "SoundAPI",
    "LoadoutAPI",
    //"DeployableAPI",
    "DamageAPI",
    "UnlockableAPI",
    "RecalculateStatsAPI",
    //"DotAPI"
})]

[BepInPlugin(MODUID, MODNAME, MODVERSION)]

public class AliemPlugin : BaseUnityPlugin {
    public const string MODUID = "com.TheTimeSweeper.Aliem";
    public const string MODNAME = "Aliem";
    public const string MODVERSION = "0.1.0";

    // a prefix for name tokens to prevent conflicts- please capitalize all name tokens for convention
    public const string DEV_PREFIX = "HABIBI";
    
    public static AliemPlugin instance;
    public static ManualLogSource Log;

    public static bool Desolator;
    public static bool holdonasec;

    private void Start() {

        Logger.LogInfo("[Initializing Aliem]");
        
        instance = this;
        
        Log = Logger;

        Modules.DamageTypes.RegisterDamageTypes();

        // load assets and read config
        Modules.Config.ReadConfig();

        Modules.Assets.Initialize();
        Modules.Projectiles.Init();
        Modules.EntityStates.Init();

        //if (Modules.Config.Debug)
        //    gameObject.AddComponent<TestValueManager>();

        //Modules.Compat.Initialize();
        Modules.Tokens.AddTokens(); // register name tokens
        Modules.ItemDisplays.PopulateDisplays(); // collect item display prefabs for use in our display rules

        Modules.Buffs.RegisterBuffs(); // add and register custom buffs/debuffs
        Modules.Dots.RegisterDots();

        new Modules.ContentPacks().Initialize();

        Hook();

        new AliemMod.Content.Survivors.AliemSurvivor().Initialize();

        Logger.LogInfo("[Initialized]");
    }

    private void Hook() {

        R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
    }

    private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, R2API.RecalculateStatsAPI.StatHookEventArgs args) {

        if (sender.HasBuff(Modules.Buffs.riddenBuff)) {
            args.moveSpeedMultAdd += 1.0f;
        }
    }
}
