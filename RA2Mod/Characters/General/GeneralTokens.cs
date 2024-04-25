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
            Language.Add(prefix + "RECOLOR_RED_NAME", "Red");
            Language.Add(prefix + "RECOLOR_BLUE_NAME", "Blue");
            Language.Add(prefix + "RECOLOR_GREEN_NAME", "Green");
            Language.Add(prefix + "RECOLOR_YELLOW_NAME", "Yellow");
            Language.Add(prefix + "RECOLOR_ORANGE_NAME", "Orange");
            Language.Add(prefix + "RECOLOR_CYAN_NAME", "Cyan");
            Language.Add(prefix + "RECOLOR_PURPLE_NAME", "Purple");
            Language.Add(prefix + "RECOLOR_PINK_NAME", "Pink");
            Language.Add(prefix + "RECOLOR_BLACK_NAME", "Black");
            #endregion
        }
    }
}
