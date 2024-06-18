using AliemMod.Content;
using AliemMod.Modules;
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

[BepInPlugin(MODUID, MODNAME, MODVERSION)]

public class AliemPlugin : BaseUnityPlugin {
    public const string MODUID = "com.TheTimeSweeper.Aliem";
    public const string MODNAME = "Aliem";
    public const string MODVERSION = "0.9.2";

    // a prefix for name tokens to prevent conflicts- please capitalize all name tokens for convention
    public const string DEV_PREFIX = "HABIBI";
    
    public static AliemPlugin instance;
    public static ManualLogSource Log;

    private void Start() {

        Logger.LogInfo("[Initializing Aliem]");
        
        instance = this;
        
        Log = Logger;

        AliemMod.Modules.Config.MyConfig = Config;

        DamageTypes.RegisterDamageTypes();

        // load assets and read config
        AliemConfig.ReadConfig();

        if (AliemConfig.Debug.Value)
            gameObject.AddComponent<TestValueManager>();

        Assets.Initialize();
        Projectiles.Init();
        States.Init();

        //if (Modules.Config.Debug)
        //    gameObject.AddComponent<TestValueManager>();

        Compat.Initialize();
        Tokens.AddTokens(); // register name tokens
        ItemDisplays.PopulateDisplays(); // collect item display prefabs for use in our display rules

        Buffs.RegisterBuffs(); // add and register custom buffs/debuffs
        Dots.RegisterDots();

        new ContentPacks().Initialize();

        Hook();

        new AliemMod.Content.Survivors.AliemSurvivor().Initialize();

        Logger.LogInfo("[Initialized]");
    }

    private void Hook() {

        R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
    }

    private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, R2API.RecalculateStatsAPI.StatHookEventArgs args) {
        
        if (sender.HasBuff(Buffs.riddenBuff)) {
            args.moveSpeedMultAdd += 1.3f;
            //args.attackSpeedMultAdd += 1.2f;
        }

        if (sender.HasBuff(Buffs.diveBuff))
        {
            args.armorAdd += AliemConfig.M3_Leap_Armor.Value;
        }

        if (sender.HasBuff(Buffs.ridingBuff))
        {
            args.armorAdd += AliemConfig.M3_Leap_RidingArmor.Value;
        }

        if (sender.HasBuff(Buffs.attackSpeedBuff))
        {
            args.attackSpeedMultAdd += 1;
        }
    }
}
