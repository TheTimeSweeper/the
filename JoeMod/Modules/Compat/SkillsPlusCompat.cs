using System;
using RoR2;
using RoR2.Skills;
using ModdedEntityStates.TeslaTrooper;
using ModdedEntityStates.TeslaTrooper.Tower;
using R2API;
using SkillsPlusPlus;
using SkillsPlusPlus.Modifiers;
using EntityStates;
using UnityEngine;

namespace Modules {

    public class SkillsPlusCompat {

        public static int SkillsPlusAdditionalTowers;

        public static void init() {

            doLanguage();

            SkillModifierManager.LoadSkillModifiers();
        }

        private static void doLanguage() {

            LanguageAPI.Add("TESLA_PRIMARY_ZAP_UPGRADE_DESCRIPTION", $"Every 2 levels, <style=cIsUtility>+1</style> Close-Range Bolt");
            LanguageAPI.Add("TESLA_SECONDARY_BIGZAP_UPGRADE_DESCRIPTION", $"<style=cIsUtility>+10%</style> Area, <style=cIsDamage>+10%</style> Damage");
            LanguageAPI.Add("TESLA_UTILITY_SHIELDZAP_UPGRADE_DESCRIPTION", $"<style=cIsUtility>+0.5</style> second buff time");
            LanguageAPI.Add("TESLA_SPECIAL_TOWER_UPGRADE_DESCRIPTION", $"<style=cIsUtility>+1</style> second lifetime, <style=cIsUtility>additional stock</style> every 3 levels");
            LanguageAPI.Add("TESLA_SPECIAL_SCEPTER_TOWER_UPGRADE_DESCRIPTION", $"<style=cIsUtility>+1</style> second lifetime, <style=cIsUtility>additional stock</style> every 3 levels");
        }

        [SkillLevelModifier("Tesla_Primary_Zap", typeof(Zap))]
        public class TeslaPrimaryModifier : BaseSkillModifier {

            public override void OnSkillEnter(BaseState skillState, int level) {
                base.OnSkillEnter(skillState, level);
                if (skillState is Zap zapState) {
                    zapState.skillsPlusCasts = Mathf.FloorToInt(AdditiveScaling(0.0f, 0.5f, level));
                }
            }
        }

        [SkillLevelModifier("Tesla_Secondary_BigZap", typeof(AimBigZap), typeof(BigZap), typeof(TowerBigZap))]
        class TeslaSecondaryModifier : BaseSkillModifier {

            public override void OnSkillEnter(BaseState skillState, int level) {
                base.OnSkillEnter(skillState, level);

                //Helpers.LogWarning("running on " + skillState.GetType().ToString());

                if (skillState is AimBigZap aimBigZapState) {

                    aimBigZapState.skillsPlusMulti = MultScaling(1, .1f, level);

                } else if (skillState is BigZap bigZapState) {

                    bigZapState.skillsPlusAreaMulti = MultScaling(1, .1f, level);
                    bigZapState.skillsPlusDamageMulti = MultScaling(1f, 0.1f, level);

                } else if (skillState is TowerBigZap towerBigZapState) {

                    towerBigZapState.secondarySkillsPlusAreaMulti = MultScaling(1, .1f, level);
                    towerBigZapState.secondarySkillsPlusDamageMulti = MultScaling(1f, 0.05f, level);
                }
            }
        }

        [SkillLevelModifier("Tesla_Utility_ShieldZap", typeof(ShieldZapCollectDamage))]
        public class TeslaUtilityModifier : BaseSkillModifier {

            public override void OnSkillEnter(BaseState skillState, int level) {
                base.OnSkillEnter(skillState, level);
                if (skillState is ShieldZapCollectDamage shieldState) {
                    shieldState.skillsPlusSeconds = AdditiveScaling(0f, 0.5f, level);
                }
            }
        }

        [SkillLevelModifier("Tesla_Special_Tower", new Type[0])]//, typeof(DeployTeslaTower), typeof(TowerLifetime))]
        public class TeslaSpecialModifier : BaseSkillModifier {

            public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
                base.OnSkillLeveledUp(level, characterBody, skillDef);

                skillDef.baseMaxStock = (int)AdditiveScaling(1, 0.334f, level);
                SkillsPlusAdditionalTowers = (int)AdditiveScaling(0, 0.334f, level);
                TowerLifetime.skillsPlusSeconds = AdditiveScaling(0, 1, level);
            }
        }

        //are scepter skills even getting upgraded?
        [SkillLevelModifier("Tesla_Special_Scepter_Tower", new Type[0])]
        public class TeslaSpecialScepterModifier : BaseSkillModifier {

            public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
                base.OnSkillLeveledUp(level, characterBody, skillDef);

                skillDef.baseMaxStock = (int)AdditiveScaling(2, 0.334f, level);
                SkillsPlusAdditionalTowers = (int)AdditiveScaling(0, 0.334f, level);
                TowerLifetime.skillsPlusSeconds = AdditiveScaling(0, 1, level);
            }
        }
    }
}