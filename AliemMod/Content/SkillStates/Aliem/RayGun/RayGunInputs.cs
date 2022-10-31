using EntityStates;

namespace ModdedEntityStates.Aliem {
    public class RayGunInputs : MashAndHoldInputs {

        protected override EntityState initialMashState => new RayGun();
        
        protected override EntityState mashState => new RayGun();
		protected override InterruptPriority mashInterruptPriority => InterruptPriority.Any;
		protected override EntityState holdState => new ChargeRayGunBig();
		protected override InterruptPriority holdInterruptPriority => InterruptPriority.Skill;

		protected override bool RepeatHoldState => false;
    }
}
