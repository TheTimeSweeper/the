using RoR2;
using RoR2.Skills;
using System;
using UnityEngine;

namespace Modules {
    public class Content {
        public static void AddCharacterBodyPrefab(GameObject bprefab) {

            ContentPacks.bodyPrefabs.Add(bprefab);
        }
        public static void AddMasterPrefab(GameObject prefab) {

            ContentPacks.masterPrefabs.Add(prefab);
        }
        public static void AddProjectilePrefab(GameObject prefab) {

            ContentPacks.projectilePrefabs.Add(prefab);
        }

        public static void AddSurvivorDef(SurvivorDef survivorDef) {

            ContentPacks.survivorDefs.Add(survivorDef);
        }
        public static void AddUnlockableDef(UnlockableDef unlockableDef) {

            ContentPacks.unlockableDefs.Add(unlockableDef);
        }
        public static void AddSkillDef(SkillDef skillDef) {

            ContentPacks.skillDefs.Add(skillDef);
        }
        public static void AddSkillFamily(SkillFamily skillFamily) {

            ContentPacks.skillFamilies.Add(skillFamily);
        }
        public static void AddEntityState(Type entityState) {

            ContentPacks.entityStates.Add(entityState);
        }

        public static void AddBuffDef(BuffDef buffDef) {

            ContentPacks.buffDefs.Add(buffDef);
        }
        public static void AddEffectDef(EffectDef effectDef) {

            ContentPacks.effectDefs.Add(effectDef);
        }

        public static void AddNetworkSoundEventDef(NetworkSoundEventDef networkSoundEventDef) {

            ContentPacks.networkSoundEventDefs.Add(networkSoundEventDef);
        }
    }
}