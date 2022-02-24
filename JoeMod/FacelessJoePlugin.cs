using BepInEx;
using BepInEx.Logging;
using HenryMod.Modules.Survivors;
using R2API.Utils;
using RoR2;
using System;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace HenryMod {

    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]
    [R2APISubmoduleDependency(new string[]
    {
        "PrefabAPI",
        "LanguageAPI",
        "SoundAPI",
    })]

    //todo: separatable plugin
    public class FacelessJoePlugin : BaseUnityPlugin
    {
        public const string MODUID = "com.TheTimeSweeper.FacelessJoe";
        public const string MODNAME = "Tesla Trooper";
        public const string MODVERSION = "0.1.0";

        // a prefix for name tokens to prevent conflicts- please capitalize all name tokens for convention
        public const string developerPrefix = "HABIBI";

        internal List<SurvivorBase> Survivors = new List<SurvivorBase>();

        public static FacelessJoePlugin instance;
        public static ManualLogSource Log;

        private void Awake()
        {
            instance = this;
            Log = Logger;
            gameObject.AddComponent<TestValueManager>();

            // load assets and read config
            Modules.Assets.Initialize();
            Modules.Config.ReadConfig();
            Modules.Compat.Initialize();
            Modules.States.RegisterStates(); // register states for networking
            Modules.Buffs.RegisterBuffs(); // add and register custom buffs/debuffs
            Modules.Projectiles.RegisterProjectiles(); // add and register custom projectiles
            Modules.Tokens.AddTokens(); // register name tokens
            Modules.ItemDisplays.PopulateDisplays(); // collect item display prefabs for use in our display rules

            // survivor initialization
            new JoeSurivor().Initialize();

            new TeslaTowerNotSurvivor().Initialize();

            //todo compiler flags when
            new TeslaTrooperSurvivor().Initialize();

            // now make a content pack and add it- this part will change with the next update
            new Modules.ContentPacks().Initialize();
            
            RoR2.ContentManagement.ContentManager.onContentPacksAssigned += LateSetup;

            Hook();
        }

        private void LateSetup(HG.ReadOnlyArray<RoR2.ContentManagement.ReadOnlyContentPack> obj)
        {
            // have to set item displays later now because they require direct object references..
            //Modules.Survivors.JoeSurivor.instance.SetItemDisplays();
            //todo compiler flags when
            Modules.Survivors.TeslaTrooperSurvivor.instance.SetItemDisplays();
        }

        private void Hook()
        {
            // run hooks here, disabling one is as simple as commenting out the line
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;

            On.EntityStates.Commando.CommandoWeapon.ThrowGrenade.PlayAnimation += ThrowGrenade_PlayAnimation;

            On.RoR2.CharacterMaster.AddDeployable += CharacterMaster_AddDeployable;
            On.RoR2.Inventory.CopyItemsFrom_Inventory_Func2 += Inventory_CopyItemsFrom_Inventory_Func2; ;
            //On.RoR2.MasterSummon.Perform += MasterSummon_Perform;
            //On.RoR2.CharacterBody.HandleConstructTurret += CharacterBody_HandleConstructTurret;
        }

        #region tower hacks
        private void Inventory_CopyItemsFrom_Inventory_Func2(On.RoR2.Inventory.orig_CopyItemsFrom_Inventory_Func2 orig, Inventory self, Inventory other, Func<ItemIndex, bool> filter) {
            if (MasterCatalog.FindMasterIndex(self.gameObject) == MasterCatalog.FindMasterIndex(TeslaTowerNotSurvivor.masterPrefab)) {
                Helpers.LogWarning("copyitemsfrom true");
                filter = TeslaTowerCopyFilter;
            }
            orig(self, other, filter);
        }
        private void CharacterMaster_AddDeployable(On.RoR2.CharacterMaster.orig_AddDeployable orig, CharacterMaster self, Deployable deployable, DeployableSlot slot) {
            if(MasterCatalog.FindMasterIndex(deployable.gameObject) == MasterCatalog.FindMasterIndex(TeslaTowerNotSurvivor.masterPrefab)) {
                Helpers.LogWarning("adddeployable true");
                slot = DeployableSlot.PowerWard;
            }

            orig(self, deployable, slot);
        }

        private CharacterMaster MasterSummon_Perform(On.RoR2.MasterSummon.orig_Perform orig, MasterSummon self) {

            if(MasterCatalog.FindMasterIndex(self.masterPrefab) == MasterCatalog.FindMasterIndex(TeslaTowerNotSurvivor.masterPrefab)) {
                Helpers.LogWarning("mastersummon true");
                self.inventoryItemCopyFilter = new Func<ItemIndex, bool>(TeslaTowerCopyFilter);
            }
            return orig(self);
        }

        private static bool TeslaTowerCopyFilter(ItemIndex itemIndex) {
            return !ItemCatalog.GetItemDef(itemIndex).ContainsTag(ItemTag.CannotCopy) && 
                (ItemCatalog.GetItemDef(itemIndex).ContainsTag(ItemTag.Damage) || 
                ItemCatalog.GetItemDef(itemIndex).ContainsTag(ItemTag.OnKillEffect));
            //return ItemCatalog.GetItemDef(itemIndex).ContainsTag(ItemTag.Damage);
        }
#endregion tower hacks
        private void CharacterBody_HandleConstructTurret(On.RoR2.CharacterBody.orig_HandleConstructTurret orig, UnityEngine.Networking.NetworkMessage netMsg) {
            orig(netMsg);
        }

        private void ThrowGrenade_PlayAnimation(On.EntityStates.Commando.CommandoWeapon.ThrowGrenade.orig_PlayAnimation orig, global::EntityStates.Commando.CommandoWeapon.ThrowGrenade self, float duration) {
            orig(self, duration);
            Chat.AddMessage(self.projectilePitchBonus.ToString());
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);

            // a simple stat hook, adds armor after stats are recalculated
            // todo recalculatestatsapi
                // this is getting messy
                // but if it workssss
            if (self)
            {
                if (self.HasBuff(Modules.Buffs.armorBuff))
                {
                    self.armor += 300f;
                }
            }
        }
    }
}