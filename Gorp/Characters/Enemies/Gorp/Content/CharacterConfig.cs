using GorpMod.Modules;
using HG.GeneralSerializer;
using R2API.Utils;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using UnityEngine.UIElements;
using UnityEngine;
namespace GorpMod.Enemies.Gorp.Content
{
    public class CharacterConfig
    {
        public const string SectionBody = "Body";
        public const string SectionSkills = "Skills";
        public const string SectionMisc = "Misc";
        public const string SectionDebug = "Debug";

        public static BepInEx.Configuration.ConfigEntry<bool> Debug;

        //body
        [Configure(SectionDebug, 10, max = 30f)]
        public static ConfigEntry<int> HealthThresholds;
        [Configure(SectionDebug, 1.3f, max = 30f)]
        public static ConfigEntry<float> Size_MinHealth;
        [Configure(SectionDebug, 2.25f, max = 30f)]
        public static ConfigEntry<float> Size_MaxHealth;

        [Configure(SectionMisc,
            2.1f,
            max = 100,
            name = "Max Absorption Health/Size",
            description = "The multiplier of starting max health that gorp is allowed to absorb")]
        public static ConfigEntry<float> MaxAbsorbtionSize;
        //[Configure(SectionMisc, -1f, max = 30f, min = -20f)]
        //public static ConfigEntry<float> Damage_MinHealth;
        [Configure(SectionMisc, 5f, max = 30f, name = "Low Health Move Speed Gain", description = "How much move speed it gains as its health gets lower.")]
        public static ConfigEntry<float> AdditionalMoveSpeed_MinHealth;
        //[Configure(SectionMisc, 3f, max = 30f)]
        //public static ConfigEntry<float> ASpeed_MinHealth;
        //[Configure(SectionMisc, 0.5f, max = 30f)]
        //public static ConfigEntry<float> CDR_MinHealth;
        [Configure(SectionMisc, 3f, max = 30f)]
        public static ConfigEntry<float> EarScale;
        [Configure(SectionDebug, 7f, max = 30f)]
        public static ConfigEntry<float> MonchDistance_MinHealth;
        [Configure(SectionDebug, 16f, max = 30f)]
        public static ConfigEntry<float> MonchDistance_MaxHealth;
        [Configure(SectionDebug, 0.3f, max = 1f)]
        public static ConfigEntry<float> MonchInterval;

        [Configure(SectionMisc, 5, max = 30f, description = "How many gips are spawned when it dies")]
        public static ConfigEntry<int> DeathGips;

        //spike
        [Configure(SectionDebug, 0.3f, name = "Spike State Damage")]
        public static ConfigEntry<float> SpikeStateRepeatInterval;
        [Configure(SectionDebug, 3)]
        public static ConfigEntry<int> SpikeStateRepeats;
        [Configure(SectionSkills, 3, description = "damage coefficient. ex: 2 = 200% damage")]
        public static ConfigEntry<int> SpikeStateDamage;

        //mortar
        [Configure(SectionDebug, 0.5f)]
        public static ConfigEntry<float> Mortar_baseDuration;
        [Configure(SectionDebug, 1f)]
        public static ConfigEntry<float> Mortar_minimumTime;
        [Configure(SectionDebug, 40f, max = 500)]
        public static ConfigEntry<float> Mortar_projectileHorizontalSpeed;
        [Configure(SectionDebug, 4f)]
        public static ConfigEntry<float> Mortar_animationTimeMulti;

        //bound
        [Configure(SectionDebug, 50f, max = 500, description = "unused attack")]
        public static ConfigEntry<float> Bound_HorizontalSpeedMultiplier;
        //bound
        [Configure(SectionDebug, 0.5f, max = 2, description = "unused attack. 1 is perfect distance, 0 is none, 2 is twice the distance etc")]
        public static ConfigEntry<float> Bound_predictionTime;


        [Configure(SectionMisc, 
            -1,
            min = -1,
            name = "Minimum Stage Clear Count",
            description = "Number of stages that must be completed before this boss can spawn")]
        public static ConfigEntry<int> minimumStageCount;
        [Configure(SectionMisc,
            600,
            name = "Spawn Credits Cost",
            description = "How many director credits does this boss cost to spawn")]
        public static ConfigEntry<int> spawnCost;

        public static List<StageSpawnInfo> StageList = new List<StageSpawnInfo>();

        public class StageSpawnInfo
        {
            private string stageName;
            private int minStages;

            public StageSpawnInfo(string stageName, int minStages)
            {
                this.stageName = stageName;
                this.minStages = minStages;
            }

            public string GetStageName() { return stageName; }
            public int GetMinStages() { return minStages; }
        }

        public void Init()
        {
            Debug = GorpPlugin.instance.Config.Bind(SectionDebug, "Debug", false, "Enables development configs and logs");
            if (!Debug.Value)
            {
                Config.DisableSection(SectionDebug);
            }

            Config.InitConfigAttributes(GetType());

            ConfigEntry<string> stagesConfig = Config.BindAndOptions(
                section: SectionMisc,
                name: "Stage Spawn List",
                defaultValue: "lakes, lakesnight, dampcavesimple, rootjungle, skymeadow",
                description: "What stages the boss will show up on. Add a '- loop' after the stagename to make it only spawn after looping. List of stage names can be found at https://github.com/risk-of-thunder/R2Wiki/wiki/Mod-Creation_Developer-Reference_Scene-Names");


            //parse stage
            string stages = new string(stagesConfig.Value.ToCharArray().Where(c => !System.Char.IsWhiteSpace(c)).ToArray());
            string[] splitStages = stages.Split(',');
            foreach (string str in splitStages)
            {
                string[] current = str.Split('-');

                string name = current[0];
                int minStages = 0;
                if (current.Length > 1)
                {
                    minStages = 5;
                }

                StageList.Add(new StageSpawnInfo(name, minStages));
            }
        }
    }
}
