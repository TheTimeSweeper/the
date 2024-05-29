using AliemMod.Components;
using AliemMod.Content.SkillDefs;
using EntityStates;

namespace ModdedEntityStates.Aliem {
    public class RayGunInstant : RayGunFire, IHasSkillDefComponent<PassiveBuildupComponent> {
        public PassiveBuildupComponent componentFromSkillDef1 { get; set; }

        protected override void ModifyState() {

			attackSoundString = "Play_INV_RayGun";

			if (componentFromSkillDef1) {
				base.damageCoefficient = componentFromSkillDef1.RedeemCharge();
			}
			else {
				damageCoefficient = 0.6f;
            }
		}

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Any;
        }
    }
}