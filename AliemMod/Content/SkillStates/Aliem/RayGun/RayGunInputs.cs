using EntityStates;

namespace ModdedEntityStates.Aliem {
    public class RayGunInputs : MashAndHoldInputs {

        protected override EntityState initialMashState => new RayGunFireUncharged();
        
        protected override EntityState newMashState => new RayGunFire();
		protected override InterruptPriority mashInterruptPriority => InterruptPriority.Any;
		protected override EntityState newHoldState => new ChargeRayGun();
		protected override InterruptPriority holdInterruptPriority => InterruptPriority.Skill;

		protected override bool RepeatHoldState => false;
    }
}
