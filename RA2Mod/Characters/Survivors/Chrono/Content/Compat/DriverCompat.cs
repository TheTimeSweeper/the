using BepInEx.Configuration;
using RA2Mod.Modules;
using RA2Mod.Survivors.Chrono.Components;
using RA2Mod.Survivors.Chrono.SkillDefs;
using RA2Mod.Survivors.Chrono.SkillStates;
using RobDriver.Modules.Components;
using RoR2;
using RoR2.Skills;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Bindings;

namespace RA2Mod.Survivors.Chrono
{
    public class DriverCompat
    {
        public static GameObject chronoIndicatorVanishDriver;
        public static ConfigEntry<float> DriverGunM1Damage;
        public static ConfigEntry<float> DriverGunM1Duration;
        public static ConfigEntry<float> DriverGunM2Damage;
        public static ConfigEntry<float> DriverGunM2TickInterval;
        public static ConfigEntry<float> DriverGunM2Duration;

        private static AssetBundle assetBundle => ChronoSurvivor.instance.assetBundle;

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void Init()
        {
            On.RoR2.SurvivorCatalog.SetSurvivorDefs += SurvivorCatalog_SetSurvivorDefs;

            InitConfig();
        }

        private static void InitConfig()
        {
            string section = "3-1. Driver Compat";

            DriverGunM1Damage = Config.BindAndOptionsSlider(
                section,
                "DriverGunM1Damage",
                1f,
                "",
                0,
                20);

            DriverGunM1Duration = Config.BindAndOptionsSlider(
                section,
                "DriverGunM1Duration",
                0.6f,
                "",
                0,
                5);

            DriverGunM2Damage = Config.BindAndOptionsSlider(
                section,
                "DriverGunM2Damage",
                0.5f,
                "",
                0,
                10);

            DriverGunM2TickInterval = Config.BindAndOptionsSlider(
                section,
                "DriverGunM2TickInterval",
                0.2f,
                "",
                0,
                10);

            DriverGunM2Duration = Config.BindAndOptionsSlider(
                section,
                "DriverGunM2Duration",
                3f,
                "",
                0,
                20);
        }

        private static void SurvivorCatalog_SetSurvivorDefs(On.RoR2.SurvivorCatalog.orig_SetSurvivorDefs orig, SurvivorDef[] newSurvivorDefs)
        {
            orig(newSurvivorDefs);

            for (int i = 0; i < newSurvivorDefs.Length; i++)
            {
                if (newSurvivorDefs[i].bodyPrefab.name == "RobDriverBody")
                {
                    newSurvivorDefs[i].bodyPrefab.AddComponent<ChronoTrackerVanishDriver>();
                    Log.Debug("found driver. adding tracker");
                    DoDriverCompat();
                    return;
                }
            }

            Log.CurrentTime("no driver. chrono compat failed");
        }

        private static void DoDriverCompat()
        {
            chronoIndicatorVanishDriver = assetBundle.LoadAsset<GameObject>("IndicatorChronoVanishDriver");

            SkillDef shootSkillDef = Skills.CreateSkillDef(new SkillDefInfo
                (
                    "chronoShoot",
                    ChronoSurvivor.CHRONO_PREFIX + "PRIMARY_SHOOT_NAME",
                    ChronoSurvivor.CHRONO_PREFIX + "PRIMARY_SHOOT_DESCRIPTION",
                    ChronoSurvivor.instance.assetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
                    new EntityStates.SerializableEntityStateType(typeof(ShootDriver)),
                    "Weapon",
                    false
                ));

            ChronoTrackerSkillDefVanish vanishSkillDef = Skills.CreateSkillDef<ChronoTrackerSkillDefVanish>(new SkillDefInfo
            {
                skillName = "chronoVanish",
                skillNameToken = ChronoSurvivor.CHRONO_PREFIX + "SPECIAL_VANISH_NAME",
                skillDescriptionToken = ChronoSurvivor.CHRONO_PREFIX + "SPECIAL_VANISH_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSpecialIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(VanishDriver)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = 8f,

                isCombatSkill = true,
                mustKeyPress = true,
            });
            
            DriverWeaponDef chronoGunWeaponDef = DriverWeaponDef.CreateWeaponDefFromInfo(new DriverWeaponDefInfo
            {
                nameToken = ChronoSurvivor.CHRONO_PREFIX + "DRIVER_GUN_NAME",
                descriptionToken = ChronoSurvivor.CHRONO_PREFIX + "DRIVER_GUN_DESCRIPTION",
                icon = assetBundle.LoadAsset<Texture2D>("texIconChrono"),
                crosshairPrefab = ChronoSurvivor.instance.prefabCharacterBody.defaultCrosshairPrefab,
                tier = DriverWeaponTier.Uncommon,
                shotCount = 8,
                primarySkillDef = shootSkillDef,
                secondarySkillDef = vanishSkillDef,
                mesh = assetBundle.LoadAsset<Mesh>("meshDriverChronoGun"),
                material = assetBundle.LoadAsset<Material>("matDriverChronoGun"),
                animationSet = DriverWeaponDef.AnimationSet.TwoHanded,
                calloutSoundString = "Play_Chrono_Voiceline_Driver",
                configIdentifier = "Chrono Legionnaire Gun Neutron Rifle"
            });
            RobDriver.DriverWeaponCatalog.AddWeapon(chronoGunWeaponDef);
        }
    }

    public class ChronoTrackerVanishDriver : ChronoTrackerVanish
    {
        public override float maxTrackingDistance => 80;

        public override float maxTrackingAngle => 30;

        public override BullseyeSearch.SortMode bullseyeSortMode => BullseyeSearch.SortMode.Angle;

        public override bool filterByLoS => false;

        protected override void SetIndicator()
        {
            this.indicator = new Indicator(base.gameObject, DriverCompat.chronoIndicatorVanishDriver);
        }
    }

    //public class ChronoTrackerSkillDefVanishDriver : ChronoTrackerSkillDefVanish { }


    public class ShootDriver : Shoot
    {
        public override float damageCoefficient => DriverCompat.DriverGunM1Damage.Value;
        public override void OnEnter()
        {
            base.OnEnter();

            if (gameObject.TryGetComponent(out DriverController iDrive)) 
            { 
                iDrive.StartTimer();
            }

            muzzleString = "ShotgunMuzzle";
        }

        protected override void PlayShootAnimation()
        {
            base.PlayShootAnimation();

            PlayAnimation("Gesture, Override", "FireMachineGun", "shoot.playbackRate", duration);
        }
    }

    public class VanishDriver : Vanish
    {
        public override float damageCoefficient => DriverCompat.DriverGunM2Damage.Value;
        public override float baseTickInterval =>  DriverCompat.DriverGunM2TickInterval.Value;
        public override float baseDuration =>      DriverCompat.DriverGunM2Duration.Value;

        private DriverWeaponDef cachedWeaponDef;
        private DriverController iDrive;

        public override void OnEnter()
        {
            if(gameObject.TryGetComponent(out iDrive))
            {
                cachedWeaponDef = iDrive?.weaponDef;
            }

            base.OnEnter();

            muzzleTransform = FindModelChild("ShotgunMuzzle");
            if(muzzleTransform == null)
            {
                muzzleTransform = transform;
            }

            SetTetherPoints();

            if (targetingAlly)
            {
                if (iDrive)
                {
                    iDrive.StartTimer();
                }
            }
        }

        public override void DoDamage()
        {
            if (iDrive)
            {
                iDrive.StartTimer(baseTickInterval);
            }

            base.DoDamage();
        }

        public override void FixedUpdate()
        {
            if (iDrive && cachedWeaponDef != iDrive.weaponDef)
            {
                base.PlayAnimation("Gesture, Override", "BufferEmpty");
                this.outer.SetNextStateToMain();
                return;
            }

            base.FixedUpdate();
        }
    }
}
