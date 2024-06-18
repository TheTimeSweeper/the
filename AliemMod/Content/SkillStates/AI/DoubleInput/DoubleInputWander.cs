using EntityStates;
using EntityStates.AI.Walker;

namespace ModdedEntityStates.Aliem.AI
{
    public class DoubleInputWander : Wander
    {
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
                outer.SetNextState(new DoubleInputWander());
                return;
            }
            if (nextState is LookBusy)
            {
                outer.SetNextState(new DoubleInputLookBusy());
                return;
            }
        }
    }
}
