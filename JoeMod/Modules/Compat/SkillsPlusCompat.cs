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

        public static int TeslaZap_InitialCasts;

        public static void init() {
            TeslaZap_InitialCasts = Zap.OrbCasts;

            doLanguage();

            SkillModifierManager.LoadSkillModifiers();
        }

        private static void doLanguage() {

            LanguageAPI.Add("TESLA_PRIMARY_ZAP_UPGRADE_DESCRIPTION", $"Every 2 levels, <style=cIsUtility>+1</style> Close-Range Bolt\n(<style=cIsUtility>+{Zap.DamageCoefficient*100f}%</style> damage and <style=cIsUtility>+1</style> max enemies bounced)");
            LanguageAPI.Add("TESLA_SECONDARY_BIGZAP_UPGRADE_DESCRIPTION", $"<style=cIsUtility>+10%</style> Area, <style=cIsDamage>+5%</style> Damage");
        }

        [SkillLevelModifier("Tesla_Primary_Zap", typeof(Zap))]
        public class TeslaZapModifier : BaseSkillModifier {

            public override void OnSkillEnter(BaseState skillState, int level) {
                base.OnSkillEnter(skillState, level);
                (skillState as Zap).skillsPlusCasts = Mathf.FloorToInt(AdditiveScaling(0.0f, 0.5f, level));
            }
        }

        [SkillLevelModifier("Tesla_Secondary_BigZap", typeof (AimBigZap), typeof(BigZap), typeof(TowerBigZap))]
        class EngiMineSkillModifier : BaseSkillModifier {

            public override void OnSkillEnter(BaseState skillState, int level) {
                
                if (skillState is AimBigZap aimBigZapState) {

                    aimBigZapState.skillsPlusMulti = MultScaling(1, .1f, level);
                    
                } else if (skillState is BigZap bigZapState) {

                    bigZapState.skillsPlusAreaMulti = MultScaling(1, .1f, level);
                    bigZapState.skillsPlusDamageMulti = MultScaling(1f, 0.2f, level);

                } else if (skillState is TowerBigZap towerBigZapState) {

                    towerBigZapState.secondarySkillsPlusAreaMulti = MultScaling(1, .15f, level);
                    towerBigZapState.secondarySkillsPlusDamageMulti = MultScaling(1f, 0.05f, level);
                }
            }
        }
    }
}