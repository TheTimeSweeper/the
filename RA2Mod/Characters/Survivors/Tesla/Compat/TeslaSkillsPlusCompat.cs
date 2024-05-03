using EntityStates;
using RA2Mod.Minions.TeslaTower.States;
using RA2Mod.Survivors.Tesla.States;
using RoR2;
using RoR2.Skills;
using SkillsPlusPlus;
using SkillsPlusPlus.Modifiers;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RA2Mod.Survivors.Tesla.Compat
{
    internal class TeslaSkillsPlusCompat
    {
        //todo teslamove network this shit probably // or just.. not
        public static int SkillsPlusAdditionalTowers;

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void init()
        {
            SkillModifierManager.LoadSkillModifiers();
        }

        [SkillLevelModifier("Tesla_Primary_Zap", typeof(Zap))]
        internal class TeslaPrimaryModifier : BaseSkillModifier
        {
            public override void OnSkillEnter(BaseState skillState, int level)
            {
                base.OnSkillEnter(skillState, level);
                if (skillState is Zap zapState)
                {
                    zapState.skillsPlusCasts = Mathf.FloorToInt(AdditiveScaling(0.0f, 0.5f, level));
                }
            }
        }

        [SkillLevelModifier("Tesla_Secondary_BigZap", typeof(AimBigZap), typeof(BigZap), typeof(TowerBigZap))]
        internal class TeslaSecondaryModifier : BaseSkillModifier
        {
            public override void OnSkillEnter(BaseState skillState, int level)
            {
                base.OnSkillEnter(skillState, level);

                //Helpers.LogWarning("running on " + skillState.GetType().ToString());

                if (skillState is AimBigZap aimBigZapState)
                {

                    aimBigZapState.skillsPlusMulti = MultScaling(1, .1f, level);

                }
                else if (skillState is BigZap bigZapState)
                {

                    bigZapState.skillsPlusAreaMulti = MultScaling(1, .1f, level);
                    bigZapState.skillsPlusDamageMulti = MultScaling(1f, 0.1f, level);

                }
                else if (skillState is TowerBigZap towerBigZapState)
                {

                    towerBigZapState.secondarySkillsPlusAreaMulti = MultScaling(1, .1f, level);
                    towerBigZapState.secondarySkillsPlusDamageMulti = MultScaling(1f, 0.05f, level);
                }
            }
        }

        [SkillLevelModifier("Tesla_Utility_ShieldZap", typeof(ShieldZapCollectDamage))]
        internal class TeslaUtilityModifier : BaseSkillModifier
        {
            public override void OnSkillEnter(BaseState skillState, int level)
            {
                base.OnSkillEnter(skillState, level);
                if (skillState is ShieldZapCollectDamage shieldState)
                {
                    shieldState.skillsPlusSeconds = AdditiveScaling(0f, 0.5f, level);
                }
            }
        }

        [SkillLevelModifier("Tesla_Special_Tower", new Type[0])]//, typeof(DeployTeslaTower), typeof(TowerLifetime))]
        internal class TeslaSpecialModifier : BaseSkillModifier
        {
            public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
            {
                base.OnSkillLeveledUp(level, characterBody, skillDef);

                skillDef.baseMaxStock = (int)AdditiveScaling(1, 0.334f, level);
                SkillsPlusAdditionalTowers = (int)AdditiveScaling(0, 0.334f, level);
                TowerLifetime.skillsPlusSeconds = AdditiveScaling(0, 1, level);
            }
        }

        //are scepter skills even getting upgraded?
        [SkillLevelModifier("Tesla_Special_Scepter_Tower", new Type[0])]
        internal class TeslaSpecialScepterModifier : BaseSkillModifier
        {
            public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
            {
                base.OnSkillLeveledUp(level, characterBody, skillDef);

                skillDef.baseMaxStock = (int)AdditiveScaling(2, 0.334f, level);
                SkillsPlusAdditionalTowers = (int)AdditiveScaling(0, 0.334f, level);
                TowerLifetime.skillsPlusSeconds = AdditiveScaling(0, 1, level);
            }
        }
    }
}
