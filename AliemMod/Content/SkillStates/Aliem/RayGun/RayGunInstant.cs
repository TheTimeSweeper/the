using AliemMod.Components;
using AliemMod.Content.SkillDefs;
using EntityStates;
using UnityEngine.Networking;

namespace ModdedEntityStates.Aliem {
    public class RayGunInstant : RayGunFire, IHasSkillDefComponent<PassiveBuildupComponent> {

        public PassiveBuildupComponent componentFromSkillDef1 { get; set; }

        private float _buildup;

        protected override void ModifyState() {

			attackSoundString = "Play_INV_RayGun";

            if(componentFromSkillDef1 == null)
            {
                componentFromSkillDef1 = GetComponent<PassiveBuildupComponent>();
            }
            if (componentFromSkillDef1)
            {
                base.damageCoefficient = componentFromSkillDef1.RedeemCharge();
            }
            else
            {
                damageCoefficient = 0.6f;
            }
		}

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Any;
        }
    }
}