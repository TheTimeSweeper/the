using EntityStates;

namespace ModdedEntityStates.Aliem
{
    public class InputSword : MashAndHoldInputs
    {
        protected override EntityState initialMashState => new SwordFire();

        protected override EntityState newMashState => new SwordFire();

        protected override EntityState newHoldState => new ChargeSword { activatorSkillSlot = activatorSkillSlot };
    }
}