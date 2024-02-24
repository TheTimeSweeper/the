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

        private ushort chronoGunIndex;

        private static AssetBundle assetBundle => ChronoSurvivor.instance.assetBundle;

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public void Init()
        {
            On.RoR2.SurvivorCatalog.SetSurvivorDefs += SurvivorCatalog_SetSurvivorDefs;

            InitConfig();

            #region compat
            Modules.Language.Add(ChronoSurvivor.CHRONO_PREFIX + "DRIVER_GUN_NAME", "Chrono Gun");
            Modules.Language.Add(ChronoSurvivor.CHRONO_PREFIX + "DRIVER_GUN_DESCRIPTION", $"Makes enemies Vanish from existence.");

            Modules.Language.Add(ChronoSurvivor.CHRONO_PREFIX + "PRIMARY_SHOOT_DRIVER_NAME", "Chrono Gun");
            Modules.Language.Add(ChronoSurvivor.CHRONO_PREFIX + "PRIMARY_SHOOT_DRIVER_DESCRIPTION", $"Fire for {Tokens.DamageValueText(DriverCompat.DriverGunM1Damage.Value)} and apply {Tokens.UtilityText("Chrono Sickness")} to enemies.");
            
            int driverTicks = (int)(DriverCompat.DriverGunM2Duration.Value / DriverCompat.DriverGunM2TickInterval.Value);
            Modules.Language.Add(ChronoSurvivor.CHRONO_PREFIX + "SPECIAL_VANISH_DRIVER_NAME", "Deconstructing");
            Modules.Language.Add(ChronoSurvivor.CHRONO_PREFIX + "SPECIAL_VANISH_DRIVER_DESCRIPTION", $"Focus your rifle for up to {Tokens.DamageValueText(DriverCompat.DriverGunM2Damage.Value * driverTicks)}. An enemy below the {Tokens.UtilityText("Chrono Sickness")} threshold will vanish from existence.");
            #endregion compat

            if (General.GeneralConfig.Debug.Value)
            {
                On.RoR2.CharacterBody.Update += CharacterBody_Update;
            }
        }

        private void CharacterBody_Update(On.RoR2.CharacterBody.orig_Update orig, CharacterBody self)
        {
            orig(self);
            if (Input.GetKeyDown(KeyCode.G))
            {
                if(self.TryGetComponent(out DriverController cantdrive55))
                {
                    cantdrive55.PickUpWeapon(RobDriver.DriverWeaponCatalog.GetWeaponFromIndex(chronoGunIndex));
                }
            }
        }

        private void InitConfig()
        {
            string section = "2-1. Driver Compat";

            DriverGunM1Damage = Config.BindAndOptionsSlider(
                section,
                "DriverGunM1Damage",
                2.0f,
                0,
                20,
                "");

            DriverGunM1Duration = Config.BindAndOptionsSlider(
                section,
                "DriverGunM1Duration",
                0.4f,
                0,
                5,
                "");

            DriverGunM2Damage = Config.BindAndOptionsSlider(
                section,
                "DriverGunM2Damage",
                0.3f,
                0,
                10,
                "");

            DriverGunM2TickInterval = Config.BindAndOptionsSlider(
                section,
                "DriverGunM2TickInterval",
                0.11f,
                0,
                10,
                "");

            DriverGunM2Duration = Config.BindAndOptionsSlider(
                section,
                "DriverGunM2Duration",
                3f,
                0,
                20,
                "");
        }

        private void SurvivorCatalog_SetSurvivorDefs(On.RoR2.SurvivorCatalog.orig_SetSurvivorDefs orig, SurvivorDef[] newSurvivorDefs)
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

        private void DoDriverCompat()
        {
            chronoIndicatorVanishDriver = assetBundle.LoadAsset<GameObject>("IndicatorChronoVanishDriver");

            SkillDef shootSkillDef = Skills.CreateSkillDef(new SkillDefInfo
                (
                    "chronoShoot",
                    ChronoSurvivor.CHRONO_PREFIX + "PRIMARY_SHOOT_DRIVER_NAME",
                    ChronoSurvivor.CHRONO_PREFIX + "PRIMARY_SHOOT_DRIVER_DESCRIPTION",
                    ChronoSurvivor.instance.assetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
                    new EntityStates.SerializableEntityStateType(typeof(ShootDriver)),
                    "Weapon",
                    false
                ));

            ChronoTrackerSkillDefVanish vanishSkillDef = Skills.CreateSkillDef<ChronoTrackerSkillDefVanish>(new SkillDefInfo
            {
                skillName = "chronoVanish",
                skillNameToken = ChronoSurvivor.CHRONO_PREFIX + "SPECIAL_VANISH_DRIVER_NAME",
                skillDescriptionToken = ChronoSurvivor.CHRONO_PREFIX + "SPECIAL_VANISH_DRIVER_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSpecialIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(VanishDriver)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = 5f,

                isCombatSkill = true,
                mustKeyPress = true,
            });
            
            var chronoGunWeaponDef = DriverWeaponDef.CreateWeaponDefFromInfo(new DriverWeaponDefInfo
            {
                nameToken = ChronoSurvivor.CHRONO_PREFIX + "DRIVER_GUN_NAME",
                descriptionToken = ChronoSurvivor.CHRONO_PREFIX + "DRIVER_GUN_DESCRIPTION",
                icon = assetBundle.LoadAsset<Texture2D>("texIconChrono"),
                crosshairPrefab = ChronoSurvivor.instance.prefabCharacterBody.defaultCrosshairPrefab,
                tier = DriverWeaponTier.Uncommon,
                shotCount = 48,
                primarySkillDef = shootSkillDef,
                secondarySkillDef = vanishSkillDef,
                mesh = assetBundle.LoadAsset<Mesh>("meshDriverChronoGun"),
                material = assetBundle.LoadAsset<Material>("matDriverChronoGun"),
                animationSet = DriverWeaponDef.AnimationSet.TwoHanded,
                calloutSoundString = "Play_Chrono_Voiceline_Driver",
                configIdentifier = "Chrono Legionnaire Neutron Rifle"
            });
            RobDriver.DriverWeaponCatalog.AddWeapon(chronoGunWeaponDef);

            chronoGunIndex = chronoGunWeaponDef.index;
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


    public class ShootDriver : ChronoShoot
    {
        public override float baseDuration => DriverCompat.DriverGunM1Duration.Value;
        public override float damageCoefficient => DriverCompat.DriverGunM1Damage.Value;
        public override float hitRadius => 1;
        public override float recoil => 0.4f;
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

            PlayAnimation("Gesture, Override", "FireMachineGun", "Shoot.playbackRate", duration);
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

            PlayAnimation("Gesture, Override", "FireMachineGun", "Shoot.playbackRate", 1000);

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
                iDrive.StartTimer(baseTickInterval/baseDuration * 10);
            }

            base.DoDamage();
        }

        public override void OnExit()
        {
            base.OnExit();
            base.PlayAnimation("Gesture, Override", "BufferEmpty");
        }

        public override void FixedUpdate()
        {
            if (iDrive && cachedWeaponDef != iDrive.weaponDef)
            {
                this.outer.SetNextStateToMain();
                return;
            }

            base.FixedUpdate();
        }
    }
}
