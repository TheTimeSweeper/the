using EntityStates;

namespace ModdedEntityStates.Aliem
{
    public class InputSawedOff : MashAndHoldInputs
    {
        protected override EntityState newMashState => new ShootSawedOffUncharged();

        protected override EntityState newHoldState => new ChargeSawedOff { activatorSkillSlot = activatorSkillSlot };
    }
}