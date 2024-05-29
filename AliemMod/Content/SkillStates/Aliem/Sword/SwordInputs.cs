using EntityStates;

namespace ModdedEntityStates.Aliem
{
    public class SwordInputs : MashAndHoldInputs
    {
        protected override EntityState initialMashState => new SwordFire();

        protected override EntityState mashState => new SwordFire();

        protected override EntityState holdState => new SwordCharging();
    }
}