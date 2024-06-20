using EntityStates;

namespace ModdedEntityStates.Aliem {
    public class InputRayGun : MashAndHoldInputs {

        protected override EntityState initialMashState => new RayGunFireUncharged();
        
        protected override EntityState newMashState => new RayGunFireUncharged();
		protected override InterruptPriority mashInterruptPriority => InterruptPriority.Any;
		protected override EntityState newHoldState => new ChargeRayGun { activatorSkillSlot = activatorSkillSlot };
		protected override InterruptPriority holdInterruptPriority => InterruptPriority.Any;
        
		protected override bool RepeatHoldState => false;
    }
}
