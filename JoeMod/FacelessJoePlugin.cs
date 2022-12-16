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
using UnityEngine;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

[BepInDependency("com.cwmlolzlz.skills", BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency("com.xoxfaby.BetterUI", BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency("com.KomradeSpectre.Aetherium", BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency("com.ThinkInvisible.TinkersSatchel", BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency("com.DestroyedClone.AncientScepter", BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency("com.DrBibop.VRAPI", BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency("com.weliveinasociety.CustomEmotesAPI", BepInDependency.DependencyFlags.SoftDependency)]

[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
[BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
[R2APISubmoduleDependency(new string[]
{
    "PrefabAPI",
    //"LanguageAPI",
    //"SoundAPI",
    "LoadoutAPI",
    "DeployableAPI",
    "DamageAPI",
    "UnlockableAPI",
    "RecalculateStatsAPI",
    "DotAPI"
})]

[BepInPlugin(MODUID, MODNAME, MODVERSION)]

public class FacelessJoePlugin : BaseUnityPlugin {
    public const string MODUID = "com.TheTimeSweeper.TeslaTrooper";
    public const string MODNAME = "Tesla Trooper";
    public const string MODVERSION = "2.0.0";
    
    // a prefix for name tokens to prevent conflicts- please capitalize all name tokens for convention
    public const string DEV_PREFIX = "HABIBI";
    
    public static FacelessJoePlugin instance;
    public static ManualLogSource Log;

    public static bool Desolator;
    public static bool holdonasec;

    void Awake() {

        instance = this;
        Log = Logger;

        Modules.Files.Init(Info);
        Modules.Language.HookRegisterLanguageTokens();

        Modules.Config.ReadConfig();
    }

    private void Start() {

        Logger.LogInfo("[Initializing Tesla Trooper]");

        Modules.DamageTypes.RegisterDamageTypes();

        // load assets and read config

        Modules.Assets.Initialize();
        Modules.SoundBanks.Init();

        if (Modules.Config.Debug)
            gameObject.AddComponent<TestValueManager>();

        Modules.Compat.Initialize();
        //Modules.Projectiles.RegisterProjectiles(); // add and register custom projectiles
        
        //if (Modules.Config.Debug)
        //    Modules.Tokens.GenerateTokens();

        Modules.States.RegisterStates(); // register states for networking
        Modules.ItemDisplays.PopulateDisplays(); // collect item display prefabs for use in our display rules

        Modules.Buffs.RegisterBuffs(); // add and register custom buffs/debuffs
        Modules.Dots.RegisterDots();


        // survivor initialization

        //someday
        //new JoeSurivor().Initialize();

        //init towers first
        TeslaTowerNotSurvivor baseTower = new TeslaTowerNotSurvivor();
        baseTower.Initialize();
        new TeslaTowerScepter().Initialize(baseTower);
        //the guy
        new TeslaTrooperSurvivor().Initialize();

        if (Desolator) {
            new DesolatorSurvivor().Initialize();
        }

        new Modules.ContentPacks().Initialize();

        Hook();

        Logger.LogInfo("[Initialized]");
    }

    private void Hook() {
        On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;

        R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;

        //On.RoR2.CharacterModel.UpdatePoisonAffix += CharacterModel_UpdatePoisonAffix;
        On.RoR2.JitterBones.RebuildBones += JitterBones_RebuildBones;
        //for figuring out plague knight throw bomb angles
        //On.EntityStates.Commando.CommandoWeapon.ThrowGrenade.PlayAnimation += ThrowGrenade_PlayAnimation;
    }

    private void JitterBones_RebuildBones(On.RoR2.JitterBones.orig_RebuildBones orig, JitterBones self) {
        if (self._skinnedMeshRenderer && self._skinnedMeshRenderer.name == "Tower_Base_Pillars_Color")
            return;
        orig(self);
    }

    //private void CharacterModel_UpdatePoisonAffix(On.RoR2.CharacterModel.orig_UpdatePoisonAffix orig, CharacterModel self) {
    //    if(self.body.baseNameToken == TeslaTowerNotSurvivor.TOWER_PREFIX + "NAME" || 
    //       self.body.baseNameToken == TeslaTowerScepter.TOWER_SCEPTER_PREFIX + "NAME") {
    //        return;
    //    }
    //    orig(self);
    //}

    private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, R2API.RecalculateStatsAPI.StatHookEventArgs args) {
        if (Desolator) {
            if (sender.HasBuff(Modules.Buffs.desolatorArmorBuff)) {
                args.armorAdd += 100f;
                args.moveSpeedMultAdd += 0.4f;
            }

            if (sender.HasBuff(Modules.Buffs.desolatorDeployBuff)) {
                args.armorAdd += 30f;
            }

            if (sender.HasBuff(Modules.Buffs.desolatorArmorShredDeBuff)) {
                args.armorAdd -= DesolatorSurvivor.ArmorShredAmount * sender.GetBuffCount(Modules.Buffs.desolatorArmorShredDeBuff);
            }

            //if (sender.HasBuff(Modules.Buffs.desolatorDotDeBuff)) {
            //    args.armorAdd -= 3f * sender.GetBuffCount(Modules.Buffs.desolatorDotDeBuff);
            //}
        }
    }

    private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo) {
        
        bool bypassArmor = (damageInfo.damageType & DamageType.BypassArmor) > DamageType.Generic;
        if (self && self.body) {
            if (self.body.HasBuff(Modules.Buffs.zapShieldBuff) && !bypassArmor) {
                float mitigatedDamage = damageInfo.damage;

                if (Modules.Config.UtilityDamageAbsorption >= 1.0f) {
                    damageInfo.rejected = true;
                } else {
                    mitigatedDamage = (1.0f - Modules.Config.UtilityDamageAbsorption) * damageInfo.damage;
                }

                IReflectionBarrier bar = self.GetComponent<IReflectionBarrier>();
                if (bar != null) {
                    bar.StoreDamage(damageInfo, damageInfo.damage);
                    damageInfo.damage = mitigatedDamage;
                }
            }

            if(Desolator && self.body.HasBuff(Modules.Buffs.desolatorDeployBuff)){
                damageInfo.force = Vector3.zero;
            }
        }
        orig(self, damageInfo);
    }

    private void ThrowGrenade_PlayAnimation(On.EntityStates.Commando.CommandoWeapon.ThrowGrenade.orig_PlayAnimation orig, EntityStates.Commando.CommandoWeapon.ThrowGrenade self, float duration) {
        orig(self, duration);
        Chat.AddMessage(self.projectilePitchBonus.ToString());
    }
}
