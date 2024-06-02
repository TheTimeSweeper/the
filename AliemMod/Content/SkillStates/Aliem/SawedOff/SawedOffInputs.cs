using EntityStates;

namespace ModdedEntityStates.Aliem
{
    public class SawedOffInputs : MashAndHoldInputs
    {
        protected override EntityState newMashState => new ShootSawedOffUncharged();

        protected override EntityState newHoldState => new ChargeSawedOff();
    }
}