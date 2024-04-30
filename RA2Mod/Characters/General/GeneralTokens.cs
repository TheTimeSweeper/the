using RA2Mod.Modules;
using RA2Mod.Survivors.Tesla;

namespace RA2Mod.General
{
    public static class GeneralTokens
    {
        private const string prefix = RA2Plugin.DEVELOPER_PREFIX;

        public static void Init()
        {
            #region recolor
            Language.Add("LOADOUT_SKILL_COLOR", "Color");

            Language.Add(prefix + "_RECOLOR_RED_NAME", "Red");
            Language.Add(prefix + "_RECOLOR_BLUE_NAME", "Blue");
            Language.Add(prefix + "_RECOLOR_GREEN_NAME", "Green");
            Language.Add(prefix + "_RECOLOR_YELLOW_NAME", "Yellow");
            Language.Add(prefix + "_RECOLOR_ORANGE_NAME", "Orange");
            Language.Add(prefix + "_RECOLOR_CYAN_NAME", "Cyan");
            Language.Add(prefix + "_RECOLOR_PURPLE_NAME", "Purple");
            Language.Add(prefix + "_RECOLOR_PINK_NAME", "Pink");
            Language.Add(prefix + "_RECOLOR_BLACK_NAME", "Black");
            #endregion
        }
    }
}
