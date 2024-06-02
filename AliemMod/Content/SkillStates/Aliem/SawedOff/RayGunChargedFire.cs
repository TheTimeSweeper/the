using AliemMod.Content;
using EntityStates;
using UnityEngine;

namespace ModdedEntityStates.Aliem
{
    public class ShootSawedOffCharged : RayGunFire {

        public override float BaseDuration => 1f;

        public override float BaseDamageCoefficient => _chargedDamageCoefficient;
        public override string soundString => _chargedShootSound;

        public override GameObject projectile => Modules.Projectiles.RayGunProjectilePrefabBig;

		private float _chargedDamageCoefficient;
        private string _chargedShootSound = "Play_AliemSawedOffCharged";

        public ShootSawedOffCharged() {
            _chargedDamageCoefficient = AliemConfig.M1_SawedOffCharged_Damage_Max.Value;
        }

        public ShootSawedOffCharged(float dam_) {
            _chargedDamageCoefficient = dam_;
        }

        protected override void ModifyState()
        {
            base.ModifyState();
            projectilePitchBonus = AliemConfig.shotgunPitch.Value;
        }

        public override void PlayAnimation(float duration) {
            base.PlayAnimation("Gesture, Override", "ShootGunBig", "ShootGun.playbackRate", duration*2);
            base.PlayAnimation(isOffHanded ? "RightArm, Over" : "LeftArm, Over", "ShootGunBig");
            base.PlayAnimation(isOffHanded ? "LeftArm, Under" : "RightArm, Under", "ShootGunBig");
        }

        public override InterruptPriority GetMinimumInterruptPriority() {
            return InterruptPriority.Skill;
        }
    }
}
