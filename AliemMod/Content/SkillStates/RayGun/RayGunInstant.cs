using AliemMod.Components;
using EntityStates;

namespace ModdedEntityStates.Aliem {
    public class RayGunInstant : RayGun {

        protected override void ModifyState() {

			attackSoundString = "Play_INV_RayGun";

            RayGunChargeComponent rayGunChargeComponent = GetComponent<RayGunChargeComponent>();
			if (rayGunChargeComponent) {
				base.damageCoefficient = rayGunChargeComponent.RedeemCharge();
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