using EntityStates;
using EntityStates.AI.Walker;
using RoR2;
using RoR2.CharacterAI;

namespace ModdedEntityStates.Aliem.AI
{
    public class OverrideAIWander : Wander
    {
        public InputBankTest overridor;

        public override BaseAI.BodyInputs GenerateBodyInputs(in BaseAI.BodyInputs previousBodyInputs)
        {
            BaseAI.BodyInputs inputs = base.GenerateBodyInputs(previousBodyInputs);
            inputs.moveVector = overridor.moveVector;
            return inputs;
        }

        public override void ModifyNextState(EntityState nextState)
        {
            base.ModifyNextState(nextState);
            if (nextState is Combat)
            {
                outer.SetNextState(new OverrideAICombat { overridor = overridor });
                return;
            }
            if (nextState is Wander)
            {
                outer.SetNextState(new OverrideAIWander { overridor = overridor });
                return;
            }
            if (nextState is LookBusy)
            {
                outer.SetNextState(new OverrideAILookBusy { overridor = overridor });
                return;
            }
        }
    }
}
