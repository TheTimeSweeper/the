﻿using MatcherMod.Survivors.Matcher.SkillStates;

namespace MatcherMod.Survivors.Matcher.Content
{
    public static class CharacterStates
    {
        public static void Init()
        {
            Modules.Content.AddEntityState(typeof(Sword));

            Modules.Content.AddEntityState(typeof(Secondary2Explosion));
            Modules.Content.AddEntityState(typeof(Secondary1Fireball));

            Modules.Content.AddEntityState(typeof(Roll));
            
            Modules.Content.AddEntityState(typeof(MatchMenu));
            Modules.Content.AddEntityState(typeof(MatchMenuInfinite));
            Modules.Content.AddEntityState(typeof(MatchMenuAI));
        }
    }
}
