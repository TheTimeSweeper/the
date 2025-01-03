﻿using PlagueMod.Survivors.Plague.SkillStates;
using PlagueMod.Survivors.Plague.SkillStates.Powder;

namespace PlagueMod.Survivors.Plague
{
    public static class PlagueStates
    {
        public static void Init()
        {
            Modules.Content.AddEntityState(typeof(SlashCombo));
            Modules.Content.AddEntityState(typeof(Shoot));
            Modules.Content.AddEntityState(typeof(Roll));
            Modules.Content.AddEntityState(typeof(ThrowBomb));
            
            Modules.Content.AddEntityState(typeof(SelectBomb));
            Modules.Content.AddEntityState(typeof(ThrowSelectedBomb));

            Modules.Content.AddEntityState(typeof(TestPowderSkillState));
            Modules.Content.AddEntityState(typeof(Test2PowderSkillState));
        }
    }
}
