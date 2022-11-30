using System;
using BepInEx.Configuration;
using UnityEngine;

namespace Modules
{
    internal static class Config
    {
        public enum ReticleType {
            Hurtbox,
            Center
        }

        public static bool Debug;

        public static bool NewColor;
        public static bool Cursed;
        public static bool TowerItemDisplays;
        public static ConfigEntry<KeyCode> voiceKey;
        public static ConfigEntry<bool> VoiceInLobby;
        public static bool RA2Icon;

        public static bool EnableTeslaTrooper;
        public static ConfigEntry<bool> TowerTargeting;
        public static ConfigEntry<ReticleType> TargetingReticleType;
        public static int LysateLimit;
        public static float UtilityDamageAbsorption;
        public static ConfigEntry<bool> UncappedUtility;

        public static bool DesolatorForceUnlock;

        public static void ReadConfig() {
            Debug = FacelessJoePlugin.instance.Config.Bind(
                "0. Debug",
                "Debug Logs",
                false,
                "in case I forget to delete them when I upload").Value;

            string sectionGeneral = "1. General";

            NewColor = FacelessJoePlugin.instance.Config.Bind(
                sectionGeneral,
                "New Color (s?)",
                false,
                "add black for the wife").Value;

            Cursed = FacelessJoePlugin.instance.Config.Bind(
                sectionGeneral,
                "Cursed",
                false,
                "Enables extra/wip content\nyes there's a fucking minecraft skin").Value;

            voiceKey = FacelessJoePlugin.instance.Config.Bind(
                sectionGeneral,
                "Voice Line Key",
                KeyCode.CapsLock,
                "key to play Tesla Trooper voice lines from Red Alert 2");

            VoiceInLobby = FacelessJoePlugin.instance.Config.Bind(
                sectionGeneral,
                "Voice Line In Lobby",
                false,
                "For the Red Alert 2 fans out there");

            RA2Icon = FacelessJoePlugin.instance.Config.Bind(
                sectionGeneral,
                "red alert 2 icon",
                false,
                "Changes character icon to the unit icon from Red Alert 2").Value;

            string sectionTeslaTrooper = "2. Tesla Trooper";

            TowerItemDisplays = FacelessJoePlugin.instance.Config.Bind(
                sectionTeslaTrooper,
                "Tower Item Displays",
                true,
                "Set false to disable tower item displays if you find them too silly").Value;

            //TargetingReticleType = FacelessJoePlugin.instance.Config.Bind(
            //    sectionTeslaTrooper,
            //    "Targeting Reticle Position", 
            //    ReticleType.Center,
            //    "Center: reticle will be on the center of the enemy\n" +
            //    "Hurtbox: reticle will be on the specific hurtbox you're targeting\n" +
            //    "Distance on m1 is still based on hurtbox, does not change gameplay.");

            TowerTargeting = FacelessJoePlugin.instance.Config.Bind(
                sectionTeslaTrooper,
                "Tower Targets Reticle",
                false,
                "if false, tower simply targets nearby. If true, tower targets the enemy you're currently targeting.\nWould appreciate feedback on how this feels");

            LysateLimit = FacelessJoePlugin.instance.Config.Bind(
                sectionTeslaTrooper,
                "Lysate Cell Additional Tower Limit",
                1,
                "With additional towers, lysate cell is way too strong for a green item. Default is 1, but for proper balance I would suggest 0\n-1 for unlimited. have fun").Value;

            UtilityDamageAbsorption = 1;
                //Mathf.Clamp(
                //FacelessJoePlugin.instance.Config.Bind(
                //    sectionGameplay,
                //    "Utility Damage Absorption Cap",
                //    1.0f,
                //    "How much damage (as a percentage) is completely blocked while charging up. If set to 0, no damage would be blocked." +
                //    "\nNote that this does not affect how much damage is reflected after the buff expires.").Value,
                //0.0f,
                //1.0f);

            UncappedUtility = FacelessJoePlugin.instance.Config.Bind(
                sectionTeslaTrooper,
                "Uncapped utility damage",
                false,
                "Removes the cap on how much damage you can retaliate with.\nIf you want utility to be his main source of damage");

            string sectionDesolator = "3. Desolator";

            FacelessJoePlugin.Desolator = Modules.Config.CharacterEnableConfig("Desolator", true, "", sectionDesolator).Value;

            //DesolatorForceUnlock = FacelessJoePlugin.instance.Config.Bind(
            //    sectionDesolator,
            //    "Force unlock",
            //    true,
            //    "Unlock character by default\nthere's no unlock yet. by downloading the mod before the achievement you've unlocked him by default").Value;


        }

        // this helper automatically makes config entries for disabling survivors
        public static ConfigEntry<bool> CharacterEnableConfig(string characterName, bool enabledDefault = true, string description = "", string section = "General")
        {
            return FacelessJoePlugin.instance.Config.Bind<bool>(section,
                                                                "Enable "+ characterName,
                                                                enabledDefault,
                                                                !string.IsNullOrEmpty(description) ? description : "Set to false to disable this character");
        }

        public static ConfigEntry<bool> EnemyEnableConfig(string characterName)
        {
            return FacelessJoePlugin.instance.Config.Bind<bool>(new ConfigDefinition(characterName, "Enabled"), true, new ConfigDescription("Set to false to disable this enemy"));
        }
    }
}