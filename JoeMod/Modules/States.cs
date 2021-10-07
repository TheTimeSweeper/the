using HenryMod.EntityStates.Henry;
using HenryMod.EntityStates.Joe;
using HenryMod.EntityStates.BaseStates;
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

            //okay, I said I this for tokens, but now I know that makes more sense for translatoin
            // but for here it's definitely asinine to have these so randomly separate from the skills
            #region joe
            ////prim
            //entityStates.Add(typeof(Primary1Swing));
            //entityStates.Add(typeof(Primary1JumpSwingFall));
            //entityStates.Add(typeof(Primary1JumpSwingLand));

            //entityStates.Add(typeof(PrimaryStupidSwing));

            //entityStates.Add(typeof(ThrowBoom));

            ////sec
            //entityStates.Add(typeof(Secondary1Fireball));
            #endregion
        }
    }
}