using MatcherMod.Survivors.Matcher.SkillStates;

namespace MatcherMod.Survivors.Matcher.MatcherContent
{
    public static class States
    {
        public static void Init()
        {
            Modules.Content.AddEntityState(typeof(SlashCombo));

            Modules.Content.AddEntityState(typeof(MatchMenu));

            Modules.Content.AddEntityState(typeof(Roll));

            Modules.Content.AddEntityState(typeof(ThrowBomb));
        }
    }
}
