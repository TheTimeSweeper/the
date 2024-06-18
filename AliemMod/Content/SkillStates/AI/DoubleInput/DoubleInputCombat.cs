using AliemMod.Components;
using EntityStates;
using EntityStates.AI.Walker;
using RoR2;
using RoR2.CharacterAI;

namespace ModdedEntityStates.Aliem.AI
{
    public class DoubleInputCombat : Combat
    {
        public override BaseAI.BodyInputs GenerateBodyInputs(in BaseAI.BodyInputs previousBodyInputs)
        {
            BaseAI.BodyInputs baseInputs = base.GenerateBodyInputs(previousBodyInputs);

            DoubleAISkillDriver skillDriver;
            if((skillDriver = dominantSkillDriver as DoubleAISkillDriver) != null)
            {
                bool pressDefaultSkill = false;
                switch (currentSkillSlot)
                {
                    case SkillSlot.Primary:
                        pressDefaultSkill = baseInputs.pressSkill1;
                        break;
                    case SkillSlot.Secondary:
                        pressDefaultSkill = baseInputs.pressSkill2;
                        break;
                    case SkillSlot.Utility:
                        pressDefaultSkill = baseInputs.pressSkill3;
                        break;
                    case SkillSlot.Special:
                        pressDefaultSkill = baseInputs.pressSkill4;
                        break;
                }
                switch (skillDriver.skillSlot2)
                {
                    case SkillSlot.Primary:
                        baseInputs.pressSkill1 = pressDefaultSkill;
                        break;
                    case SkillSlot.Secondary:
                        baseInputs.pressSkill2 = pressDefaultSkill;
                        break;
                    case SkillSlot.Utility:
                        baseInputs.pressSkill3 = pressDefaultSkill;
                        break;
                    case SkillSlot.Special:
                        baseInputs.pressSkill4 = pressDefaultSkill;
                        break;
                }
            }
            return baseInputs;
        }

        public override void ModifyNextState(EntityState nextState)
        {
            base.ModifyNextState(nextState);
            if (nextState is Combat)
            {
                outer.SetNextState(new DoubleInputCombat());
                return;
            }
            if (nextState is Wander)
            {
                outer.SetNextState(new DoubleInputWander ());
                return;
            }
            if (nextState is LookBusy)
            {
                outer.SetNextState(new DoubleInputLookBusy ());
                return;
            }
        }
    }
}
