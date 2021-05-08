using HenryMod.SkillStates.Henry;
using HenryMod.SkillStates.Joe;
using HenryMod.SkillStates.BaseStates;
using System.Collections.Generic;
using System;

namespace HenryMod.Modules
{
    public static class States
    {
        internal static List<Type> entityStates = new List<Type>();

        internal static void RegisterStates()
        {
            //testing if we need to register the base state
            //entityStates.Add(typeof(BaseMeleeAttackButEpic));

            #region henry
            entityStates.Add(typeof(SlashCombo));

            entityStates.Add(typeof(Shoot));

            entityStates.Add(typeof(Roll));

            entityStates.Add(typeof(ThrowBomb));
            #endregion

            #region joe
            entityStates.Add(typeof(Primary1Swing ));
            #endregion
        }
    }
}