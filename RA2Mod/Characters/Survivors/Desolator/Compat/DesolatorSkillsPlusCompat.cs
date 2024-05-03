using EntityStates;
using RA2Mod.Survivors.Desolator.States;
using SkillsPlusPlus;
using SkillsPlusPlus.Modifiers;
using System.Runtime.CompilerServices;

namespace RA2Mod.Survivors.Desolator.Compat
{
    internal class DesolatorSkillsPlusCompat
    {
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void init()
        {
            SkillModifierManager.LoadSkillModifiers();
        }

        [SkillLevelModifier("Desolator_Primary_Beam", typeof(RadBeam))]
        internal class DesolatorPrimaryModifier : BaseSkillModifier
        {
            public override void OnSkillEnter(BaseState skillState, int level)
            {
                base.OnSkillEnter(skillState, level);
                if (skillState is RadBeam radBeam)
                {
                    radBeam.skillsPlusDurationMultiplier = MultScaling(1f, -0.1f, level);
                }
            }
        }
    }
}