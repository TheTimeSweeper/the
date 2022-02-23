using System;
using RoR2;
using RoR2.Skills;
using JoeMod.ModdedEntityStates.TeslaTrooper;
using JoeMod.ModdedEntityStates.TeslaTrooper.Tower;
using R2API;
using SkillsPlusPlus;
using SkillsPlusPlus.Modifiers;

namespace HenryMod.Modules {

    internal class SkillsPlusCompat {

        public static int TeslaZap_InitialCasts;

        internal static void init() {
            TeslaZap_InitialCasts = Zap.OrbCasts;

            doLanguage();

            SkillModifierManager.LoadSkillModifiers();
        }

        private static void doLanguage() {

            LanguageAPI.Add("TESLA_PRIMARY_ZAP_UPGRADE_DESCRIPTION", $"<style=cIsUtility>+1</style> Orb Cast ( <style=cIsUtility>+{Zap.DamageCoefficient*100f}%</style> damage and <style=cIsUtility>+1</style> max enemies bounced)");
        }

        [SkillLevelModifier("Tesla_Primary_Zap", typeof(Zap))]
        public class TeslaZapModifier : SimpleSkillModifier<Zap> {


            public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
                base.OnSkillLeveledUp(level, characterBody, skillDef);

                Zap.OrbCasts = AdditiveScaling(SkillsPlusCompat.TeslaZap_InitialCasts, 1, level);
            }

        }
    }
}