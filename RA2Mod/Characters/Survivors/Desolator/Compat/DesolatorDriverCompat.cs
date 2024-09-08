using RA2Mod.Modules;
using RA2Mod.Survivors.Desolator.States;
using RobDriver.Modules.Components;
using RoR2;
using RoR2.Skills;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RA2Mod.Survivors.Desolator.Compat
{
    internal class DesolatorDriverCompat
    {
        private ushort desolatorGunIndex;

        private static AssetBundle assetBundle => DesolatorSurvivor.instance.assetBundle;

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public void Init()
        {
            //InitConfig();

            #region tokens
            Modules.Language.Add(DesolatorSurvivor.TOKEN_PREFIX + "DRIVER_GUN_NAME", "Rad Cannon");
            Modules.Language.Add(DesolatorSurvivor.TOKEN_PREFIX + "DRIVER_GUN_DESCRIPTION", $"Make it glow.");

            //Modules.Language.Add(TeslaTrooperSurvivor.TESLA_PREFIX + "PRIMARY_SHOOT_DRIVER_NAME", "Chrono Gun");
            //Modules.Language.Add(TeslaTrooperSurvivor.TESLA_PREFIX + "PRIMARY_SHOOT_DRIVER_DESCRIPTION", $"Fire for {Tokens.DamageValueText(DriverCompat.DriverGunM1Damage.Value)} and apply {Tokens.UtilityText("Chrono Sickness")} to enemies.");

            //int driverTicks = (int)(DriverCompat.DriverGunM2Duration.Value / DriverCompat.DriverGunM2TickInterval.Value);
            //Modules.Language.Add(TeslaTrooperSurvivor.TESLA_PREFIX + "SPECIAL_VANISH_DRIVER_NAME", "Deconstructing");
            //Modules.Language.Add(TeslaTrooperSurvivor.TESLA_PREFIX + "SPECIAL_VANISH_DRIVER_DESCRIPTION", $"Focus your rifle for up to {Tokens.DamageValueText(DriverCompat.DriverGunM2Damage.Value * driverTicks)}. An enemy below the {Tokens.UtilityText("Chrono Sickness")} threshold will vanish from existence.");
            #endregion tokens

            Content.AddEntityState(typeof(DriverRadBeam));
            Content.AddEntityState(typeof(DriverDeployEnter));

            if (General.GeneralConfig.Debug.Value)
            {
                On.RoR2.CharacterBody.Update += CharacterBody_Update;
            }

            Hooks.RoR2.SurvivorCatalog.SetSurvivorDefs_Driver += SurvivorCatalog_SetSurvivorDefs_Driver;
        }

        private void CharacterBody_Update(On.RoR2.CharacterBody.orig_Update orig, CharacterBody self)
        {
            orig(self);
            if (Input.GetKeyDown(KeyCode.J))
            {
                if (self.TryGetComponent(out DriverController cantdrive55))
                {
                    cantdrive55.PickUpWeapon(RobDriver.DriverWeaponCatalog.GetWeaponFromIndex(desolatorGunIndex));
                }
            }
        }

        private void SurvivorCatalog_SetSurvivorDefs_Driver(GameObject driverBody)
        {
            DoDriverCompat();
        }

        private void DoDriverCompat()
        {
            SkillDef primarySkillDefBeam =
                Skills.CreateSkillDef(new SkillDefInfo("Desolator_Driver_Primary_Beam",
                                                       DesolatorSurvivor.TOKEN_PREFIX + "PRIMARY_BEAM_NAME",
                                                       DesolatorSurvivor.TOKEN_PREFIX + "PRIMARY_BEAM_DESCRIPTION",
                                                       assetBundle.LoadAsset<Sprite>("texDesolatorSkillPrimary"),
                                                       new EntityStates.SerializableEntityStateType(typeof(DriverRadBeam)),
                                                       "Weapon",
                                                       false));

            SkillDef deploySkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Desolator_Driver_Secondary_Deploy",
                skillNameToken = DesolatorSurvivor.TOKEN_PREFIX + "SPECIAL_DEPLOY_NAME",
                skillDescriptionToken = DesolatorSurvivor.TOKEN_PREFIX + "SPECIAL_DEPLOY_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texDesolatorSkillSpecial"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(DriverDeployEnter)),
                activationStateMachineName = "Body",
                baseMaxStock = 1,
                baseRechargeInterval = 12f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = false,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                keywordTokens = new string[] { "KEYWORD_RADIATION_SPECIAL" }
            });

            DriverWeaponDef desolatorGunWeaponDef = DriverWeaponDef.CreateWeaponDefFromInfo(new DriverWeaponDefInfo
            {
                nameToken = DesolatorSurvivor.TOKEN_PREFIX + "DRIVER_GUN_NAME",
                descriptionToken = DesolatorSurvivor.TOKEN_PREFIX + "DRIVER_GUN_DESCRIPTION",
                icon = assetBundle.LoadAsset<Texture2D>("texIconDesolatorRA2"),
                crosshairPrefab = DesolatorSurvivor.instance.prefabCharacterBody.defaultCrosshairPrefab,
                tier = DriverWeaponTier.Uncommon,
                shotCount = 48,
                primarySkillDef = primarySkillDefBeam,
                secondarySkillDef = deploySkillDef,
                mesh = assetBundle.LoadAsset<Mesh>("meshDriverDesolatorGun"),
                material = assetBundle.LoadAsset<Material>("matDesolatorCannon"),
                animationSet = DriverWeaponDef.AnimationSet.TwoHanded,
                calloutSoundString = "Play_Desolator_Voiceline_Driver",
                configIdentifier = "Desolator Rad Cannon"
            });
            RobDriver.DriverWeaponCatalog.AddWeapon(desolatorGunWeaponDef);

            desolatorGunIndex = desolatorGunWeaponDef.index;
        }

        public class DriverRadBeam : RadBeam
        {
            public override string muzzleString => "ShotgunMuzzle";

            public override void PlayShootAnimation()
            {
                PlayAnimation("Gesture, Override", "FireTwohand");
                GetComponent<DriverController>().StartTimer();
            }
        }

        public class DriverDeployEnter : DeployEnter
        {
            protected override void PlayCannonAnimations(Animator animator)
            {
            }
        }
    }
}