using BepInEx.Configuration;

namespace JoeModForReal.Content.Survivors {
    public static class GenjiStates {
        public static void Init() {
            R2API.ContentAddition.AddEntityState(typeof(ModdedEntityStates.Genji.ThrowShuriken), out _);
            R2API.ContentAddition.AddEntityState(typeof(ModdedEntityStates.Genji.ThrowShurikenAlt), out _);
            R2API.ContentAddition.AddEntityState(typeof(ModdedEntityStates.Genji.Deflect), out _);
            R2API.ContentAddition.AddEntityState(typeof(ModdedEntityStates.Genji.Dash), out _);
            R2API.ContentAddition.AddEntityState(typeof(ModdedEntityStates.Genji.DragonBlade), out _);
            R2API.ContentAddition.AddEntityState(typeof(ModdedEntityStates.Genji.DragonBladeIntro), out _);
            R2API.ContentAddition.AddEntityState(typeof(ModdedEntityStates.Genji.DragonBladeOutro), out _);
            R2API.ContentAddition.AddEntityState(typeof(ModdedEntityStates.Genji.SwingBlade), out _);
        }
    }

    public static class GenjiConfig {

        public static ConfigEntry<float> shurikenDamage;
        public static ConfigEntry<float> shurikensInterval;
        public static ConfigEntry<float> shurikensFinalInterval;
        public static ConfigEntry<int> shurikens;
        public static ConfigEntry<float> deflectDuration;
        public static ConfigEntry<float> deflectCooldown;
        public static ConfigEntry<float> deflectProjectileDamageMultiplier;
        public static ConfigEntry<float> deflectBulletAttackDamageMultiplier;
        public static ConfigEntry<float> deflectGolemLaserDamageMultiplier;
        public static ConfigEntry<float> dashDamage;
        public static ConfigEntry<float> dashCooldown;
        public static ConfigEntry<float> dashSpeed;
        public static ConfigEntry<float> dashDuration;
        public static ConfigEntry<float> dashGroundFriction;
        public static ConfigEntry<float> dragonBladeIntroDuration;
        public static ConfigEntry<float> dragonBladeDuration;
        public static ConfigEntry<float> dragonBladeDamage;
        public static ConfigEntry<float> dragonBladeSwingDuration;
        public static ConfigEntry<float> dragonBladeChargeMultiplier;
        public static ConfigEntry<float> dragonBladeChargeRequired;

        public static void Init() {
            string section = "Genji";

            shurikenDamage = Modules.Config.BindAndOptions<float>(
                section,
                "ShurikenDamage",
                0.7f);

            shurikensInterval = Modules.Config.BindAndOptionsSlider(
                section,
                "shurikensInterval",
                0.15f,
                "time between m1 3 shuriken tosses",
                0f,
                1f);

            shurikensFinalInterval = Modules.Config.BindAndOptionsSlider(
                section,
                "shurikensFinalInterval",
                0.4f,
                "time after last shuriken toss until move ends, also time between alternate fire fan tosses",
                0f,
                2f);
            shurikens = Modules.Config.BindAndOptions<int>(
                section,
                "shurikens",
                3,
                "go nuts");

            deflectDuration = Modules.Config.BindAndOptions<float>(
                section,
                "deflectDuration",
                3f);

            deflectCooldown = Modules.Config.BindAndOptions<float>(
                section,
                "deflectCooldown",
                8f,
                "",
                true);

            deflectProjectileDamageMultiplier = Modules.Config.BindAndOptions<float>(
                section,
                "deflectProjectileDamageMultiplier",
                2f);

            deflectBulletAttackDamageMultiplier = Modules.Config.BindAndOptions<float>(
                section,
                "deflectBulletAttackDamageMultiplier",
                2f);

            deflectGolemLaserDamageMultiplier = Modules.Config.BindAndOptions<float>(
                section,
                "deflectGolemLaserDamageMultiplier",
                2f);

            dashDamage = Modules.Config.BindAndOptions<float>(
                section,
                "dashDamage",
                4f);
            dashCooldown = Modules.Config.BindAndOptions<float>(
                section,
                "dashCooldown",
                8f,
                "",
                true);
            dashSpeed = Modules.Config.BindAndOptionsSlider(
                section,
                "dashSpeed",
                42f,
                "",
                0,
                1000);

            dashDuration = Modules.Config.BindAndOptions<float>(
                section,
                "dashDuration",
                0.5f);

            dashGroundFriction = Modules.Config.BindAndOptionsSlider(
                section,
                "dashGroundFriction",
                42f,
                "How much speed is reduced when touching the ground. set to 1 to instantly stop, set to 0 for no friction. Also based on angle.",
                0,
                1);

            dragonBladeIntroDuration = Modules.Config.BindAndOptions<float>(
                section,
                "dragonBladeIntroDuration",
                1f);

            dragonBladeDuration = Modules.Config.BindAndOptions<float>(
                section,
                "dragonBladeDuration",
                8f);

            dragonBladeDamage = Modules.Config.BindAndOptions<float>(
                section,
                "dragonBladeDamage",
                5f);

            dragonBladeSwingDuration = Modules.Config.BindAndOptions<float>(
                section,
                "dragonBladeSwingDuration",
                2f,
                "actual attack time is half of this. second half is recover if not interrupted");

            dragonBladeChargeMultiplier = Modules.Config.BindAndOptions<float>(
                section,
                "dragonBladeChargeMultiplier",
                0.2f,
                "how much does damage % charge ultimate");

            dragonBladeChargeRequired = Modules.Config.BindAndOptionsSlider(
                section,
                "dragonBladeChargeRequired",
                20f,
                "",
                0,
                100,
                true);
        }
    }
}