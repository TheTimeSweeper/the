using EntityStates;

namespace ModdedEntityStates.Aliem
{
    public class InputRifle : MashAndHoldInputs
    {
        protected override EntityState newMashState => new ShootRifleUncharged();

        protected override EntityState newHoldState => new ChargeRifle();
    }
}