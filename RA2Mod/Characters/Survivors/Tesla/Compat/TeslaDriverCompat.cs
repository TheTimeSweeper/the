using RA2Mod.Modules;
using RA2Mod.Survivors.Tesla.SkillDefs;
using RA2Mod.Survivors.Tesla.States;
using RobDriver.Modules.Components;
using RoR2;
using RoR2.Skills;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RA2Mod.Survivors.Tesla.Compat
{
    internal class TeslaDriverCompat
    {
        private ushort teslaGunIndex;

        private static AssetBundle assetBundle => TeslaTrooperSurvivor.instance.assetBundle;

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public void Init()
        {
            Hooks.RoR2.SurvivorCatalog.SetSurvivorDefs_Driver += SurvivorCatalog_SetSurvivorDefs_Driver;

            //InitConfig();

            #region tokens
            Modules.Language.Add(TeslaTrooperSurvivor.TOKEN_PREFIX + "DRIVER_GUN_NAME", "Tesla Gauntlet");
            Modules.Language.Add(TeslaTrooperSurvivor.TOKEN_PREFIX + "DRIVER_GUN_DESCRIPTION", $"2000 volts coming up.");

            //Modules.Language.Add(TeslaTrooperSurvivor.TESLA_PREFIX + "PRIMARY_SHOOT_DRIVER_NAME", "Chrono Gun");
            //Modules.Language.Add(TeslaTrooperSurvivor.TESLA_PREFIX + "PRIMARY_SHOOT_DRIVER_DESCRIPTION", $"Fire for {Tokens.DamageValueText(DriverCompat.DriverGunM1Damage.Value)} and apply {Tokens.UtilityText("Chrono Sickness")} to enemies.");

            //int driverTicks = (int)(DriverCompat.DriverGunM2Duration.Value / DriverCompat.DriverGunM2TickInterval.Value);
            //Modules.Language.Add(TeslaTrooperSurvivor.TESLA_PREFIX + "SPECIAL_VANISH_DRIVER_NAME", "Deconstructing");
            //Modules.Language.Add(TeslaTrooperSurvivor.TESLA_PREFIX + "SPECIAL_VANISH_DRIVER_DESCRIPTION", $"Focus your rifle for up to {Tokens.DamageValueText(DriverCompat.DriverGunM2Damage.Value * driverTicks)}. An enemy below the {Tokens.UtilityText("Chrono Sickness")} threshold will vanish from existence.");
            #endregion tokens

            if (General.GeneralConfig.Debug.Value)
            {
                On.RoR2.CharacterBody.Update += CharacterBody_Update;
            }
        }

        private void SurvivorCatalog_SetSurvivorDefs_Driver(GameObject driverBody)
        {
            driverBody.AddComponent<TeslaTrackerComponent>();
            driverBody.AddComponent<TeslaTrackerComponentZap>();
            Log.Debug("found driver. adding tracker");
            DoDriverCompat();
        }

        private void CharacterBody_Update(On.RoR2.CharacterBody.orig_Update orig, CharacterBody self)
        {
            orig(self);
            if (Input.GetKeyDown(KeyCode.H))
            {
                if (self.TryGetComponent(out DriverController cantdrive55))
                {
                    cantdrive55.PickUpWeapon(RobDriver.DriverWeaponCatalog.GetWeaponFromIndex(teslaGunIndex));
                }
            }
        }

        private void DoDriverCompat()
        {
            //chronoIndicatorVanishDriver = assetBundle.LoadAsset<GameObject>("IndicatorChronoVanishDriver");

            TeslaTrackingSkillDef primarySkillDefZap =           //this constructor creates a skilldef for a typical primary
                Skills.CreateSkillDef<TeslaTrackingSkillDef>(new SkillDefInfo("Tesla_Primary_Zap",
                                                                              TeslaTrooperSurvivor.TOKEN_PREFIX + "PRIMARY_ZAP_NAME",
                                                                              TeslaTrooperSurvivor.TOKEN_PREFIX + "PRIMARY_ZAP_DESCRIPTION",
                                                                              assetBundle.LoadAsset<Sprite>("texTeslaSkillPrimary"),
                                                                              new EntityStates.SerializableEntityStateType(typeof(Zap)),
                                                                              "Weapon",
                                                                              false));

            primarySkillDefZap.keywordTokens = new string[] { "KEYWORD_CHARGED" };

            SkillDef bigZapSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Tesla_Secondary_BigZap",
                skillNameToken = TeslaTrooperSurvivor.TOKEN_PREFIX + "SECONDARY_BIGZAP_NAME",
                skillDescriptionToken = TeslaTrooperSurvivor.TOKEN_PREFIX + "SECONDARY_BIGZAP_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texTeslaSkillSecondary"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(AimBigZap)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 5.5f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_STUNNING", "KEYWORD_SHOCKING" }
            });

            DriverWeaponDef teslaGunWeaponDef = DriverWeaponDef.CreateWeaponDefFromInfo(new DriverWeaponDefInfo
            {
                nameToken = TeslaTrooperSurvivor.TOKEN_PREFIX + "DRIVER_GUN_NAME",
                descriptionToken = TeslaTrooperSurvivor.TOKEN_PREFIX + "DRIVER_GUN_DESCRIPTION",
                icon = assetBundle.LoadAsset<Texture2D>("texIconTeslaRA2"),
                crosshairPrefab = TeslaTrooperSurvivor.instance.prefabCharacterBody.defaultCrosshairPrefab,
                tier = DriverWeaponTier.Uncommon,
                shotCount = 48,
                primarySkillDef = primarySkillDefZap,
                secondarySkillDef = bigZapSkillDef,
                mesh = assetBundle.LoadAsset<Mesh>("meshDriverTeslaGauntlet"),
                material = assetBundle.LoadAsset<Material>("matTesla_original_Armor"),
                animationSet = DriverWeaponDef.AnimationSet.Default,
                calloutSoundString = "Play_Tesla_Voiceline_Driver",
                configIdentifier = "Tesla Trooper Gauntlet"
            });
            RobDriver.DriverWeaponCatalog.AddWeapon(teslaGunWeaponDef);

            teslaGunIndex = teslaGunWeaponDef.index;
        }
    }
}
