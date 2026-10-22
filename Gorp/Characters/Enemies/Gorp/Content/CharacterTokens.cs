using System;
using GorpMod.Characters.Enemies.Gorp;
using GorpMod.Modules;

namespace GorpMod.Enemies.Gorp.Content
{
    public static class CharacterTokens
    {
        public static void Init()
        {
            AddHenryTokens();

            ////uncomment this to spit out a lanuage file with all the above tokens that people can translate
            ////make sure you set Language.usingLanguageFolder and printingEnabled to true
            //Language.PrintOutput("Henry.txt");
            ////refer to guide on how to build and distribute your mod with the proper folders
        }

        public static void AddHenryTokens()
        {
            string prefix = GorpCharacter.TOKEN_PREFIX;

            string desc = ".<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
             + "< ! > ." + Environment.NewLine + Environment.NewLine
             + "< ! > ." + Environment.NewLine + Environment.NewLine
             + "< ! > ." + Environment.NewLine + Environment.NewLine
             + "< ! > ." + Environment.NewLine + Environment.NewLine;

            Language.Add(prefix + "NAME", "Gorp");
            Language.Add(prefix + "DESCRIPTION", desc);
            Language.Add(prefix + "SUBTITLE", "End of the Slime");
            Language.Add(prefix + "LORE", "Big.");
            Language.Add(prefix + "OUTRO_FLAVOR", "..and so it left, .");
            Language.Add(prefix + "OUTRO_FAILURE", "..and so it vanished, .");

        }
    }
}
