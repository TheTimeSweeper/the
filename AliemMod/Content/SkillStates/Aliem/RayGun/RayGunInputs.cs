using EntityStates;

namespace ModdedEntityStates.Aliem {
    public class RayGunInputs : MashAndHoldInputs {

        protected override EntityState initialMashState => new RayGunFire();
        
        protected override EntityState mashState => new RayGunFireUncharged();
		protected override InterruptPriority mashInterruptPriority => InterruptPriority.Any;
		protected override EntityState holdState => new RayGunCharging();
		protected override InterruptPriority holdInterruptPriority => InterruptPriority.Skill;

		protected override bool RepeatHoldState => false;
    }
}
