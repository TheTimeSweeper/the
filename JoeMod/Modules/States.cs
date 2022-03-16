using ModdedEntityStates.Henry;
using ModdedEntityStates.Joe;
using ModdedEntityStates.BaseStates;
using System.Collections.Generic;
using System;

namespace Modules
{
    internal static class States
    {
        public static List<Type> entityStates => ContentPacks.entityStates;

        public static void RegisterStates()
        {
            #region henry
            entityStates.Add(typeof(SlashCombo));

            entityStates.Add(typeof(Shoot));

            entityStates.Add(typeof(Roll));

            entityStates.Add(typeof(ThrowBomb));
            #endregion

            //okay, I said I this for tokens, and now I know that makes more sense for translatoin
            // but for here it's definitely asinine to have these so randomly separate from the skills
            //oh i guess it's for organization of registering all the shit to the contentpack, however
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