using AliemMod.Components;
using AliemMod.Content.SkillDefs;
using EntityStates;
using UnityEngine.Networking;

namespace ModdedEntityStates.Aliem {
    public class RayGunInstant : RayGunFire, IHasSkillDefComponent<PassiveBuildupComponent> {

        public PassiveBuildupComponent componentFromSkillDef1 { get; set; }

        public static float builldupDamageCOefficient = 2;

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
            damageCoefficient *= builldupDamageCOefficient;

        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Any;
        }
    }
}