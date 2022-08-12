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

[BepInDependency("com.cwmlolzlz.skills", BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency("com.xoxfaby.BetterUI", BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency("com.KomradeSpectre.Aetherium", BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency("com.ThinkInvisible.TinkersSatchel", BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency("com.DestroyedClone.AncientScepter", BepInDependency.DependencyFlags.SoftDependency)]

[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
[BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
[R2APISubmoduleDependency(new string[]
{
    "PrefabAPI",
    "LanguageAPI",
    "SoundAPI",
    "LoadoutAPI",
    "DeployableAPI",
    "DamageAPI",
    "UnlockableAPI"
})]

[BepInPlugin(MODUID, MODNAME, MODVERSION)]

public class FacelessJoePlugin : BaseUnityPlugin {
    public const string MODUID = "com.TheTimeSweeper.TeslaTrooper";
    public const string MODNAME = "Tesla Trooper";
    public const string MODVERSION = "1.1.2";

    // a prefix for name tokens to prevent conflicts- please capitalize all name tokens for convention
    public const string DEV_PREFIX = "HABIBI";

    internal List<SurvivorBase> Survivors = new List<SurvivorBase>();

    public static FacelessJoePlugin instance;
    public static ManualLogSource Log;

    public static bool conductiveMechanic => conductiveAlly || conductiveEnemy;
    public static bool conductiveEnemy = false;
    public static bool conductiveAlly = true;

    private void Start() {

        Logger.LogInfo("[Initializing Tesla Trooper]");

        instance = this;

        Log = Logger;

        // load assets and read config
        Modules.Assets.Initialize();
        Modules.Config.ReadConfig();
        if (Modules.Config.Debug)
            gameObject.AddComponent<TestValueManager>();

        Modules.Compat.Initialize();
        Modules.States.RegisterStates(); // register states for networking
        Modules.Buffs.RegisterBuffs(); // add and register custom buffs/debuffs
        //Modules.Projectiles.RegisterProjectiles(); // add and register custom projectiles
        Modules.Tokens.AddTokens(); // register name tokens
        Modules.ItemDisplays.PopulateDisplays(); // collect item display prefabs for use in our display rules

        Modules.DamageTypes.RegisterDamageTypes();
        Modules.Buffs.RegisterBuffs();

        // survivor initialization
        //new JoeSurivor().Initialize();

        new TeslaTowerNotSurvivor().Initialize();       
        new TeslaTrooperSurvivor().Initialize();

        new Modules.ContentPacks().Initialize();

        Hook();

        Logger.LogInfo("[Initialized]");
    }

    private void Hook() {
        // run hooks here, disabling one is as simple as commenting out the line
        On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;

        //for figuring out plague knight throw bomb angles
        //On.EntityStates.Commando.CommandoWeapon.ThrowGrenade.PlayAnimation += ThrowGrenade_PlayAnimation;

        On.RoR2.CharacterBody.Awake += CharacterBody_Awake;
    }

    private void CharacterBody_Awake(On.RoR2.CharacterBody.orig_Awake orig, CharacterBody self) {
        orig(self);
        self.gameObject.AddComponent<SkillTracker>().Init(self);
    }

    private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo) {

        bool flag = (damageInfo.damageType & DamageType.BypassArmor) > DamageType.Generic;

        if (self && self.body && self.body.HasBuff(Modules.Buffs.zapShieldBuff) && !flag) {
            damageInfo.rejected = true;

            //yes basically copied from the old barrier code
            IBarrier bar = self.GetComponent<IBarrier>();
            if (bar!= null) {
                bar.BlockedDamage(damageInfo, damageInfo.damage); //I guess I would need IL for this
            }
        }
        orig(self, damageInfo);
    }

    private void ThrowGrenade_PlayAnimation(On.EntityStates.Commando.CommandoWeapon.ThrowGrenade.orig_PlayAnimation orig, global::EntityStates.Commando.CommandoWeapon.ThrowGrenade self, float duration) {
        orig(self, duration);
        Chat.AddMessage(self.projectilePitchBonus.ToString());
    }
}
